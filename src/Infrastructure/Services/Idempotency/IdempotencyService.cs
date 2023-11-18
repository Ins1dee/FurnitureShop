using Application.Abstractions.Idempotency;
using Infrastructure.Caching;

namespace Infrastructure.Services.Idempotency;

public sealed class IdempotencyService : IIdempotencyService
{
    private readonly CasheService _casheService;

    public IdempotencyService(CasheService casheService)
    {
        _casheService = casheService;
    }

    public async Task<bool> RequestExistsAsync(Guid requestId, CancellationToken cancellationToken = default)
    {
        var key = $"request-{requestId}";

        return await _casheService.AnyAsync(key, cancellationToken);
    }

    public async Task CreateRequestAsync(Guid requstId, string name, CancellationToken cancellationToken = default)
    {
        var key = $"request-{requstId}";
        IdempotentRequest idempotentRequest = new(requstId, name, DateTime.UtcNow);
        
        await _casheService.SetAsync(key, idempotentRequest, cancellationToken);
    }
}