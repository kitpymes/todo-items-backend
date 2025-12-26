using MediatR;
using TodoItems.Domain._Common.AppResults;
using TodoItems.Domain.Aggregates.TodoListAggregate.Interfaces;

namespace TodoItems.Application.TodoList.UseCases.Commands;

public record UpdateItemCommand(Guid TodoListId, int ItemId, string Description) : IRequest<IAppResult>;

public class UpdateItemCommandHandler(ITodoListRepository repository) : IRequestHandler<UpdateItemCommand, IAppResult>
{
    private readonly ITodoListRepository _repository = repository;

    public async Task<IAppResult> Handle(UpdateItemCommand request, CancellationToken cancellationToken)
    {
        var todoList = await _repository.GetTodoListByIdAsync(request.TodoListId, cancellationToken);

        if (todoList is null)
            return AppResult.BadRequest($"La lista de tareas con Id {request.TodoListId} no fue encontrada.");

        todoList.UpdateItem(request.ItemId, request.Description);

        await _repository.SaveAsync(todoList, cancellationToken);

        return AppResult.Success();
    }
}
