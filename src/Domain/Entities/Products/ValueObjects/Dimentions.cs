using Ardalis.GuardClauses;
using Domain.Errors;

namespace Domain.Entities.Products.ValueObjects;

public record Dimentions
{
    private Dimentions(double width, double height, double length)
    {
        Width = width;
        Height = height;
        Length = length;
    }

    public double Width { get; init; }
    
    public double Height { get; init; }
    
    public double Length { get; init; }

    public static Dimentions Create(double width, double height, double length)
    {
        Guard.Against.NegativeOrZero(width, DomainErrors.Product.InvalidDimentions().Message);
        Guard.Against.NegativeOrZero(height, DomainErrors.Product.InvalidPriceValue().Message);
        Guard.Against.NegativeOrZero(length, DomainErrors.Product.InvalidPriceValue().Message);
        
        return new Dimentions(width, height, length);
    }
}