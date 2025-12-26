using FluentAssertions;
using Moq;
using TodoItems.Domain.Aggregates.TodoListAggregate;
using TodoItems.Domain.Aggregates.TodoListAggregate.Interfaces;

namespace TodoItems.Application.Tests.Item;

public class ItemCreatedEventHandlerTests
{
    [Fact]
    public async Task Handling_ItemCreatedEvent_ShouldRegisterInitialProgress()
    {
        // Arrange
        //var todoListId = Guid.NewGuid();
        //var itemId = new Random().Next(1, 1000);
        //var todoList = new Domain.Aggregates.TodoListAggregate.TodoList();

        //todoList.AddItem(itemId, "Test Item", "Description", new Domain.Aggregates.TodoListAggregate.ValeObjects.Category("General"));

        //var todoListMock = new Mock<ITodoList>();
        //todoListMock.Setup(t => t.GetItemById(itemId)).Returns(todoList.Items.First(i => i.Id == itemId));


        //var repoMock = new Mock<ITodoListRepository>();
        //repoMock.Setup(r => r.GetByIdAsync(todoListId, CancellationToken.None)).ReturnsAsync(todoList);

        //var handler = new ItemCreatedEventHandler(repoMock.Object);

        //// Act
        //await handler.Handle(new ItemCreatedEvent(item.Id), CancellationToken.None);

        //// Assert
        //item.Progressions.Should().ContainSingle(p => p.Percent == 0);
    }
}
