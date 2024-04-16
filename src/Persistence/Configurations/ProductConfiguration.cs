using Domain.Entities.Products;
using Domain.Entities.Products.ValueObjects;
using Domain.Shared.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Configurations;

public sealed class ProductConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.HasKey(product => product.Id);
        
        builder.Property(product => product.Id)
            .HasConversion(
                productId => productId.Value,
                value => new ProductId(value));
        
        builder.Property(product => product.Name)
            .HasConversion(
                name => name.Value,
                value => Name.Create(value));
        
        builder.Property(product => product.Description)
            .HasConversion(
                description => description.Value,
                value => Description.Create(value));
        
        builder.OwnsOne(product => product.Brand, brand =>
        {
            brand.Property(brand => brand.Company)
                .HasColumnName("Company");
            
            brand.Property(brand => brand.Country)
                .HasColumnName("Country");
        });
        
        builder.Property(product => product.Price)
            .HasConversion(
                price => price.Value,
                value => Price.Create(value));
        
        builder.OwnsOne(product => product.Dimensions, dimentions =>
        {
            dimentions.Property(dimentions => dimentions.Height)
                .HasColumnName("Height");
            
            dimentions.Property(dimentions => dimentions.Width)
                .HasColumnName("Width");

            dimentions.Property(dimentions => dimentions.Length)
                .HasColumnName("Length");
        });

        builder.HasMany(product => product.Categories)
            .WithMany(category => category.Products)
            .UsingEntity(j => j.ToTable("ProductCategories"));

        builder.HasMany(product => product.WarehouseProducts)
            .WithOne(wp => wp.Product)
            .HasForeignKey(wp => wp.ProductId);

        builder.HasMany(product => product.SupplyProducts)
            .WithOne(sp => sp.Product)
            .HasForeignKey(sp => sp.ProductId);
    }
}