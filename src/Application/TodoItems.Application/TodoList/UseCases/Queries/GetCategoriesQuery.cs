using AutoMapper;
using MediatR;
using TodoItems.Domain._Common.AppResults;
using TodoItems.Domain.Aggregates.TodoListAggregate.Interfaces;

namespace TodoItems.Application.TodoList.UseCases.Queries;

public record GetCategoriesQuery() : IRequest<IAppResult>;

public class GetCategoriesQueryHandler(ITodoListRepository repository, IMapper mapper) : IRequestHandler<GetCategoriesQuery, IAppResult>
{
    private readonly ITodoListRepository _repository = repository;
    private readonly IMapper _mapper = mapper;

    public async Task<IAppResult> Handle(GetCategoriesQuery request, CancellationToken cancellationToken)
    {
        var items = await _repository.GetAllCategoriesAsync(cancellationToken);

        return AppResult.Success(items);
    }
}
