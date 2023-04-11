using Standard.ExampleContext.Domain.Repositories;

namespace Standard.ExampleContext.Domain.DbContext;

public interface IDbContextFactory
{
    IExampleContext CreateContext();
    IExampleRepository CreateExampleRepository(IExampleContext context);
}