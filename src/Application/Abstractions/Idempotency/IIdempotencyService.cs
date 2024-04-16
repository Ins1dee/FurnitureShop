namespace Application.Abstractions.Idempotency;

public interface IIdempotencyService
{
    Task<bool> RequestExistsAsync(Guid requestId, CancellationToken cancellationToken = default);

    Task CreateRequestAsync(Guid requstId, string name, CancellationToken cancellationToken = default);
}