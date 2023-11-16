using Ardalis.GuardClauses;
using FurnitureShop.Domain.Errors;

namespace FurnitureShop.Domain.Entities.Products.ValueObjects;

public record Brand
{
    public const int NameMaxLength = 50;
    
    private Brand(string company, string country)
    {
        Company = company;
        Country = country;
    }

    public string Company { get; init; }
    
    public string Country { get; init; }

    public static Brand Create(string company, string country)
    {
        Guard.Against.NullOrWhiteSpace(company);
        Guard.Against.NullOrWhiteSpace(country);
        
        Guard.Against.OutOfRange(
            company.Length,
            nameof(company),
            0, NameMaxLength,
            DomainErrors.Common.TooLongLength().Message);

        Guard.Against.OutOfRange(
            country.Length,
            nameof(country),
            0, NameMaxLength,
            DomainErrors.Common.TooLongLength().Message);
        
        return new Brand(company, country);
    }
}