using AutoMapper;
using TodoItems.Application.Item.DTOs;
using TodoItems.Application.Item.UseCases.Commands;

namespace TodoItems.Application.Item.Mappings;

public class ItemMapperProfile : Profile
{
    public ItemMapperProfile()
    {
        CreateMap<AddItemCommand, Domain.Entities.Item>();

        CreateMap<Domain.Entities.Item, ItemDto>();
    }
}
