using AutoMapper;
using Moq;
using TodoItems.Application.TodoList.UseCases.Commands;
using TodoItems.Domain.Aggregates.TodoListAggregate;
using TodoItems.Domain.Aggregates.TodoListAggregate.Interfaces;
using TodoItems.Domain.Aggregates.TodoListAggregate.ValeObjects;

namespace TodoItems.Application.Tests.Item;

public class AddItemUseCaseTests
{
    [Fact]
    public async Task Execute_ShouldAddItem()
    {
        // Arrange
        var todoListId = Guid.NewGuid();
        var itemId = new Random().Next(1, 1000);
        var request = new AddItemCommand(todoListId, Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), Guid.NewGuid().ToString());

        var mapperMock = new Mock<IMapper>();
        var repoMock = new Mock<ITodoListRepository>();
        var todoListMock = new Mock<ITodoList>();

        repoMock.Setup(r => r.GetTodoListByIdAsync(todoListId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new Domain.Aggregates.TodoListAggregate.TodoList());

        repoMock.Setup(r => r.GetAllCategoriesAsync(It.IsAny<CancellationToken>()));

        repoMock.Setup(r => r.GetNextItemIdAsync()).ReturnsAsync(itemId); 

        todoListMock
            .Setup(m => m. AddItem(itemId, It.IsAny<string>(), It.IsAny<string>(), It.IsAny<Category>()))
            .Verifiable();

        var useCase = new AddItemCommandHandler(todoListMock.Object, repoMock.Object, mapperMock.Object);

        // Act
        await useCase.Handle(request, CancellationToken.None);

        // Assert
        repoMock.Verify(
           r => r.AddAsync(It.Is<TodoItem>(item =>
               item.Id == itemId &&
               item.Title == request.Title &&
               item.Description == request.Description &&
               item.Category.Name == request.Category
           ), CancellationToken.None),
           Times.Once);
    }
}
