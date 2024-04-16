using Application.Abstractions.Idempotency;
using Domain.Shared;
using MediatR;

namespace Application.Behaviors;

public sealed class IdempotentCommandPipelineBehavior<TRequest, TResponse> 
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IIdempotentCommandBase
    where TResponse : IResult
{
    private readonly IIdempotencyService _idempotencyService;

    public IdempotentCommandPipelineBehavior(IIdempotencyService idempotencyService)
    {
        _idempotencyService = idempotencyService;
    }

    public async Task<TResponse> Handle(
        TRequest request, 
        RequestHandlerDelegate<TResponse> next, 
        CancellationToken cancellationToken)
    {
        if (await _idempotencyService.RequestExistsAsync(request.RequestId, cancellationToken))
        {
            return (TResponse)TResponse.IdempotencyResult();
        }

        await _idempotencyService.CreateRequestAsync(request.RequestId,  typeof(TRequest).Name, cancellationToken);

        return await next();
    }
}