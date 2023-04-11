using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Standard.ExampleContext.Domain.DbContext;
using Standard.ExampleContext.Infrastructure.DbContext;
using Standard.Api;
using Standard.ExampleContext.IntegrationTest.Seed;

namespace Standard.ExampleContext.IntegrationTest;

//for more about WebApplicationFactory: https://fullstackmark.com/post/20/painless-integration-testing-with-aspnet-core-web-api
public class CustomWebApplicationFactory : WebApplicationFactory<Program>
{
    private readonly string _connectionString = $"DataSource={Guid.NewGuid()}.db";

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            // Removing Existing Db Context
            var descriptor = services.SingleOrDefault(d =>
                d.ServiceType == typeof(DbContextOptions<Infrastructure.DbContext.ExampleContext>));
            if (descriptor != null)
                services.Remove(descriptor);

            var descriptorTwo = services.SingleOrDefault(d => d.ServiceType == typeof(IDbContextFactory));
            if (descriptorTwo != null)
                services.Remove(descriptorTwo);

            // Create a new service provider.
            var serviceProvider = new ServiceCollection()
                .AddEntityFrameworkSqlite()
                .BuildServiceProvider();

            var dbContextOptionsBuilder =
                new DbContextOptionsBuilder<Infrastructure.DbContext.ExampleContext>();
            dbContextOptionsBuilder.UseSqlite(_connectionString);
            services.AddTransient<IDbContextFactory>(sp => new DbContextFactory(dbContextOptionsBuilder.Options));

            // Add a database context (AppDbContext) using an in-memory database for testing.
            services.AddDbContext<Infrastructure.DbContext.ExampleContext>(options =>
            {
                options.UseSqlite(_connectionString);
                options.UseInternalServiceProvider(serviceProvider);
            });

            // Build the service provider.
            var buildServiceProvider = services.BuildServiceProvider();

            // Create a scope to obtain a reference to the database contexts
            using var scope = buildServiceProvider.CreateScope();
            var scopedServices = scope.ServiceProvider;
            var dbContext =
                scopedServices.GetRequiredService<Infrastructure.DbContext.ExampleContext>();

            var logger = scopedServices.GetRequiredService<ILogger<CustomWebApplicationFactory>>();

            // Ensure the database is created.
            dbContext.Database.EnsureCreated();

            try
            {
                // Seed the database with some specific test data.
                Task.Run(() => ExampleSeed.Populate(dbContext));
            }
            catch (Exception ex)
            {
                logger.LogError(ex,
                    $"An error occurred seeding the database with test messages. Error: {ex.Message}");
            }
        });
    }
}