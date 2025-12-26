using MediatR;
using TodoItems.Domain._Common;
using TodoItems.Domain._Common.ShadowProperties;
using TodoItems.Infrastructure.Persistence;

namespace TodoItems.Infrastructure.Extensions;

static class MediatorExtension
{
    public static async Task DispatchDomainEventsAsync(this IMediator mediator, TodoListDbContext context)
    {
        var entitiesWithEvents = context.ChangeTracker.Entries<IAggregateRoot>()
           .Where(x => x.Entity is not INotMapped && x.Entity.DomainEvents.Count > 0)
           .Select(e => e.Entity)
           .ToList();

        foreach (var entity in entitiesWithEvents)
        {
            var events = entity.DomainEvents.ToList();

            entity.ClearDomainEvents();

            foreach (var domainEvent in events)
            {
                await mediator.Publish(domainEvent, CancellationToken.None);
            }
        }
    }
}