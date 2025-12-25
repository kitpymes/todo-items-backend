using TodoItems.Domain.Entities;

namespace TodoItems.Domain._Common.Interfaces;

public interface IItemRepository
{
    Task<IReadOnlyCollection<Item>> GetAllAsync(CancellationToken cancellationToken);

    Task<Item?> GetByIdAsync(Guid id, CancellationToken cancellationToken);

    Task AddAsync(Item item, CancellationToken cancellationToken);

    Task UpdateAsync(Item item, CancellationToken cancellationToken);

    Task RemoveAsync(Item item, CancellationToken cancellationToken);
}
