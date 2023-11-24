using Ardalis.GuardClauses;
using Domain.Entities.OrderDetails;
using Domain.Errors;

namespace Domain.Entities.Orders.ValueObjects;

public sealed record TotalAmount
{
    private TotalAmount(double value)
    {
        Value = value;
    }

    public double Value { get; init; }

    public static TotalAmount FromOrderDetails(List<OrderDetail> orderDetails)
    { 
        var total = orderDetails.Sum(od => od.Product.Price.Value * od.Quantity.Value);

        return new TotalAmount(total);
    }
    
    public static TotalAmount Create(double totalAmount)
    {
        Guard.Against.NegativeOrZero(totalAmount, DomainErrors.Order.InvalidTotalAmount().Message);
        
        return new TotalAmount(totalAmount);
    }
}