using Standard.ExampleContext.Application.Dtos;

namespace Standard.ExampleContext.Application.Facades.Interfaces;

public interface IExampleFacade
{
    Task<ExampleResponseDto> GetByFilterAsync(ExampleFilterDto filterDto);
    Task<PaginationDto<ExampleResponseDto>> GetListByFilterAsync(ExampleFilterDto filterDto);
    Task<long> CreateAsync(ExampleRequestDto exampleRequestDto);
    Task UpdateAsync(long id, ExampleRequestDto exampleRequestDto);
    Task DeleteAsync(long id);
}