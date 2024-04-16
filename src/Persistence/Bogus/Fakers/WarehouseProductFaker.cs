using Bogus;
using Domain.Entities.Products;
using Domain.Entities.WarehouseProducts;
using Domain.Entities.Warehouses;
using Domain.Shared.ValueObjects;

namespace Persistence.Bogus.Fakers;

public sealed class WarehouseProductFaker : Faker<WarehouseProduct>
{
    public WarehouseProductFaker(List<Warehouse> warehouses)
    {
        RuleFor(wp => wp.Id, f => new WarehouseProductId(f.Random.Guid()));
        RuleFor(wp => wp.Quantity, f => Quantity.Create(f.Random.Int(10, 1000)));
        RuleFor(wp => wp.ProductId, new ProductId(Guid.NewGuid()));
        RuleFor(wp => wp.WarehouseId, f => f.PickRandom(warehouses).Id);
    }
}