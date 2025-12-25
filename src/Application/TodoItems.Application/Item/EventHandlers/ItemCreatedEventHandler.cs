
using MediatR;
using System.ComponentModel.DataAnnotations;
using TodoItems.Domain._Common.Events;
using TodoItems.Domain._Common.Interfaces;

namespace TodoItems.Application.Item.EventHandlers;

public class ItemCreatedEventHandler(IItemRepository repository) : INotificationHandler<ItemCreatedEvent>
{
    private readonly IItemRepository _repository = repository;

    public async Task Handle(ItemCreatedEvent notification, CancellationToken cancellationToken)
    {
        var item = await _repository.GetByIdAsync(notification.ItemId, cancellationToken);

        if (item is null)
        {
            throw new ValidationException("Item not found.");
        }

        item.RegisterProgression(0m);
    }
}
