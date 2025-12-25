using Microsoft.EntityFrameworkCore;
using TodoItems.Domain._Common.Interfaces;
using TodoItems.Domain.Entities;

namespace TodoItems.Infrastructure.Persistence.Repositories;

public class ItemRepository(ItemDbContext context) : IItemRepository
{
    private readonly ItemDbContext _context = context;

    public async Task<IReadOnlyCollection<Item>> GetAllAsync(CancellationToken cancellationToken)
        => await _context.Items.ToListAsync(cancellationToken);

    public async Task<Item?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
        => await _context.Items
            .Include(i => i.Progressions)
            .FirstOrDefaultAsync(i => i.Id == id, cancellationToken);

    public async Task AddAsync(Item item, CancellationToken cancellationToken)
    {
        await _context.Items.AddAsync(item, cancellationToken);

        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateAsync(Item item, CancellationToken cancellationToken)
    {
        _context.Items.Update(item);

        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task RemoveAsync(Item item, CancellationToken cancellationToken)
    {
        _context.Items.Remove(item);

        await _context.SaveChangesAsync(cancellationToken);
    }
}
