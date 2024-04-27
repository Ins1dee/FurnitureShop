namespace Application.Features.Orders;

public record OrderResponse(
    string SellerName,
    string CustomerName,
    string CustomerPhone,
    double TotalAmount,
    string PaymentStatus,
    DateTime CreatedAtUtc,
    List<OrderDetailResponse> OrderDetails,
    List<IncomeResponse> Incomes);