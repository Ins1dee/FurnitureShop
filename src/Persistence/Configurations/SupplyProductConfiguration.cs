using Domain.Entities.Expenses;
using Domain.Entities.Suppliers;
using Domain.Entities.SupplyProducts;
using Domain.Shared.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Configurations;

public sealed class SupplyProductConfiguration : IEntityTypeConfiguration<SupplyProduct>
{
    public void Configure(EntityTypeBuilder<SupplyProduct> builder)
    {
        builder.HasKey(sp => sp.Id);

        builder.Property(sp => sp.Id)
            .HasConversion(
                supplierProductId => supplierProductId.Value,
                value => new SupplyProductId(value));

        builder.Property(sp => sp.CreatedAtUtc)
            .IsRequired();

        builder.OwnsOne(sp => sp.Quantity, quantity =>
        {
            quantity.Property(quantity => quantity.Value).HasColumnName("Quantity");
        });

        builder.HasOne(sp => sp.Product)
            .WithMany(p => p.SupplyProducts)
            .HasForeignKey(sp => sp.ProductId);

        builder.HasOne(sp => sp.Supplier)
            .WithMany(s => s.Products)
            .HasForeignKey(sp => sp.SupplierId);

        builder.OwnsMany(sp => sp.Expenses, expenses =>
        {
            expenses
                .WithOwner(e => e.SupplyProduct)
                .HasForeignKey(e => e.SupplyProductId);

            expenses.ToTable("Expenses");

            expenses.HasKey(e => e.Id);

            expenses.Property(e => e.Id)
                .HasConversion(
                    expenseId => expenseId.Value,
                    value => new ExpenseId(value));

            expenses.Property(e => e.Amount)
                .HasConversion(
                    amount => amount.Value,
                    value => Amount.Create(value))
                .HasColumnName("Amount");

            expenses.Property(e => e.CreatedAtUtc)
                .IsRequired();
        });
    }
}