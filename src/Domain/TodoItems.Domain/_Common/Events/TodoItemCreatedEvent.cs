using MediatR;

namespace TodoItems.Domain._Common.Events;

public record class TodoItemCreatedEvent(Guid TodoListId, int ItemId) : INotification;