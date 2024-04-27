namespace Application.Features.Orders;

public record GetAllOrdersResponse(
    Guid Id,
    string SellerName,
    string CustomerName,
    string CustomerPhone,
    double TotalAmount,
    string PaymentStatus,
    DateTime CreatedAtUtc);