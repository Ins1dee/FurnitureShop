using Domain.Entities.OrderDetails;
using Domain.Entities.OrderDetails.ValueObjects;
using Domain.Entities.Orders;
using Domain.Entities.Orders.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Configurations;

public sealed class OrderConfiguration : IEntityTypeConfiguration<Order>
{
    public void Configure(EntityTypeBuilder<Order> builder)
    {
        builder.HasKey(order => order.Id);
        
        builder.Property(order => order.Id)
            .HasConversion(
                orderId => orderId.Value,
                value => new OrderId(value));
        
        builder.OwnsOne(order => order.CustomerDetails, customerDetails =>
        {
            customerDetails.Property(customerDetails => customerDetails.Name)
                .HasColumnName("CustomerName");
            
            customerDetails.Property(customerDetails => customerDetails.PhoneNumber)
                .HasColumnName("CustomerPhoneNumber");
            
            customerDetails.Property(customerDetails => customerDetails.Address)
                .HasColumnName("CustomerAddress");
        });

        builder.Property(order => order.TotalAmount)
            .HasConversion(
                totalAmount => totalAmount.Value,
                value => TotalAmount.Create(value));
        
        builder.OwnsOne(order => order.Status, status =>
        {
            status.Property(status => status.Value).HasColumnName("Status");
        });
        
        builder.Property(order => order.CreatedAtUtc)
            .IsRequired();
        
        builder.OwnsMany(order => order.OrderDetails, orderDetails =>
        {
            orderDetails
                .WithOwner()
                .HasForeignKey(od => od.OrderId);
            
            orderDetails.ToTable("OrderDetails");
            
            orderDetails.HasKey(od => od.Id);

            orderDetails.Property(od => od.Id)
                .HasConversion(
                    orderDetailId => orderDetailId.Value,
                    value => new OrderDetailId(value));
                
            
            orderDetails.Property(od => od.Quantity)
                .HasConversion(
                    quantity => quantity.Value,
                    value => Quantity.Create(value))
                .HasColumnName("Quantity");
            
            orderDetails.HasOne(od => od.Product)
                .WithMany()
                .HasForeignKey(od => od.ProductId);
        });
        
        builder.HasOne(order => order.User)
            .WithMany(user => user.Orders)
            .HasForeignKey(order => order.UserId);
    }
}