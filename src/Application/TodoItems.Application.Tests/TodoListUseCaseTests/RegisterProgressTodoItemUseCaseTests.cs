using Moq;
using TodoItems.Application.UseCases.TodoListUseCases.RegisterProgressTodoItem;
using TodoItems.Domain.Aggregates.TodoListAggregate;
using TodoItems.Domain.Aggregates.TodoListAggregate.ValeObjects;

namespace TodoItems.Application.Tests.TodoListUseCaseTests;

public class RegisterProgressTodoItemUseCaseTests
{
    [Fact]
    public async Task Execute_ShouldRegisterProgressionTodoItem()
    {
        // Arrange
        var itemId = new Random().Next(1, 1000);
        var newItemId = itemId + 1;
        var category = new Category(Guid.NewGuid().ToString());
        var todoList = new TodoList(Guid.NewGuid().ToString());
        todoList.AddItem(itemId, Guid.NewGuid().ToString(), category);

        var repoMock = new Mock<ITodoListRepository>();

        repoMock.Setup(r => r.GetTodoListByIdAsync(todoList.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(todoList);

        var useCase = new RegisterProgressTodoItemCommandHandler(repoMock.Object);

        // Act
        await useCase.Handle(new RegisterProgressTodoItemCommand(todoList.Id, itemId, DateTime.Now, 25), CancellationToken.None);

        // Assert
        repoMock.Verify(r => r.SaveAsync(
            It.Is<TodoList>(tl => tl.Items
                .Any(i => i.Id == itemId && i.Progressions
                    .Any(p => p.Percent == 25))), 
            It.IsAny<CancellationToken>()), Times.Once);
    }
}
