namespace Application.Features.Orders;

public record IncomeResponse(
    Guid Id,
    double Amount,
    DateTime CreatedAtUtc);