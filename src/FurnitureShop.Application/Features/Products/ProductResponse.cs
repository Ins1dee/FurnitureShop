using FurnitureShop.Application.Features.Categories;

namespace FurnitureShop.Application.Features.Products;

public sealed record ProductResponse(
    string Name,
    string Description,
    string BrandCompany,
    string BrandCountry,
    double Price,
    double Width,
    double Height,
    double Length,
    List<CategoryResponse> Categories);