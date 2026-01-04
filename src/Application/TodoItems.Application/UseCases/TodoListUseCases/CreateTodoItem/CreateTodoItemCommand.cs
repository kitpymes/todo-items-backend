using MediatR;
using TodoItems.Application._Common.Exceptions;
using TodoItems.Domain._Common.AppResults;
using TodoItems.Domain.Aggregates.TodoListAggregate;
using TodoItems.Domain.Aggregates.TodoListAggregate.ValeObjects;

namespace TodoItems.Application.UseCases.TodoListUseCases.CreateTodoItem;

public record CreateTodoItemCommand(Guid TodoListId, string Title, string Category, string? Description) : IRequest<IAppResult>;

public class CreateTodoItemCommandHandler(ITodoListRepository repository) : IRequestHandler<CreateTodoItemCommand, IAppResult>
{
    private readonly ITodoListRepository _repository = repository;

    public async Task<IAppResult> Handle(CreateTodoItemCommand request, CancellationToken cancellationToken)
    {
        var todoList = await _repository.GetTodoListByIdAsync(request.TodoListId, cancellationToken);

        if (todoList is null)
            throw new AppValidationsException($"La lista de tareas con Id {request.TodoListId} no fue encontrada.");

        var categories = await _repository.GetAllCategoriesAsync(cancellationToken);

        if (!categories.Contains(request.Category))
            throw new AppValidationsException($"La categoría '{request.Category}' no existe. Categorías disponibles: {string.Join(", ", categories)}");

        var existsItemTitle = await _repository.ExistsAsync(x => x.Items.Any(i => i.Title == request.Title), cancellationToken);

        if (existsItemTitle)
            throw new AppValidationsException($"Ya existe una tarea con el mismo título: '{request.Title}'.");

        int newItemId = await _repository.GetNextItemIdAsync();

        var category = new Category(request.Category);

        todoList.AddItem(newItemId, request.Title, category, request.Description);

        await _repository.SaveAsync(todoList, cancellationToken);

        return AppResult.Success(x => x.WithData(newItemId));
    }
}
