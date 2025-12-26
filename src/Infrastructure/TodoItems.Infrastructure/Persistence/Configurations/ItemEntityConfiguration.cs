using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TodoItems.Domain.Aggregates.TodoListAggregate;
using TodoItems.Domain.Aggregates.TodoListAggregate.ValeObjects;

namespace TodoItems.Infrastructure.Persistence.Configurations;

public sealed class ItemEntityConfiguration : IEntityTypeConfiguration<TodoItem>
{
    public void Configure(EntityTypeBuilder<TodoItem> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id).ValueGeneratedNever();

        builder.Property(x => x.Title).IsRequired();

        builder.HasOne<TodoList>().WithMany(x => x.Items).HasForeignKey("TodoListId");

        builder.Property(x => x.Category)
           .HasConversion(
               v => v.Name,          // Al guardar en DB
               v => new Category(v)  // Al leer de DB
           )
           .HasColumnName("CategoryName")
           .IsRequired();

        builder.OwnsMany(e => e.Progressions, pb =>
        {
            pb.WithOwner().HasForeignKey("ItemId");
            pb.Property(p => p.Date).IsRequired();
            pb.Property(p => p.Percent).IsRequired();

            pb.HasKey("ItemId", nameof(Progression.Date));
        });
    }
}
