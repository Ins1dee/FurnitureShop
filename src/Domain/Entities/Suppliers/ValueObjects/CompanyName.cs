using Ardalis.GuardClauses;
using Domain.Errors;

namespace Domain.Entities.Suppliers.ValueObjects;

public sealed record CompanyName
{
    public const int MaxLength = 50;

    private CompanyName(string value)
    {
        Value = value;
    }

    public string Value { get; init; }

    public static CompanyName Create(string value)
    {
        Guard.Against.NullOrWhiteSpace(value);

        Guard.Against.OutOfRange(
            value.Length,
            nameof(value),
            0, MaxLength,
            DomainErrors.Common.TooLongLength().Message);

        return new CompanyName(value);
    }
}