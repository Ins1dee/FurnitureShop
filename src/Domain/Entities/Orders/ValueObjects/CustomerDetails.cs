using Ardalis.GuardClauses;
using Domain.Errors;

namespace Domain.Entities.Orders.ValueObjects;

public sealed record CustomerDetails
{
    public const int NameMaxLength = 50;

    public const int PhoneMaxLength = 50;
    
    private CustomerDetails(string name, string phoneNumber)
    {
        Name = name;
        PhoneNumber = phoneNumber;
    }

    public string Name { get; init; }
    
    public string PhoneNumber { get; init; }

    public static CustomerDetails Create(string name, string phoneNumber)
    {
        Guard.Against.NullOrWhiteSpace(name);
        Guard.Against.NullOrWhiteSpace(phoneNumber);
        
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
        
        
        return new CustomerDetails(name, phoneNumber);
    }
}