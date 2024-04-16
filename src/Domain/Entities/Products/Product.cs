using Domain.Abstractions;
using Domain.Entities.Categories;
using Domain.Entities.Products.ValueObjects;
using Domain.Entities.SupplyProducts;
using Domain.Entities.WarehouseProducts;
using Domain.Shared.ValueObjects;

namespace Domain.Entities.Products;

public sealed class Product : Entity<ProductId>, IAggregateRoot
{
    private readonly List<Category> _categories = new();

    private readonly List<WarehouseProduct> _warehouseProducts = new();

    private readonly List<SupplyProduct> _supplyProducts = new();
    
    private Product(
        ProductId id,
        Name name, 
        Description description, 
        Brand brand, 
        Price price, 
        Dimensions dimensions,
        IEnumerable<Category> categories) 
        : base(id)
    {
        Name = name;
        Description = description;
        Brand = brand;
        Price = price;
        Dimensions = dimensions;
        _categories.AddRange(categories);
    }

    public Product()
    {
        // For EF Core
    }

    public Name Name { get; private set; }

    public Description Description { get; private set; }

    public Brand Brand { get; private set; }
    
    public Price Price { get; private set; }
    
    public Dimensions Dimensions { get; private set; }
    
    public IReadOnlyList<Category> Categories => _categories;

    public IReadOnlyList<WarehouseProduct> WarehouseProducts => _warehouseProducts;

    public IReadOnlyList<SupplyProduct> SupplyProducts => _supplyProducts;

    public static Product Create(
        ProductId id, 
        Name name, 
        Description description, 
        Brand brand, 
        Price price,
        Dimensions dimensions,
        IEnumerable<Category> categories)
    {
        return new Product(id, name, description, brand, price, dimensions, categories);
    }

    public void Update(
        Name name, 
        Description description, 
        Brand brand, 
        Price price, 
        Dimensions dimensions, 
        IEnumerable<Category> categories)
    {
        Name = name;
        Description = description;
        Brand = brand;
        Price = price;
        Dimensions = dimensions;
        _categories.Clear();
        _categories.AddRange(categories);
    }
}