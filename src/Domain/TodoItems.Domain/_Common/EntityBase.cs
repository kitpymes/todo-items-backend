
using System.Diagnostics.CodeAnalysis;
using MediatR;

namespace TodoItems.Domain._Common;

public abstract class EntityBase<TKey> : IEquatable<TKey>, IEntityBase
{
#pragma warning disable CS8618 // Un campo que no acepta valores NULL debe contener un valor distinto de NULL al salir del constructor. Considere la posibilidad de declararlo como que admite un valor NULL.
    protected EntityBase() { }
#pragma warning restore CS8618 // Un campo que no acepta valores NULL debe contener un valor distinto de NULL al salir del constructor. Considere la posibilidad de declararlo como que admite un valor NULL.

    protected EntityBase(TKey key) => Id = key;

    public virtual TKey Id { get; protected set; }

    #region DomainEvents

    private readonly List<INotification> _domainEvents = [];

    public IReadOnlyCollection<INotification> DomainEvents => _domainEvents.AsReadOnly();

    public void AddDomainEvent(INotification domainEvent) => _domainEvents.Add(domainEvent);

    public void RemoveDomainEvent(INotification domainEvent) => _domainEvents.Remove(domainEvent);

    public void ClearDomainEvents() => _domainEvents.Clear();

    #endregion DomainEvents

    #region Equals

    public static bool operator ==(EntityBase<TKey> left, EntityBase<TKey> right)
    {
        if (left is null && right is null)
        {
            return true;
        }

        if (left is null || right is null)
        {
            return false;
        }

        return left.Equals(right);
    }

    public static bool operator !=(EntityBase<TKey> left, EntityBase<TKey> right) => !(left == right);

    public override bool Equals(object? obj)
    {
        if (obj is null)
        {
            return false;
        }

        return obj is EntityBase<TKey> entity && Equals(entity.Id);
    }

    public bool Equals([AllowNull] TKey other) => Id?.Equals(other) ?? false;

    public override int GetHashCode() => (GetType().Name + Id).GetHashCode(StringComparison.OrdinalIgnoreCase);

    #endregion Equals
}
