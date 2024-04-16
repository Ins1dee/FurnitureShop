using Bogus;
using Domain.Entities.Orders;
using Domain.Entities.Orders.ValueObjects;
using Domain.Entities.Users;
using Domain.Shared.ValueObjects;

namespace Persistence.Bogus.Fakers;

public sealed class OrderFaker: Faker<Order>
{
    private readonly List<OrderPaymentStatus> _paymentStatuses = new() 
        { OrderPaymentStatus.Finished, OrderPaymentStatus.InProcess };
    public OrderFaker(List<User> users)
    {
        RuleFor(o => o.Id, f => new OrderId(f.Random.Guid()));
        RuleFor(u => u.CreatedAtUtc, f => f.Date.Between(new DateTime(2020, 1, 1), new DateTime(2023, 12, 30)));
        RuleFor(o => o.CustomerDetails, f => CustomerDetails.Create(f.Person.FullName, f.Person.Phone));
        RuleFor(o => o.TotalAmount, Amount.Create(1));
        RuleFor(o => o.PaymentStatus, f => f.PickRandom(_paymentStatuses));
        RuleFor(o => o.UserId, f => f.PickRandom(users).Id);
    }
}