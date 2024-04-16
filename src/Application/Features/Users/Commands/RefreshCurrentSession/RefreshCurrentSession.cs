using Application.Abstractions;
using Application.Abstractions.Messaging;
using Domain.Entities.RefreshSessions;
using Domain.Entities.Users;
using Domain.Errors;
using Domain.Shared;

namespace Application.Features.Users.Commands.RefreshCurrentSession;

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
        User? user = await _sessionService.GetLoggedInUserAsync();

        if (user is null)
        {
            return Result.Unauthorized<RefreshSessionResponse>(DomainErrors.User.Unauthorized());
        }
        
        var refreshToken = _sessionService.GetCurrentSessionFromCookies();

        Result<RefreshSession> updateResult = user
            .UpdateRefreshSession(refreshToken);

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