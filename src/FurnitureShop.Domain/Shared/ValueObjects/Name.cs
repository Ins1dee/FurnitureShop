using Ardalis.GuardClauses;
using FurnitureShop.Domain.Errors;

namespace FurnitureShop.Domain.Shared.ValueObjects;

public record Name
{
    public const int MaxLength = 50;
    
    private Name(string value)
    {
        Value = value;
    }

    public string Value { get; init; }

    public static Name Create(string value)
    {
        Guard.Against.NullOrWhiteSpace(value);
        
        Guard.Against.OutOfRange(
            value.Length,
            nameof(value),
            0, MaxLength,
            DomainErrors.Common.TooLongLength().Message);

        return new Name(value);
    }
}