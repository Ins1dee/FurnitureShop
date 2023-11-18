using Application.Abstractions.Messaging;

namespace Application.Abstractions.Idempotency;

public record IdempotentCommand(Guid RequestId) : ICommand, IIdempotentCommandBase;

public record IdempotentCommand<TResponse>(Guid RequestId) : ICommand<TResponse>, IIdempotentCommandBase;

public interface IIdempotentCommandBase
{
    Guid RequestId { get; init; }
}