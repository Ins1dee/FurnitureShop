using Application.Abstractions;
using Application.Features.Categories;
using Application.Features.Orders;
using Application.Features.Orders.Queries;
using Application.Features.Products;
using Domain.Entities.Categories;
using Domain.Entities.OrderDetails;
using Domain.Entities.Orders;
using Domain.Entities.Products;
using Riok.Mapperly.Abstractions;

namespace Infrastructure.Mapping;

[Mapper]
public partial class Mapper : IMapper
{
    public partial TTarget Map<TTarget>(object source);
    
    private partial List<ProductResponse> MapProductResponses(List<Product> products);

    private partial List<CategoryResponse> MapCategoryResponses(List<Category> categories);

    private partial List<OrderResponse> MapOrderRepsonses(List<Order> orders);
    
    private partial List<OrderDetailResponse> MapOrderDetailRepsonses(List<OrderDetail> orders);
    
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

    [MapProperty("CustomerDetails.Name", "CustomerName")]
    [MapProperty("CustomerDetails.PhoneNumber", "CustomerPhone")]
    [MapProperty("CustomerDetails.Address", "CustomerAddress")]
    [MapProperty("TotalAmount.Value", "TotalAmount")]
    [MapProperty("Status.Value", "Status")]
    private partial OrderResponse MapOrderResponse(Order order);
    
    [MapProperty("Product.Name.Value", "Product")]
    [MapProperty("Product.Price.Value", "Price")]
    [MapProperty("Quantity.Value", "Quantity")]
    private partial OrderDetailResponse MapOrderDetailResponse(OrderDetail order);
}