using Bogus;
using Domain.Entities.Products;
using Domain.Entities.Suppliers;
using Domain.Entities.SupplyProducts;
using Domain.Shared.ValueObjects;

namespace Persistence.Bogus.Fakers;

public sealed class SupplyProductFaker : Faker<SupplyProduct>
{
    public SupplyProductFaker(List<Supplier> suppliers, List<Product> products)
    {
        RuleFor(sp => sp.Id, f => new SupplyProductId(f.Random.Guid()));
        RuleFor(sp => sp.Quantity, f => Quantity.Create(f.Random.Int(10, 100)));
        RuleFor(sp => sp.CreatedAtUtc, f => f.Date.Between(new DateTime(2020, 1, 1), new DateTime(2023, 12, 30)));
        RuleFor(sp => sp.SupplierId, f => f.PickRandom(suppliers).Id);
        RuleFor(sp => sp.ProductId, f => f.PickRandom(products).Id);
    }
}