using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TodoItems.Domain.Entities;

namespace TodoItems.Infrastructure.Persistence.Configurations;

public sealed class ItemEntityConfiguration : IEntityTypeConfiguration<Item>
{
    public void Configure(EntityTypeBuilder<Item> builder)
    {
        builder.Ignore(e => e.DomainEvents);

        builder.HasKey(e => e.Id);

        builder.OwnsMany(e => e.Progressions, pb =>
        {
            pb.WithOwner().HasForeignKey("ItemId");
            pb.Property<DateTime>("Date");
            pb.Property<decimal>("Percent");
            pb.HasKey("ItemId", "Date");
        });
    }
}
