using Domain.Abstractions;
using Domain.Entities.Orders;
using Domain.Shared.ValueObjects;
using Microsoft.VisualBasic;

namespace Domain.Entities.Incomes;

public class Income: Entity<IncomeId>
{
    public Income(
        IncomeId id, 
        OrderId orderId, 
        Amount amount, 
        DateTime createdAtUtc, 
        Order? order) 
        : base(id)
    {
        OrderId = orderId;
        Amount = amount;
        CreatedAtUtc = createdAtUtc;
        Order = order;
    }

    public Income()
    {
        // For EF
    }

    public OrderId OrderId { get; private set; }

    public Amount Amount { get; private set; }

    public DateTime CreatedAtUtc { get; private set; }

    public Order? Order { get; private set; }

    public void SetAmount(double amount)
    {
        Amount = Amount.Create(amount);
    }

    public static Income Create(OrderId id, Amount amount, Order? order = default)
    {
        return new Income(new IncomeId(Guid.NewGuid()), id, amount, DateTime.UtcNow, order);
    }
}