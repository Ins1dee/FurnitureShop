namespace Application.Features.Orders;

public record OrderDetailResponse(
    string Product,
    double Price,
    int Quantity);