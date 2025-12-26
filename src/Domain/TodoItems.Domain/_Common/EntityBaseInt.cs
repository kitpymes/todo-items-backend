namespace TodoItems.Domain._Common;

public abstract class EntityBaseInt : EntityBase<int>
{
    protected EntityBaseInt() { }

    protected EntityBaseInt(int id)
        : base(id) { }
}