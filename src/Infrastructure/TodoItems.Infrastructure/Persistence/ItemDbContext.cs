using Microsoft.EntityFrameworkCore;
using TodoItems.Application.Events;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TodoItems.Domain.Entities;

namespace TodoItems.Infrastructure.Persistence;

public class ItemDbContext(
    DbContextOptions<ItemDbContext> options,
    IDomainEventDispatcher dispatcher) : DbContext(options)
{
    public DbSet<Item> Items { get; set; } = null!;

    private readonly IDomainEventDispatcher _dispatcher = dispatcher;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Item>(entity =>
        {
            entity.HasKey(e => e.Id);

            entity.Ignore(e => e.DomainEvents);

            // Configurar el owned type Progression explícitamente para evitar
            // problemas de constructor binding de EF Core.
            entity.OwnsMany(e => e.Progressions, pb =>
            {
                pb.WithOwner().HasForeignKey("ItemId");
                pb.Property<DateTime>("Date");
                pb.Property<decimal>("Percent");
                pb.HasKey("ItemId", "Date");

                // Opcional: nombre de la tabla para las progressions (si no quieres tabla por defecto)
                // pb.ToTable("ItemProgressions");
            });
        });
    }

    public override async Task<int> SaveChangesAsync(
        CancellationToken cancellationToken = default)
    {
        var domainEvents = ChangeTracker
            .Entries<Item>()
            .SelectMany(e => e.Entity.DomainEvents)
            .ToList();

        var result = await base.SaveChangesAsync(cancellationToken);

        await _dispatcher.DispatchAsync(domainEvents);

        foreach (var entry in ChangeTracker.Entries<Item>())
        {
            entry.Entity.ClearDomainEvents();
        }

        return result;
    }
}
