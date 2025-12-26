using AutoMapper;
using TodoItems.Application.TodoList.DTOs;
using TodoItems.Domain.Aggregates.TodoListAggregate;
using TodoItems.Domain.Aggregates.TodoListAggregate.ValeObjects;

namespace TodoItems.Application.TodoList.Mappings;

public class ItemMapping : Profile
{
    public ItemMapping()
    {
        CreateMap<TodoItem, TodoItemReportDto>().IncludeAllDerived();

        CreateMap<Progression, ProgressionDto>();
    }
}
