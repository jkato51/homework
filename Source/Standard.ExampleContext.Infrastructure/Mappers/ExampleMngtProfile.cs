using AutoMapper;
using Standard.ExampleContext.Application.Dtos;
using Standard.ExampleContext.Domain.Entities;
using Standard.ExampleContext.Domain.Models;

namespace Standard.ExampleContext.Infrastructure.Mappers;

public class ExampleMngtProfile : Profile
{
    public ExampleMngtProfile()
    {
        ExampleProfile();
    }

    private void ExampleProfile()
    {
        CreateMap<ExampleRequestDto, Example>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.Created, opt => opt.Ignore())
            .ForMember(dest => dest.Updated, opt => opt.Ignore());

        CreateMap<Example, ExampleResponseDto>()
            .ForMember(dest => dest.FullName, opt => opt.Ignore())
            .AfterMap((source, destination) => { destination.FullName = source.GetFullName(); });

        CreateMap<ExampleFilterDto, ExampleFilter>();

        CreateMap<Pagination<Example>, PaginationDto<ExampleResponseDto>>()
            .AfterMap((source, converted, context) =>
            {
                converted.Result = context.Mapper.Map<List<ExampleResponseDto>>(source.Result);
            });
    }
}