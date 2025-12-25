using FluentAssertions;
using Moq;
using TodoItems.Application.Item.EventHandlers;
using TodoItems.Domain._Common.Events;
using TodoItems.Domain._Common.Interfaces;

namespace TodoItems.Application.Tests.Item;

public class ItemCreatedEventHandlerTests
{
    [Fact]
    public async Task Handling_ItemCreatedEvent_ShouldRegisterInitialProgress()
    {
        // Arrange
        var item = new Domain.Entities.Item("Title", "Desc", "Cat");

        var repoMock = new Mock<IItemRepository>();
        repoMock.Setup(r => r.GetByIdAsync(item.Id, CancellationToken.None)).ReturnsAsync(item);

        var handler = new ItemCreatedEventHandler(repoMock.Object);

        // Act
        await handler.Handle(new ItemCreatedEvent(item.Id), CancellationToken.None);

        // Assert
        item.Progressions.Should().ContainSingle(p => p.Percent == 0);
    }
}
