namespace Application.Features.Deliveries;

public record DeliveryResponse(
    Guid Id,
    string Address,
    DateTime CreatedAtUtc,
    DateTime ArrivesAtUtc,
    bool Delivered,
    string? UserName);