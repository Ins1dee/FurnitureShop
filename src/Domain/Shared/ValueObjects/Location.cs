using Ardalis.GuardClauses;
using Domain.Errors;

namespace Domain.Shared.ValueObjects;

public sealed record Location
{
    public const int MaxLength = 100;

    private Location(string value)
    {
        Value = value;
    }

    public string Value { get; init; }

    public static Location Create(string value)
    {
        Guard.Against.NullOrWhiteSpace(value);

        Guard.Against.OutOfRange(
            value.Length,
            nameof(value),
            0, MaxLength,
            DomainErrors.Common.TooLongLength().Message);

        return new Location(value);
    }
}