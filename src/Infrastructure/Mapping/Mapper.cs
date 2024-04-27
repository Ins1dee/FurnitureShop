using Application.Abstractions;
using Application.Features.Categories;
using Application.Features.Deliveries;
using Application.Features.Orders;
using Application.Features.Orders.Queries;
using Application.Features.Products;
using Domain.Entities.Categories;
using Domain.Entities.Deliveries;
using Domain.Entities.Incomes;
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

    private partial List<OrderResponse> MapOrderResponses(List<Order> orders);
    
    private partial List<OrderDetailResponse> MapOrderDetailRepsonses(List<OrderDetail> orders);

    private partial List<GetAllOrdersResponse> MapGetAllOrdersRepsonses(List<Order> orders);

    private partial List<IncomeResponse> MapIncomeResponses(List<Income> incomes);

    private partial List<DeliveryResponse> MapDeliveryResponses(List<Delivery> deliveries);
    
    [MapProperty("Id.Value", "Id")]
    [MapProperty("Name.Value", "Name")]
    [MapProperty("Description.Value", "Description")]
    [MapProperty("Brand.Company", "BrandCompany")]
    [MapProperty("Brand.Country", "BrandCountry")]
    [MapProperty("Price.Value", "Price")]
    [MapProperty("Dimensions.Width","Width")]
    [MapProperty("Dimensions.Height","Height")]
    [MapProperty("Dimensions.Length", "Length")]
    private partial ProductResponse MapProductResponse(Product product);

    [MapProperty("Id.Value", "Id")]
    [MapProperty("Name.Value", "Name")]
    private partial CategoryResponse MapCategoryResponse(Category category);

    [MapProperty("User.FullName", "SellerName")]
    [MapProperty("CustomerDetails.Name", "CustomerName")]
    [MapProperty("CustomerDetails.PhoneNumber", "CustomerPhone")]
    [MapProperty("TotalAmount.Value", "TotalAmount")]
    [MapProperty("PaymentStatus.Value", "PaymentStatus")]
    private partial OrderResponse MapOrderResponse(Order order);

    [MapProperty("Id.Value", "Id")]
    [MapProperty("User.FullName", "SellerName")]
    [MapProperty("CustomerDetails.Name", "CustomerName")]
    [MapProperty("CustomerDetails.PhoneNumber", "CustomerPhone")]
    [MapProperty("TotalAmount.Value", "TotalAmount")]
    [MapProperty("PaymentStatus.Value", "PaymentStatus")]
    private partial GetAllOrdersResponse MapGetAllOrdersResponse(Order order);


    [MapProperty("Product.Name.Value", "Product")]
    [MapProperty("Product.Price.Value", "Price")]
    [MapProperty("Quantity.Value", "Quantity")]
    private partial OrderDetailResponse MapOrderDetailResponse(OrderDetail order);

    [MapProperty("Amount.Value", "Amount")]
    [MapProperty("Id.Value", "Id")]
    private partial IncomeResponse MapIncomeResponses(Income income);

    [MapProperty("Id.Value", "Id")]
    [MapProperty("Address.Value", "Address")]
    [MapProperty("User.FullName", "UserName")]
    private partial DeliveryResponse MapDeliveryResponse(Delivery delivery);
}