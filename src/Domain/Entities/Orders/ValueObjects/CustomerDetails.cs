using Ardalis.GuardClauses;
using Domain.Errors;

namespace Domain.Entities.Orders.ValueObjects;

public sealed record CustomerDetails
{
    public const int NameMaxLength = 50;
    
    public const int AddressMaxLength = 50;

    public const int PhoneMaxLength = 20;
    
    private CustomerDetails(string name, string phoneNumber, string address)
    {
        Name = name;
        PhoneNumber = phoneNumber;
        Address = address;
    }

    public string Name { get; init; }
    
    public string PhoneNumber { get; init; }
    
    public string Address { get; init; }

    public static CustomerDetails Create(string name, string phoneNumber, string address)
    {
        Guard.Against.NullOrWhiteSpace(name);
        Guard.Against.NullOrWhiteSpace(phoneNumber);
        Guard.Against.NullOrWhiteSpace(address);
        
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
            address.Length,
            nameof(phoneNumber),
            0, AddressMaxLength,
            DomainErrors.Common.TooLongLength().Message);
        
        return new CustomerDetails(name, phoneNumber, address);
    }
}