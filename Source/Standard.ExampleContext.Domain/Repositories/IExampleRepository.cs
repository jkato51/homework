using Standard.ExampleContext.Domain.Entities;
using Standard.ExampleContext.Domain.Models;

namespace Standard.ExampleContext.Domain.Repositories;

public interface IExampleRepository : IRepositoryBase<Example>
{
    Task<int> CountByFilterAsync(ExampleFilter filter);
    Task<Example> GetByFilterAsync(ExampleFilter filter);
    Task<List<Example>> GetListByFilterAsync(ExampleFilter filter);
}