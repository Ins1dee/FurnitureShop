using FurnitureShop.Domain.Abstractions;
using FurnitureShop.Domain.Entities.Categories;
using FurnitureShop.Domain.Entities.Products.ValueObjects;
using FurnitureShop.Domain.Shared.ValueObjects;

namespace FurnitureShop.Domain.Entities.Products;

public sealed class Product : Entity<ProductId>, IAggregateRoot
{
    private readonly List<Category> _categories = new();
    
    private Product(
        ProductId id,
        Name name, 
        Description description, 
        Brand brand, 
        Price price, 
        Dimentions dimentions,
        IEnumerable<Category> categories) 
        : base(id)
    {
        Name = name;
        Description = description;
        Brand = brand;
        Price = price;
        Dimentions = dimentions;
        _categories.AddRange(categories);
    }

    private Product()
    {
        // For EF Core
    }

    public Name Name { get; private set; }

    public Description Description { get; private set; }

    public Brand Brand { get; private set; }
    
    public Price Price { get; private set; }
    
    public Dimentions Dimentions { get; private set; }
    
    public IReadOnlyList<Category> Categories => _categories;

    public static Product Create(
        ProductId id, 
        Name name, 
        Description description, 
        Brand brand, 
        Price price,
        Dimentions dimentions,
        IEnumerable<Category> categories)
    {
        return new Product(id, name, description, brand, price, dimentions, categories);
    }

    public void Update(
        Name name, 
        Description description, 
        Brand brand, 
        Price price, 
        Dimentions dimentions, 
        IEnumerable<Category> categories)
    {
        Name = name;
        Description = description;
        Brand = brand;
        Price = price;
        Dimentions = dimentions;
        _categories.Clear();
        _categories.AddRange(categories);
    }
}