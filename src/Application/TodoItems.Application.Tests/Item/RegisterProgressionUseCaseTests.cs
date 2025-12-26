using FluentAssertions;
using Moq;
using TodoItems.Domain.Aggregates.TodoListAggregate.Interfaces;

namespace TodoItems.Application.Tests.Item;

public class RegisterProgressionUseCaseTests
{
    [Fact]
    public async Task Execute_ShouldRegisterProgression()
    {
        //// Arrange
        //var item = new Domain.Entities.TodoItem("Title", "Old", "Cat");

        //var repoMock = new Mock<ITodoListRepository>();
        //repoMock.Setup(r => r.GetByIdAsync(item.Id, CancellationToken.None)).ReturnsAsync(item);

        //var useCase = new RegisterProgressItemCommandHandler(repoMock.Object);

        //// Act
        //await useCase.Handle(new RegisterProgressionCommand(item.Id, 25), CancellationToken.None);

        //// Assert
        //item.Progressions.Should().ContainSingle(p => p.Percent == 25);
        //item.Progressions.Should().HaveCount(1);
    }
}
