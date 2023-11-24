namespace Application.Features.Orders;

public record OrderResponse(
    string CustomerName,
    string CustomerPhone,
    string CustomerAddress,
    double TotalAmount,
    string Status,
    DateTime CreatedAtUtc,
    List<OrderDetailResponse> OrderDetails);