using Bogus;
using Domain.Entities.OrderDetails;
using Domain.Entities.Orders;
using Domain.Entities.Products;
using Domain.Shared.ValueObjects;

namespace Persistence.Bogus.Fakers;

public sealed class OrderDetailFaker: Faker<OrderDetail>
{
    public OrderDetailFaker(List<Order> orders, List<Product> products)
    {
        RuleFor(od => od.Id, f => new OrderDetailId(f.Random.Guid()));
        RuleFor(od => od.OrderId, f => f.PickRandom(orders).Id);
        RuleFor(od => od.ProductId, f => f.PickRandom(products).Id);
        RuleFor(od => od.Quantity, f => Quantity.Create(f.Random.Int(1, 20)));
    }
}