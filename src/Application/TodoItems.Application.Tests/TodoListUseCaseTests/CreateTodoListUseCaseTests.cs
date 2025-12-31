using Moq;
using TodoItems.Application.UseCases.TodoListUseCases.CreateTodoList;
using TodoItems.Domain.Aggregates.TodoListAggregate;

namespace TodoItems.Application.Tests.TodoListUseCaseTests;

public class CreateTodoListUseCaseTests
{
    [Fact]
    public async Task Execute_ShouldCreateTodoList()
    {
        // Arrange
        var title = Guid.NewGuid().ToString();
        var description = Guid.NewGuid().ToString();

        var repoMock = new Mock<ITodoListRepository>();

        var useCase = new CreateTodoListCommandHandler(repoMock.Object);

        var request = new CreateTodoListCommand(title, description);

        // Act
        var result = await useCase.Handle(request, CancellationToken.None);

        // Assert
        repoMock.Verify(r => r.SaveAsync(
        It.Is<TodoList>(tl =>            
            tl.Title == title &&
            tl.Description == description),
        It.IsAny<CancellationToken>()),
        Times.Once);
    }
}
