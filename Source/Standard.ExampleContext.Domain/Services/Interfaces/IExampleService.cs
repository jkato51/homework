using Standard.ExampleContext.Domain.Entities;
using Standard.ExampleContext.Domain.Models;

namespace Standard.ExampleContext.Domain.Services.Interfaces;

public interface IExampleService
{
    Task<Pagination<Example>> GetListByFilterAsync(ExampleFilter filter);
    Task<Example> GetByFilterAsync(ExampleFilter filter);
    Task UpdateAsync(long id, Example example);
    Task<long> CreateAsync(Example example);
    Task DeleteAsync(long id);
}