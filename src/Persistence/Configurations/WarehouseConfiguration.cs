using Domain.Entities.SupplyProducts;
using Domain.Entities.Warehouses;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Configurations;

public sealed class WarehouseConfiguration : IEntityTypeConfiguration<Warehouse>
{
    public void Configure(EntityTypeBuilder<Warehouse> builder)
    {
        builder.HasKey(warehouse => warehouse.Id);

        builder.Property(warehouse => warehouse.Id)
            .HasConversion(
                warehouseId => warehouseId.Value,
                value => new WarehouseId(value));

        builder.OwnsOne(warehouse => warehouse.Location, location =>
        {
            location.Property(location => location.Value).HasColumnName("Location");
        });

        builder.OwnsOne(warehouse => warehouse.QuantityLimit, limit =>
        {
            limit.Property(limit => limit.Value).HasColumnName("QuantityLimit");
        });

        builder.HasMany(warehouse => warehouse.Products)
            .WithOne(p => p.Warehouse)
            .HasForeignKey(p => p.WarehouseId);
    }
}