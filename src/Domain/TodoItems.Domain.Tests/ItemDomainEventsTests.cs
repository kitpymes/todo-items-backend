using FluentAssertions;
using TodoItems.Domain._Common.Events;
using TodoItems.Domain.Entities;

namespace TodoItems.Domain.Tests;

public class ItemDomainEventsTests
{
    [Fact]
    public void CreatingItem_ShouldRaise_ItemCreatedEvent()
    {
        var item = new Item("Title", "Desc", "Cat");

        item.DomainEvents.Should().ContainSingle(e => e is ItemCreatedEvent);
    }
}
