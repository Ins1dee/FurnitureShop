using Domain.Entities.Deliveries;
using Domain.Entities.Orders;
using Domain.Shared.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Configurations;

public sealed class DeliveryConfiguration : IEntityTypeConfiguration<Delivery>
{
    public void Configure(EntityTypeBuilder<Delivery> builder)
    {
        builder.HasKey(delivery => delivery.Id);

        builder.Property(delivery => delivery.Id)
            .HasConversion(
                deliveryId => deliveryId.Value,
                value => new DeliveryId(value));

        builder.Property(delivery => delivery.OrderId)
            .HasConversion(
                orderId => orderId.Value,
                value => new OrderId(value));

        builder.Property(delivery => delivery.Address)
            .HasConversion(
                address => address.Value,
                value => Location.Create(value));

        builder.Property(delivery => delivery.ArrivesAtc)
            .IsRequired();

        builder.Property(delivery => delivery.CreatedAtUtc)
            .IsRequired();

        builder.Property(delivery => delivery.Delivered)
            .IsRequired();

        builder.HasOne(delivery => delivery.Order)
            .WithOne(order => order.Delivery)
            .HasForeignKey<Delivery>(delivery => delivery.OrderId);

        builder.HasOne(delivery => delivery.User)
            .WithMany(user => user.Deliveries)
            .HasForeignKey(delivery => delivery.UserId);
    }
}