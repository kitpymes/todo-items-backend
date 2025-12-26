namespace TodoItems.Domain.Aggregates.TodoListAggregate.Interfaces;

/* REQUERIDO: Es necesario que definas también un repositorio que tenga la siguiente interfaz que servirá para pedir el siguiente Id
    a utilizar para crear un TodoItem y para poder validar que la category pasada este dentro de las categories del sistema:

    public interface ITodoListRepository
    {
        int GetNextId();
        List<string> GetAllCategories();
    }
 */

public interface ITodoListRepository
{
    Task<int> GetNextItemIdAsync();

    Task<IReadOnlyCollection<string>> GetAllCategoriesAsync(CancellationToken cancellationToken);

    Task<IReadOnlyCollection<TodoList>> GetAllTodoListAsync(CancellationToken cancellationToken);

    Task<TodoList?> GetTodoListByIdAsync(Guid todoListId, CancellationToken cancellationToken);

    Task<IReadOnlyCollection<TodoItem>> GetAllItemsAsync(Guid todoListId, CancellationToken cancellationToken);
    
    Task SaveAsync(TodoList todoList, CancellationToken cancellationToken);




    Task<IReadOnlyCollection<TodoItem>> GetAllAsync(CancellationToken cancellationToken);

    Task AddAsync(TodoItem item, CancellationToken cancellationToken);

    Task UpdateAsync(TodoItem item, CancellationToken cancellationToken);

    Task RemoveAsync(TodoItem item, CancellationToken cancellationToken);
}
