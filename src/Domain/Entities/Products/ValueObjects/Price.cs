using Ardalis.GuardClauses;
using Domain.Errors;

namespace Domain.Entities.Products.ValueObjects;

public record Price
{
    private Price(double value)
    {
        Value = value;
    }

    public double Value { get; init; }

    public static Price Create(double value)
    {
        Guard.Against.NegativeOrZero(value, DomainErrors.Product.InvalidPriceValue().Message);
        
        return new Price(value);
    }
}