namespace Domain.Entities.Orders.ValueObjects;

public sealed record OrderPaymentStatus
{
    public OrderPaymentStatus(string value)
    {
        Value = value;
    }
    
    public static OrderPaymentStatus InProcess => new(nameof(InProcess));

    public static OrderPaymentStatus Finished => new(nameof(Finished));

    public string Value { get; }
}