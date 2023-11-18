namespace Application.Features.Products.Commands.Update;

public record UpdateProductRequest(
    string Name,
    string Description,
    string BrandCompany,
    string BrandCountry,
    double Price,
    double Width,
    double Height,
    double Length,
    List<string> Categories);