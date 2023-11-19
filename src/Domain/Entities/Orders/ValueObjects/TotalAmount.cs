using Domain.Entities.OrderDetails;

namespace Domain.Entities.Orders.ValueObjects;

public sealed record TotalAmount
{
    private TotalAmount(double value)
    {
        Value = value;
    }

    public double Value { get; init; }

    public static TotalAmount Create(List<OrderDetail> orderDetails)
    { 
        var total = orderDetails.Sum(od => od.Product.Price.Value);

        return new TotalAmount(total);
    }
}