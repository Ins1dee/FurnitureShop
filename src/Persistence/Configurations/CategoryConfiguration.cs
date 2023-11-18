using Domain.Entities.Categories;
using Domain.Shared.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Configurations;

public sealed class CategoryConfiguration : IEntityTypeConfiguration<Category>
{
    public void Configure(EntityTypeBuilder<Category> builder)
    {
        builder.HasKey(category => category.Id);
        
        builder.Property(category => category.Id)
            .HasConversion(
                categoryId => categoryId.Value,
                value => new CategoryId(value));
        
        builder.Property(category => category.Name)
            .HasConversion(
                name => name.Value,
                value => Name.Create(value));

        builder.HasMany(category => category.Products)
            .WithMany(product => product.Categories)
            .UsingEntity(j => j.ToTable("ProductCategories"));
    }
}