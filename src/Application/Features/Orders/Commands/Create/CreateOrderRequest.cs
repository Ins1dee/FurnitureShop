namespace Application.Features.Orders.Commands.Create;

public record CreateOrderRequest(
    Dictionary<Guid, int> Products,
    string CustomerName,
    string CustomerPhone,
    string CustomerAddress);