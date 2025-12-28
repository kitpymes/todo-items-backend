using FluentAssertions;
using Moq;
using TodoItems.Application.TodoList.UseCases.Commands;
using TodoItems.Domain.Aggregates.TodoListAggregate.Interfaces;
using TodoItems.Domain.Aggregates.TodoListAggregate.ValeObjects;

namespace TodoItems.Application.Tests.Item;

public class RegisterProgressItemUseCaseTests
{
    [Fact]
    public async Task Execute_ShouldRegisterProgression()
    {
        // Arrange
        var itemId = new Random().Next(1, 1000);
        var newItemId = itemId + 1;
        var category = new Category(Guid.NewGuid().ToString());
        var todoList = new Domain.Aggregates.TodoListAggregate.TodoList();
        todoList.AddItem(itemId, Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), category);

        var repoMock = new Mock<ITodoListRepository>();

        repoMock.Setup(r => r.GetTodoListByIdAsync(todoList.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(todoList);

        var useCase = new RegisterProgressItemCommandHandler(repoMock.Object);

        // Act
        await useCase.Handle(new RegisterProgressItemCommand(todoList.Id, itemId, 25), CancellationToken.None);

        // Assert
        repoMock.Verify(r => r.SaveAsync(
            It.Is<Domain.Aggregates.TodoListAggregate.TodoList>(tl => tl.Items
                .Any(i => i.Id == itemId && i.Progressions
                    .Any(p => p.Percent == 25))), 
            It.IsAny<CancellationToken>()), Times.Once);
    }
}
