using Application.Features.Categories;

namespace Application.Features.Products;

public sealed record ProductResponse(
    Guid Id,
    string Name,
    string Description,
    string BrandCompany,
    string BrandCountry,
    double Price,
    double Width,
    double Height,
    double Length,
    List<CategoryResponse> Categories);