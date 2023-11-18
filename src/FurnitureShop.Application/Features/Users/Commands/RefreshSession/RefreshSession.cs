using FurnitureShop.Application.Abstractions;
using FurnitureShop.Application.Abstractions.Idempotency;
using FurnitureShop.Application.Abstractions.Messaging;
using FurnitureShop.Domain.Entities.Users;
using FurnitureShop.Domain.Shared;

namespace FurnitureShop.Application.Features.Users.Commands.RefreshSession;

public sealed record RefreshSessionCommand() : ICommand<RefreshSessionResponse>;

public sealed class RefreshSessionCommandhandler : ICommandHandler<RefreshSessionCommand, RefreshSessionResponse>
{
    private readonly ISessionService _sessionService;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IJwtProvider _jwtProvider;

    public RefreshSessionCommandhandler(ISessionService sessionService, IUnitOfWork unitOfWork, IJwtProvider jwtProvider)
    {
        _sessionService = sessionService;
        _unitOfWork = unitOfWork;
        _jwtProvider = jwtProvider;
    }

    public async Task<Result<RefreshSessionResponse>> Handle(RefreshSessionCommand request, CancellationToken cancellationToken)
    {
        User user = await _sessionService.GetLoggedInUserAsync();
        var refreshToken = _sessionService.GetCurrentSessionFromCookies();

        Result<Domain.Entities.RefreshSessions.RefreshSession> updateResult = user.UpdateRefreshSession(refreshToken);

        if (updateResult.IsFailure)
        {
            return Result.BadRequest<RefreshSessionResponse>(updateResult.Error);
        }
        
        _sessionService.DeleteCurrentSessionFromCookies();
        _sessionService.SetSessionInCookies(updateResult.Value);
        
        
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return new RefreshSessionResponse(_jwtProvider.Generate(user));
    }
}