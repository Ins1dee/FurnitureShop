using Bogus;
using Domain.Entities.Deliveries;
using Domain.Entities.Orders;
using Domain.Shared.ValueObjects;

namespace Persistence.Bogus.Fakers;

public sealed class DeliveryFaker: Faker<Delivery>
{
    public DeliveryFaker(List<Order> orders)
    {
        RuleFor(d => d.Id, f => new DeliveryId(f.Random.Guid()));
        RuleFor(d => d.Delivered, true);
        RuleFor(d => d.Address, f => Location.Create(f.Address.FullAddress()));
    }
}