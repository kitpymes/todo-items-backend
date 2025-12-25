namespace TodoItems.Domain._Common;

public abstract class EntityBaseGuid(Guid id) : EntityBase<Guid>(id)
{
    protected EntityBaseGuid(): this(Guid.NewGuid()) { }
}