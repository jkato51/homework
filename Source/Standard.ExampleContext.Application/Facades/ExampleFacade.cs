using AutoMapper;
using Standard.ExampleContext.Domain.Entities;
using Standard.ExampleContext.Domain.Models;
using Standard.ExampleContext.Domain.Services.Interfaces;
using Standard.ExampleContext.Application.Dtos;
using Standard.ExampleContext.Application.Facades.Interfaces;

namespace Standard.ExampleContext.Application.Facades;

public class ExampleFacade : IExampleFacade
{
    private readonly IExampleService _exampleService;
    private readonly IMapper _mapper;

    public ExampleFacade(IExampleService exampleService, IMapper mapper)
    {
        _exampleService = exampleService;
        _mapper = mapper;
    }

    public async Task<PaginationDto<ExampleResponseDto>> GetListByFilterAsync(ExampleFilterDto filterDto)
    {
        var filter = _mapper.Map<ExampleFilter>(filterDto);

        var result = await _exampleService.GetListByFilterAsync(filter);

        var paginationDto = _mapper.Map<PaginationDto<ExampleResponseDto>>(result);

        return paginationDto;
    }

    public async Task<ExampleResponseDto> GetByFilterAsync(ExampleFilterDto filterDto)
    {
        var filter = _mapper.Map<ExampleFilter>(filterDto);

        var result = await _exampleService.GetByFilterAsync(filter);

        var resultDto = _mapper.Map<ExampleResponseDto>(result);

        return resultDto;
    }

    public async Task UpdateAsync(long id, ExampleRequestDto exampleRequestDto)
    {
        var example = _mapper.Map<Example>(exampleRequestDto);

        await _exampleService.UpdateAsync(id, example);
    }

    public async Task<long> CreateAsync(ExampleRequestDto exampleRequestDto)
    {
        var example = _mapper.Map<Example>(exampleRequestDto);

        var id = await _exampleService.CreateAsync(example);

        return id;
    }

    public async Task DeleteAsync(long id)
    {
        await _exampleService.DeleteAsync(id);
    }
}