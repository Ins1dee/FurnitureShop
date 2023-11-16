namespace FurnitureShop.WebAPI.Requests;

public record CreateProductRequest(
    string Name,
    string Description,
    string BrandCompany,
    string BrandCountry,
    double Price,
    double Width,
    double Height,
    double Length,
    List<string> Categories);