namespace Domain.Entities.Orders.ValueObjects;

public sealed record OrderStatus
{
    private OrderStatus(string value)
    {
        Value = value;
    }
    
    public static OrderStatus InProcess => new(nameof(InProcess));

    public static OrderStatus Shipped => new(nameof(Shipped));

    public static OrderStatus Delivered => new(nameof(Delivered));

    public static OrderStatus Completed => new(nameof(Completed)); 

    public string Value { get; }
}