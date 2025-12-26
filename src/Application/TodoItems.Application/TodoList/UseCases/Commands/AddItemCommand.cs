using MediatR;
using TodoItems.Domain._Common.AppResults;
using TodoItems.Domain.Aggregates.TodoListAggregate.Interfaces;
using TodoItems.Domain.Aggregates.TodoListAggregate.ValeObjects;

namespace TodoItems.Application.TodoList.UseCases.Commands;

public record AddItemCommand(Guid TodoListId, string Title, string Description, string Category) : IRequest<IAppResult>;

public class AddItemCommandHandler(ITodoListRepository repository) : IRequestHandler<AddItemCommand, IAppResult>
{
    private readonly ITodoListRepository _repository = repository;

    public async Task<IAppResult> Handle(AddItemCommand request, CancellationToken cancellationToken)
    {
        var todoList = await _repository.GetTodoListByIdAsync(request.TodoListId, cancellationToken);

        if (todoList is null)
            return AppResult.BadRequest($"La lista de tareas con Id {request.TodoListId} no fue encontrada.");

        var categories = await _repository.GetAllCategoriesAsync(cancellationToken);

        if (!categories.Contains(request.Category))
            return AppResult.BadRequest($"La categoría '{request.Category}' no existe. Categorías disponibles: {string.Join(", ", categories)}");

        int newItemId = await _repository.GetNextItemIdAsync();

        var category = new Category(request.Category);

        todoList.AddItem(newItemId, request.Title, request.Description, category);

        await _repository.SaveAsync(todoList, cancellationToken);

        return AppResult.Success(newItemId);
    }
}
