using MediatR;
using TodoItems.Domain._Common.AppResults;
using TodoItems.Domain.Aggregates.TodoListAggregate.Interfaces;

namespace TodoItems.Application.TodoList.UseCases.Commands;

public record RemoveItemCommand(Guid TodoListId, int ItemId) : IRequest<IAppResult>;

public class RemoveItemCommandHandler(ITodoListRepository repository) : IRequestHandler<RemoveItemCommand, IAppResult>
{
    private readonly ITodoListRepository _repository = repository;

    public async Task<IAppResult> Handle(RemoveItemCommand request, CancellationToken cancellationToken)
    {
        var todoList = await _repository.GetTodoListByIdAsync(request.TodoListId, cancellationToken);

        if (todoList is null)
            return AppResult.BadRequest($"La lista de tareas con Id {request.TodoListId} no fue encontrada.");

        todoList.RemoveItem(request.ItemId);

        await _repository.SaveAsync(todoList, cancellationToken);

        return AppResult.Success();
    }
}