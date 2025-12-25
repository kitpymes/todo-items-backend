using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using TodoItems.Domain.Entities;
using TodoItems.Infrastructure.Extensions;

namespace TodoItems.Infrastructure.Persistence;

public class ItemDbContext(DbContextOptions<ItemDbContext> options, IServiceProvider serviceProvider) : DbContext(options)
{
    private readonly IMediator _mediator = serviceProvider.GetRequiredService<IMediator>();

    public DbSet<Item> Items { get; set; } = null!;
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ItemDbContext).Assembly);
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        var result = await base.SaveChangesAsync(cancellationToken);

        await _mediator.DispatchDomainEventsAsync(this);

        return result;
    }
}
