using Ardalis.GuardClauses;
using Domain.Errors;

namespace Domain.Entities.Products.ValueObjects;

public record Description
{
    public const int MaxLength = 200;
    
    private Description(string value)
    {
        Value = value;
    }

    public string Value { get; init; }

    public static Description Create(string value)
    {
        Guard.Against.NullOrWhiteSpace(value);
        
        Guard.Against.OutOfRange(
            value.Length,
            nameof(value),
            0, MaxLength,
            DomainErrors.Common.TooLongLength().Message);

        return new Description(value);
    }
}