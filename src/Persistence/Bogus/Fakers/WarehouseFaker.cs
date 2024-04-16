using Bogus;
using Domain.Entities.Warehouses;
using Domain.Entities.Warehouses.ValueObjects;
using Domain.Shared.ValueObjects;

namespace Persistence.Bogus.Fakers;

public sealed class WarehouseFaker : Faker<Warehouse>
{
    public WarehouseFaker()
    {
        RuleFor(w => w.Id, f => new WarehouseId(f.Random.Guid()));
        RuleFor(w => w.Location, f => Location.Create(f.Address.FullAddress()));
        RuleFor(w => w.QuantityLimit, f => QuantityLimit.Create(f.Random.Int(100, 1000)));
    }
}