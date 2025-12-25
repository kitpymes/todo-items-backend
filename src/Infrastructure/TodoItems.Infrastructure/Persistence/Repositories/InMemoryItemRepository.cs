using TodoItems.Domain._Common.Interfaces;
using TodoItems.Domain.Entities;

namespace TodoItems.Infrastructure.Persistence.Repositories;

public class InMemoryItemRepository : IItemRepository
{
    private readonly List<Item> _items = [];

    public async Task<IReadOnlyCollection<Item>> GetAllAsync(CancellationToken _) 
        => await Task.FromResult(_items);

    public async Task<Item?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
        => await Task.FromResult(_items.FirstOrDefault(i => i.Id == id));

    public async Task AddAsync(Item item, CancellationToken cancellationToken)
        => await Task.Run(() => _items.Add(item), cancellationToken);

    public async Task UpdateAsync(Item item, CancellationToken cancellationToken)
    {
        var editItem = _items.FirstOrDefault(x => x.Id == item.Id);

        if (editItem is not null)
        {
            _items.Remove(editItem);
            _items.Add(item);
        }

        await Task.CompletedTask;
    }

    public async Task RemoveAsync(Item item, CancellationToken cancellationToken)
        => await Task.Run(() => _items.Remove(item), cancellationToken);
}
