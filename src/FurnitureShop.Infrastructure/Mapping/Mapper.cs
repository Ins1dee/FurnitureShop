using FurnitureShop.Application.Abstractions;
using FurnitureShop.Application.Features.Categories;
using FurnitureShop.Application.Features.Products;
using FurnitureShop.Domain.Entities.Categories;
using FurnitureShop.Domain.Entities.Products;
using Riok.Mapperly.Abstractions;

namespace FurnitureShop.Infrastructure.Mapping;

[Mapper]
public sealed partial class Mapper : IMapper
{
    public partial TTarget Map<TTarget>(object source);
    
    private partial List<ProductResponse> MapProductResponses(List<Product> products);

    private partial List<CategoryResponse> MapCategoryResponses(List<Category> categories);
    
    [MapProperty("Name.Value", "Name")]
    [MapProperty("Description.Value", "Description")]
    [MapProperty("Brand.Company", "BrandCompany")]
    [MapProperty("Brand.Country", "BrandCountry")]
    [MapProperty("Price.Value", "Price")]
    [MapProperty("Dimentions.Width","Width")]
    [MapProperty("Dimentions.Height","Height")]
    [MapProperty("Dimentions.Length", "Length")]
    private partial ProductResponse MapProductResponse(Product product);

    [MapProperty("Name.Value", "Name")]
    private partial CategoryResponse MapCategoryResponse(Category category);
}