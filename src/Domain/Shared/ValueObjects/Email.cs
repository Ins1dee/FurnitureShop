using System.ComponentModel.DataAnnotations;
using Ardalis.GuardClauses;
using Domain.Errors;

namespace Domain.Shared.ValueObjects;

public sealed record Email
{
    private Email(string value)
    {
        Value = value;
    }
    
    public string Value { get; init; }

    public static Email Create(string value)
    {
        Guard.Against.NullOrWhiteSpace(value);
        Guard.Against.InvalidEmail(value, DomainErrors.Email.InvalidEmail().Message);

        return new Email(value);
    }
}