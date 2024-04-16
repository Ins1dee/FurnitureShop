using Ardalis.GuardClauses;
using Domain.Entities.Products.ValueObjects;
using Domain.Errors;

namespace Domain.Shared.ValueObjects;

public sealed record Quantity
{
    private Quantity(int value)
    {
        Value = value;
    }

    public int Value { get; init; }

    public static Quantity Create(int value)
    {
        Guard.Against.NegativeOrZero(value, DomainErrors.Order.InvalidQuantity().Message);

        return new Quantity(value);
    }
}