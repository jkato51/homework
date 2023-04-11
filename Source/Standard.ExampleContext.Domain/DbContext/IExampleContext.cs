namespace Standard.ExampleContext.Domain.DbContext;

public interface IExampleContext : IDisposable
{
    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}