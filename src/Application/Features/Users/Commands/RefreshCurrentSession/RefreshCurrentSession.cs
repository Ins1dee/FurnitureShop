using Application.Abstractions;
using Application.Abstractions.Messaging;
using Domain.Entities.RefreshSessions;
using Domain.Entities.Users;
using Domain.Errors;
using Domain.Shared;
using Serilog;

namespace Application.Features.Users.Commands.RefreshCurrentSession;

public sealed record RefreshSessionCommand() : ICommand<RefreshSessionResponse>;

public sealed class RefreshSessionCommandhandler : ICommandHandler<RefreshSessionCommand, RefreshSessionResponse>
{
    private readonly ISessionService _sessionService;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IJwtProvider _jwtProvider;
    private readonly ILogger _logger;

    public RefreshSessionCommandhandler(
        ISessionService sessionService,
        IUnitOfWork unitOfWork,
        IJwtProvider jwtProvider,
        ILogger logger)
    {
        _sessionService = sessionService;
        _unitOfWork = unitOfWork;
        _jwtProvider = jwtProvider;
        _logger = logger;
    }

    public async Task<Result<RefreshSessionResponse>> Handle(RefreshSessionCommand request, CancellationToken cancellationToken)
    {
        User? user = await _sessionService.GetLoggedInUserAsync(cancellationToken);

        if (user is null)
        {
            _logger.Error($"An error occured tryng to refresh current session. " +
                          $"Error message: {DomainErrors.User.Unauthorized()}. " +
                          $"Status code: 401");

            return Result.Unauthorized<RefreshSessionResponse>(DomainErrors.User.Unauthorized());
        }
        
        var refreshToken = _sessionService.GetCurrentSessionFromCookies();

        Result<RefreshSession> updateResult = user
            .UpdateRefreshSession(refreshToken);

        if (updateResult.IsFailure)
        {
            _logger.Error($"An error occured tryng to refresh current session. " +
                          $"Error message: {updateResult.Error.Message}. " +
                          $"Status code: 400");

            return Result.BadRequest<RefreshSessionResponse>(updateResult.Error);
        }
        
        _sessionService.DeleteCurrentSessionFromCookies();
        _sessionService.SetSessionInCookies(updateResult.Value);
        
        
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return new RefreshSessionResponse(_jwtProvider.Generate(user));
    }
}