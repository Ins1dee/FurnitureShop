using Ardalis.GuardClauses;
using Domain.Entities.OrderDetails;
using Domain.Errors;

namespace Domain.Shared.ValueObjects;

public sealed record Amount
{
    private Amount(double value)
    {
        Value = value;
    }

    public double Value { get; init; }

    public static Amount FromOrderDetails(List<OrderDetail> orderDetails)
    {
        var total = orderDetails.Sum(od => od.Product.Price.Value * od.Quantity.Value);

        return new Amount(total);
    }

    public static Amount Create(double totalAmount)
    {
        return new Amount(totalAmount);
    }
}