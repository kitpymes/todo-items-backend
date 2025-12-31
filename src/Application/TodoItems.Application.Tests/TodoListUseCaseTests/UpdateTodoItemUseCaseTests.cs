using Moq;
using TodoItems.Application.UseCases.TodoListUseCases.UpdateTodoItem;
using TodoItems.Domain.Aggregates.TodoListAggregate;
using TodoItems.Domain.Aggregates.TodoListAggregate.ValeObjects;

namespace TodoItems.Application.Tests.TodoListUseCaseTests;

public class UpdateTodoItemUseCaseTests
{
    [Fact]
    public async Task Execute_ShouldUpdateTodoItemTitle()
    {
        // Arrange
        var itemId = new Random().Next(1, 1000);
        var category = new Category(Guid.NewGuid().ToString());
        var title = Guid.NewGuid().ToString();
        var newTitle = Guid.NewGuid().ToString();
        var todoList = new TodoList(Guid.NewGuid().ToString());
        todoList.AddItem(itemId, title, category);

        var repoMock = new Mock<ITodoListRepository>();

        repoMock.Setup(r => r.GetTodoListByIdAsync(todoList.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(todoList);

        var useCase = new UpdateTodoItemCommandHandler(repoMock.Object);

        var request = new UpdateTodoItemCommand(todoList.Id, itemId, newTitle, null);

        // Act
        var result = await useCase.Handle(request, CancellationToken.None);

        // Assert
        repoMock.Verify(r => r.SaveAsync(
            It.Is<TodoList>(tl => tl.Items.Any(i => i.Title == newTitle)),
            It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Execute_ShouldUpdateTodoItemDescription()
    {
        // Arrange
        var itemId = new Random().Next(1, 1000);
        var category = new Category(Guid.NewGuid().ToString());
        var description = Guid.NewGuid().ToString();
        var newDescription = Guid.NewGuid().ToString();
        var todoList = new TodoList(Guid.NewGuid().ToString());
        todoList.AddItem(itemId, Guid.NewGuid().ToString(), category, description);

        var repoMock = new Mock<ITodoListRepository>();

        repoMock.Setup(r => r.GetTodoListByIdAsync(todoList.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(todoList);

        var useCase = new UpdateTodoItemCommandHandler(repoMock.Object);

        var request = new UpdateTodoItemCommand(todoList.Id, itemId, Guid.NewGuid().ToString(), newDescription);

        // Act
        var result = await useCase.Handle(request, CancellationToken.None);

        // Assert
        repoMock.Verify(r => r.SaveAsync(
            It.Is<TodoList>(tl => tl.Items.Any(i => i.Description == newDescription)),
            It.IsAny<CancellationToken>()), Times.Once);
    }
}
