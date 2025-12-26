using AutoMapper;
using MediatR;
using TodoItems.Domain._Common.AppResults;
using TodoItems.Domain.Aggregates.TodoListAggregate.Interfaces;

namespace TodoItems.Application.TodoList.UseCases.Queries;

public record GetTodoListQuery() : IRequest<IAppResult>;

public class GetTodoListQueryHandler(ITodoListRepository repository, IMapper mapper) : IRequestHandler<GetTodoListQuery, IAppResult>
{
    private readonly ITodoListRepository _repository = repository;
    private readonly IMapper _mapper = mapper;

    public async Task<IAppResult> Handle(GetTodoListQuery request, CancellationToken cancellationToken)
    {
        var items = await _repository.GetAllTodoListAsync(cancellationToken);

        return AppResult.Success(items);
    }
}
