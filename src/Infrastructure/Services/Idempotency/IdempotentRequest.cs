namespace Infrastructure.Services.Idempotency;

public sealed record IdempotentRequest(
    Guid Id,
    string Name,
    DateTime CreatedOnUtc);