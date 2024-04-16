namespace Application.Features.Orders;

public record OrderResponse(
    string CustomerName,
    string CustomerPhone,
    double TotalAmount,
    string Status,
    DateTime CreatedAtUtc,
    List<OrderDetailResponse> OrderDetails);