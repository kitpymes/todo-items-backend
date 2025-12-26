using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Xml.Linq;
using TodoItems.Application.TodoList.DTOs;
using TodoItems.Domain.Aggregates.TodoListAggregate;
using TodoItems.Domain.Aggregates.TodoListAggregate.Interfaces;

namespace TodoItems.Infrastructure.Persistence.Repositories;

public class TodoListRepository(TodoListDbContext context) : ITodoListRepository
{
    private readonly TodoListDbContext _context = context;

    public async Task<int> GetNextItemIdAsync()
        => (await _context.Set<TodoItem>().MaxAsync(x => (int?)x.Id) ?? 0) + 1;

    public async Task<IReadOnlyCollection<string>> GetAllCategoriesAsync(CancellationToken cancellationToken)
        => await _context.Set<TodoItem>()
            .Select(x => x.Category.Name)
            .Distinct()
            .ToListAsync(cancellationToken);

    public async Task<IReadOnlyCollection<TodoList>> GetAllTodoListAsync(CancellationToken cancellationToken)
        => await _context.TodoLists
            .Include(x => x.Items)
            .OrderBy(x => x.Id)
            .ToListAsync(cancellationToken);

    public async Task<TodoList?> GetTodoListByIdAsync(Guid todoListId, CancellationToken cancellationToken)
        => await _context.TodoLists
            .Include(x => x.Items)
                .ThenInclude(i => i.Progressions)
            .FirstOrDefaultAsync(x => x.Id == todoListId, cancellationToken);

    public async Task<IReadOnlyCollection<TodoItem>> GetAllItemsAsync(Guid todoListId, CancellationToken cancellationToken)
    => await _context.TodoLists
        .Where(l => l.Id == todoListId)
        .SelectMany(l => l.Items)
        .OrderBy(i => i.Id)
        .ToListAsync(cancellationToken);

    public async Task SaveAsync(TodoList todoList, CancellationToken cancellationToken)
    {
        if (_context.Entry(todoList).State == EntityState.Detached)
        {
            _context.TodoLists.Add(todoList);
        }

        await _context.SaveChangesAsync(cancellationToken);
    }



            

    public Task AddAsync(TodoItem item, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task UpdateAsync(TodoItem item, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task RemoveAsync(TodoItem item, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<IReadOnlyCollection<TodoItem>> GetAllAsync(CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    //public async Task<IReadOnlyCollection<TodoItem>> GetAllAsync(CancellationToken cancellationToken)
    //    => await _context.Items.ToListAsync(cancellationToken);

    //public async Task<TodoItem?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    //    => await _context.Items
    //        .Include(i => i.Progressions)
    //        .FirstOrDefaultAsync(i => i.Id == id, cancellationToken);

    //public async Task AddAsync(TodoItem item, CancellationToken cancellationToken)
    //{
    //    await _context.Items.AddAsync(item, cancellationToken);

    //    await _context.SaveChangesAsync(cancellationToken);
    //}

    //public async Task UpdateAsync(TodoItem item, CancellationToken cancellationToken)
    //{
    //    _context.Items.Update(item);

    //    await _context.SaveChangesAsync(cancellationToken);
    //}

    //public async Task RemoveAsync(TodoItem item, CancellationToken cancellationToken)
    //{
    //    _context.Items.Remove(item);

    //    await _context.SaveChangesAsync(cancellationToken);
    //}
}
