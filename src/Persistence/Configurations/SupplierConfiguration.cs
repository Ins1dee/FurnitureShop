using Domain.Entities.Deliveries;
using Domain.Entities.Suppliers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Configurations;

public sealed class SupplierConfiguration : IEntityTypeConfiguration<Supplier>
{
    public void Configure(EntityTypeBuilder<Supplier> builder)
    {
        builder.HasKey(supplier => supplier.Id);

        builder.Property(supplier => supplier.Id)
            .HasConversion(
                supplierId => supplierId.Value,
                value => new SupplierId(value));

        builder.OwnsOne(supplier => supplier.CompanyName, companyName =>
        {
            companyName.Property(name => name.Value).HasColumnName("CompanyName");
        });

        builder.OwnsOne(supplier => supplier.ContactDetails, contactDetails =>
        {
            contactDetails.Property(contactDetails => contactDetails.Name)
                .HasColumnName("ContactName");

            contactDetails.Property(contactDetails => contactDetails.PhoneNumber)
                .HasColumnName("ContactPhoneNumber");
        });

        builder.HasMany(supplier => supplier.Products)
            .WithOne(p => p.Supplier)
            .HasForeignKey(sp => sp.SupplierId);
    }
}