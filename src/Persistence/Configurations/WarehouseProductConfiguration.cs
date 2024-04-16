using Domain.Entities.WarehouseProducts;
using Domain.Entities.Warehouses;
using Domain.Shared.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Configurations;

public sealed class WarehouseProductConfiguration : IEntityTypeConfiguration<WarehouseProduct>
{
    public void Configure(EntityTypeBuilder<WarehouseProduct> builder)
    {
        builder.HasKey(wp => wp.Id);

        builder.Property(wp => wp.Id)
            .HasConversion(
                wpId => wpId.Value,
                value => new WarehouseProductId(value));

        builder.OwnsOne(wp => wp.Quantity, quantity =>
        {
            quantity.Property(quantity => quantity.Value).HasColumnName("Quantity");
        });

        builder.HasOne(wp => wp.Product)
            .WithMany(p => p.WarehouseProducts)
            .HasForeignKey(wp => wp.ProductId);

        builder.HasOne(wp => wp.Warehouse)
            .WithMany(w => w.Products)
            .HasForeignKey(wp => wp.WarehouseId);
    }
}