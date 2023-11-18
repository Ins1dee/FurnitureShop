using Domain.Abstractions;
using Domain.Entities.Products;
using Domain.Shared.ValueObjects;

namespace Domain.Entities.Categories;

public sealed class Category : Entity<CategoryId>, IAggregateRoot
{
    private readonly List<Product> _products = new();

    private Category(
        CategoryId id,
        Name name) 
        : base(id)
    {
        Name = name;
    }

    private Category()
    {
        // For EF Core
    }
    
    public Name Name { get; private set; }

    public IReadOnlyList<Product> Products => _products;
}