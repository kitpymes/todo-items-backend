using MediatR;
using TodoItems.Domain._Common;
using TodoItems.Domain._Common.ShadowProperties;
using TodoItems.Infrastructure.Persistence;

namespace TodoItems.Infrastructure.Extensions;

static class MediatorExtension
{
    public static async Task DispatchDomainEventsAsync(this IMediator mediator, ItemDbContext context)
    {
        var domainEntities = context.ChangeTracker.Entries<IEntityBase>()
            .Where(x => x.Entity is not INotMapped && x.Entity.DomainEvents.Count > 0);

        if (domainEntities.Any())
        {
            var domainEvents = domainEntities.SelectMany(x => x.Entity.DomainEvents).ToList();

            foreach (var domainEvent in domainEvents)
            {
                await mediator.Publish(domainEvent);
            }

            domainEntities.ToList().ForEach(entity => entity.Entity.ClearDomainEvents());
        }
    }
}