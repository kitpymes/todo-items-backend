using AutoMapper;
using Moq;
using TodoItems.Application.Item.UseCases.Commands;
using TodoItems.Domain._Common.Interfaces;

namespace TodoItems.Application.Tests.Item;

public class AddItemUseCaseTests
{
    [Fact]
    public async Task Execute_ShouldAddItem()
    {
        // Arrange
        var request = new AddItemCommand("Title", "Old", "Cat");

        var mapperMock = new Mock<IMapper>();
        mapperMock.Setup(m => m.Map<Domain.Entities.Item>(It.IsAny<AddItemCommand>()))
            .Returns((AddItemCommand cmd) => new Domain.Entities.Item(cmd.Title, cmd.Description, cmd.Category));

        var repoMock = new Mock<IItemRepository>();       

        var useCase = new AddItemCommandHandler(repoMock.Object, mapperMock.Object);

        var item = mapperMock.Object.Map<Domain.Entities.Item>(request);

        // Act
        await useCase.Handle(request, CancellationToken.None);

        // Assert
        repoMock.Verify(
           r => r.AddAsync(It.Is<Domain.Entities.Item>(item =>
               item.Id == item.Id &&
               item.Title == request.Title &&
               item.Description == request.Description &&
               item.Category == request.Category
           ), CancellationToken.None),
           Times.Once);
    }
}
