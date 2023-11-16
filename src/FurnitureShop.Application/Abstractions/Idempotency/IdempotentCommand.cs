using FurnitureShop.Application.Abstractions.Messaging;

namespace FurnitureShop.Application.Abstractions.Idempotency;

public record IdempotentCommand(Guid RequestId) : ICommand, IIdempotentCommandBase;

public record IdempotentCommand<TResponse>(Guid RequestId) : ICommand<TResponse>, IIdempotentCommandBase;

public interface IIdempotentCommandBase
{
    Guid RequestId { get; init; }
}