using Microsoft.EntityFrameworkCore;
using Standard.ExampleContext.Domain.DbContext;
using Standard.ExampleContext.Domain.Repositories;
using Standard.ExampleContext.Infrastructure.Repositories;

namespace Standard.ExampleContext.Infrastructure.DbContext;

public class DbContextFactory : IDbContextFactory
{
    private readonly DbContextOptions<ExampleContext> _dbContextOptions;

    public DbContextFactory(DbContextOptions<ExampleContext> dbContextOptions)
    {
        _dbContextOptions = dbContextOptions;
    }

    public IExampleContext CreateContext()
    {
        return new ExampleContext(_dbContextOptions);
    }

    public IExampleRepository CreateExampleRepository(IExampleContext context)
    {
        return new ExampleRepository((ExampleContext)context);
    }
}