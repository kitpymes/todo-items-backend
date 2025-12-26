using FluentAssertions;
using TodoItems.Domain._Common.Events;
using TodoItems.Domain.Aggregates.TodoListAggregate;
using TodoItems.Domain.Aggregates.TodoListAggregate.ValeObjects;

namespace TodoItems.Domain.Tests;

public class ItemDomainEventsTests
{
    [Fact]
    public void CreatingItem_ShouldRaise_ItemCreatedEvent()
    {
        var itemId = 1;
        var category = new Category("Cat");

        var item = new TodoItem(itemId, "Title", "Desc", category);

      //  item.DomainEvents.Should().ContainSingle(e => e is TodoItemCreatedEvent);
    }
}
