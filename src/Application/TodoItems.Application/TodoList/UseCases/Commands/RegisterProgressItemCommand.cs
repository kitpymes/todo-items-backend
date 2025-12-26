using MediatR;
using System.Net;
using TodoItems.Domain._Common.AppResults;
using TodoItems.Domain.Aggregates.TodoListAggregate.Interfaces;

namespace TodoItems.Application.TodoList.UseCases.Commands;

public record RegisterProgressItemCommand(Guid TodoListId, int ItemId, decimal Percent, DateTime? Date = null) : IRequest<IAppResult>;

public class RegisterProgressItemCommandHandler(ITodoListRepository repository) : IRequestHandler<RegisterProgressItemCommand, IAppResult>
{
    private readonly ITodoListRepository _repository = repository;

    public async Task<IAppResult> Handle(RegisterProgressItemCommand request, CancellationToken cancellationToken)
    {
        var todoList = await _repository.GetTodoListByIdAsync(request.TodoListId, cancellationToken);

        if (todoList is null)
            return AppResult.BadRequest($"La lista de tareas con Id {request.TodoListId} no fue encontrada.");

        var registrationDate = request.Date ?? DateTime.UtcNow;

        todoList.RegisterProgression(request.ItemId, registrationDate, request.Percent);

        await _repository.SaveAsync(todoList, cancellationToken);

        return AppResult.Success();
    }
}