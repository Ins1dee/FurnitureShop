using Ardalis.GuardClauses;
using Domain.Errors;

namespace Domain.Entities.Suppliers.ValueObjects;

public sealed record ContactDetails
{
    public const int NameMaxLength = 50;

    public const int EmailMaxLength = 50;

    public const int PhoneMaxLength = 50;

    private ContactDetails(string name, string phoneNumber, string email)
    {
        Name = name;
        PhoneNumber = phoneNumber;
        Email = email;
    }

    public string Name { get; init; }

    public string PhoneNumber { get; init; }

    public string Email { get; init; }

    public static ContactDetails Create(string name, string phoneNumber, string email)
    {
        Guard.Against.NullOrWhiteSpace(name);
        Guard.Against.NullOrWhiteSpace(phoneNumber);
        Guard.Against.NullOrWhiteSpace(email);

        Guard.Against.OutOfRange(
            name.Length,
            nameof(name),
            0, NameMaxLength,
            DomainErrors.Common.TooLongLength().Message);

        Guard.Against.OutOfRange(
            phoneNumber.Length,
            nameof(phoneNumber),
            0, PhoneMaxLength,
            DomainErrors.Common.TooLongLength().Message);

        Guard.Against.OutOfRange(
            email.Length,
            nameof(phoneNumber),
            0, EmailMaxLength,
            DomainErrors.Common.TooLongLength().Message);

        return new ContactDetails(name, phoneNumber, email);
    }
}