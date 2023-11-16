namespace FurnitureShop.Application.Features.Products.Commands.Create;

public record ProductResponse(
    string Name, 
    string Description,
    string Brand,
    double Price,
    double Width,
    double Height,
    double Length,
    List<string> Categories);