using System.IO.Compression;
using CorrelationId;
using CorrelationId.DependencyInjection;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting.WindowsServices;
using Microsoft.OpenApi.Models;
using NLog;
using NLog.Web;
using Standard.ExampleContext.Application.Facades;
using Standard.ExampleContext.Application.Facades.Interfaces;
using Standard.ExampleContext.Domain.DbContext;
using Standard.ExampleContext.Domain.Repositories;
using Standard.ExampleContext.Domain.Services;
using Standard.ExampleContext.Domain.Services.Interfaces;
using Standard.ExampleContext.Infrastructure.DbContext;
using Standard.ExampleContext.Infrastructure.Mappers;
using Standard.ExampleContext.Infrastructure.Repositories;
using Standard.Api.Middlewares;
using Swashbuckle.AspNetCore.SwaggerGen;

var builder = WebApplication.CreateBuilder(new WebApplicationOptions
{
    Args = args,
    ContentRootPath = WindowsServiceHelpers.IsWindowsService() ? AppContext.BaseDirectory : default
});

builder.Host.UseWindowsService();
builder.Host.UseNLog();
builder.Services.AddControllers(); 
builder.Services.AddDefaultCorrelationId(); 
AddApplicationServices(); 
AddDomainServices();
AddInfrastructureServices();
builder.Services.AddAutoMapper(typeof(ExampleMngtProfile));
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(SetupSwagger());
builder.Services.Configure<GzipCompressionProviderOptions>(options => options.Level = CompressionLevel.Optimal);
builder.Services.AddResponseCompression(options => { options.Providers.Add<GzipCompressionProvider>(); });

var dbContextOptionsBuilder = new DbContextOptionsBuilder<ExampleContext>();

dbContextOptionsBuilder.UseSqlServer(builder.Configuration.GetConnectionString("DbConnection"));

builder.Services.AddTransient<IDbContextFactory>(sp => new DbContextFactory(dbContextOptionsBuilder.Options));

builder.Services.AddDbContext<ExampleContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DbConnection"),
        a => { a.MigrationsAssembly("PDS.Standard.Api"); });
});

AddNLog();

await using var app = builder.Build();

app.UseCorrelationId();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Standard Api"));
    app.UseMiddleware<LogRequestMiddleware>();
    app.UseMiddleware<LogResponseMiddleware>();
}

app.UseHttpsRedirection();

app.MapControllers();

await app.RunAsync();

void AddApplicationServices()
{
    builder.Services.AddTransient<IExampleFacade, ExampleFacade>();
}

void AddDomainServices()
{
    builder.Services.AddTransient<IExampleService, ExampleService>();
    builder.Services.AddSingleton<IPasswordHasherService, PasswordHasherService>();
}

void AddInfrastructureServices()
{
    builder.Services.AddTransient<IExampleRepository, ExampleRepository>();
}

static Action<SwaggerGenOptions> SetupSwagger()
{
    return c =>
    {
        c.SwaggerDoc("v1", new OpenApiInfo { Title = "CF API", Version = "v1" });

        c.CustomSchemaIds(x => x.FullName);

        c.AddSecurityDefinition("Authorization", new OpenApiSecurityScheme
        {
            Type = SecuritySchemeType.Http,
            Scheme = "bearer",
            Description = "JWT Authorization header using the Bearer scheme",
            In = ParameterLocation.Header
        });

        c.AddSecurityRequirement(new OpenApiSecurityRequirement
        {
            {
                new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = "Authorization"
                    }
                },
                new List<string>()
            }
        });
    };
}

void AddNLog()
{
    if (builder.Environment.EnvironmentName.Contains("Test")) return;
    LogManager.Setup().LoadConfigurationFromAppSettings().GetCurrentClassLogger();
}

public partial class Program { }