using FluentAssertions;
using Moq;
using TodoItems.Application.Item.UseCases.Commands;
using TodoItems.Domain._Common.Interfaces;

namespace TodoItems.Application.Tests.Item;

public class RegisterProgressionUseCaseTests
{
    [Fact]
    public async Task Execute_ShouldRegisterProgression()
    {
        // Arrange
        var item = new Domain.Entities.Item("Title", "Old", "Cat");

        var repoMock = new Mock<IItemRepository>();
        repoMock.Setup(r => r.GetByIdAsync(item.Id, CancellationToken.None)).ReturnsAsync(item);

        var useCase = new RegisterProgressionCommandHandler(repoMock.Object);

        // Act
        await useCase.Handle(new RegisterProgressionCommand(item.Id, 25), CancellationToken.None);

        // Assert
        item.Progressions.Should().ContainSingle(p => p.Percent == 25);
        item.Progressions.Should().HaveCount(1);
    }
}
