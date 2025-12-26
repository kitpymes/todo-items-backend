using AutoMapper;
using MediatR;
using TodoItems.Application.TodoList.DTOs;
using TodoItems.Domain._Common.AppResults;
using TodoItems.Domain.Aggregates.TodoListAggregate.Interfaces;

namespace TodoItems.Application.TodoList.UseCases.Queries;

public record GetItemsByTodoListIdQuery(Guid TodoListId) : IRequest<IAppResult>;

public class GetItemsByTodoListIdQueryHandler(ITodoListRepository repository, IMapper mapper) 
    : IRequestHandler<GetItemsByTodoListIdQuery, IAppResult>
{
    private readonly ITodoListRepository _repository = repository;
    private readonly IMapper _mapper = mapper;

    public async Task<IAppResult> Handle(GetItemsByTodoListIdQuery request, CancellationToken cancellationToken)
    {
        var items = await _repository.GetAllItemsAsync(request.TodoListId, cancellationToken);

        //  var dtos = _mapper.Map<IReadOnlyCollection<TodoItemReportDto>>(items);

        var dtos = items
            .Select(i => new TodoItemReportDto(
                i.Id,
                i.Title,
                i.Description,
                i.Category.Name,
                [.. i.Progressions
                    .OrderByDescending(p => p.Date)
                    .Select(p => new ProgressionDto(p.Date, p.Percent))]
            ));

        return AppResult.Success(dtos);      
    }
}
