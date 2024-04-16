using Ardalis.GuardClauses;
using Domain.Errors;

namespace Domain.Entities.Warehouses.ValueObjects;

public sealed record QuantityLimit
{
    private QuantityLimit(int value)
    {
        Value = value;
    }

    public int Value { get; init; }

    public static QuantityLimit Create(int limit)
    {
        Guard.Against.NegativeOrZero(limit, DomainErrors.Order.InvalidTotalAmount().Message);

        return new QuantityLimit(limit);
    }
}