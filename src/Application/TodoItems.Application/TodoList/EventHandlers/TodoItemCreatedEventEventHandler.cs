
using MediatR;
using TodoItems.Domain._Common.Events;
using TodoItems.Domain.Aggregates.TodoListAggregate.Interfaces;

namespace TodoItems.Application.TodoList.EventHandlers;

public class TodoItemCreatedEventEventHandler(ITodoListRepository repository) : INotificationHandler<TodoItemCreatedEvent>
{
    private readonly ITodoListRepository _repository = repository;

    public async Task Handle(TodoItemCreatedEvent notification, CancellationToken cancellationToken)
    {
        var todoList = await _repository.GetTodoListByIdAsync(notification.TodoListId, cancellationToken);

        if (todoList is not null)
        {
            todoList.RegisterProgression(notification.ItemId, DateTime.UtcNow, 0);

            await _repository.SaveAsync(todoList, cancellationToken);
        }        
    }
}
