using Domain.Entities.Incomes;
using Domain.Entities.OrderDetails;
using Domain.Entities.Orders;
using Domain.Entities.Orders.ValueObjects;
using Domain.Shared.ValueObjects;
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
        });

        builder.Property(order => order.TotalAmount)
            .HasConversion(
                totalAmount => totalAmount.Value,
                value => Amount.Create(value));

        builder.Property(order => order.PaymentStatus)
            .HasConversion(paymentStatus => paymentStatus.Value, value => new OrderPaymentStatus(value));
        
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

        builder.OwnsMany(order => order.Incomes, incomes =>
        {
            incomes
                .WithOwner(incomes => incomes.Order)
                .HasForeignKey(od => od.OrderId);

            incomes.ToTable("Incomes");

            incomes.HasKey(income => income.Id);

            incomes.Property(income => income.Id)
                .HasConversion(
                    incomeId => incomeId.Value,
                    value => new IncomeId(value));


            incomes.Property(income => income.Amount)
                .HasConversion(
                    amount => amount.Value,
                    value => Amount.Create(value))
                .HasColumnName("Amount");

            incomes.Property(income => income.CreatedAtUtc)
                .IsRequired();
        });

        builder.HasOne(order => order.User)
            .WithMany(user => user.Orders)
            .HasForeignKey(order => order.UserId);

        builder.HasOne(order => order.Delivery)
            .WithOne(delivery => delivery.Order)
            .HasForeignKey<Order>(order => order.DeliveryId);
    }
}