using MediatR;

namespace TodoItems.Domain._Common.Events;

public record class ItemCreatedEvent(Guid ItemId) : INotification;