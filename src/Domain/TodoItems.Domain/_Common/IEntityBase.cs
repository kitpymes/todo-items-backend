using MediatR;
using System.ComponentModel.DataAnnotations.Schema;

namespace TodoItems.Domain._Common;

public interface IEntityBase
{
    [NotMapped]
    public IReadOnlyCollection<INotification> DomainEvents { get; }

    public void AddDomainEvent(INotification domainEvent);

    public void RemoveDomainEvent(INotification domainEvent);

    public void ClearDomainEvents();
}
