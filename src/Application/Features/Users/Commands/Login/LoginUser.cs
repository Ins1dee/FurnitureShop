using Application.Abstractions;
using Application.Abstractions.Idempotency;
using Application.Abstractions.Messaging;
using FluentValidation;
using Domain.Entities.Users;
using Domain.Errors;
using Domain.Shared;

namespace Application.Features.Users.Commands.Login;

public sealed record LoginUserCommand(
    Guid RequestId,
    string Email,
    string Password) : IdempotentCommand<LoginResponse>(RequestId);

public sealed class LoginUserCommandValidator : AbstractValidator<LoginUserCommand>
{
    public LoginUserCommandValidator()
    {
        RuleFor(x => x.Email)
            .NotNull()
            .NotEmpty()
            .EmailAddress();

        RuleFor(x => x.Password)
            .NotNull()
            .NotEmpty();
    }
}

public sealed class LoginUserCommandhandler : ICommandHandler<LoginUserCommand, LoginResponse>
{
    private readonly IUserRepository _userRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IJwtProvider _jwtProvider;
    private readonly ISessionService _sessionService;

    public LoginUserCommandhandler(
        IUserRepository userRepository, 
        IUnitOfWork unitOfWork, 
        IJwtProvider jwtProvider, 
        ISessionService sessionService)
    {
        _userRepository = userRepository;
        _unitOfWork = unitOfWork;
        _jwtProvider = jwtProvider;
        _sessionService = sessionService;
    }

    public async Task<Result<LoginResponse>> Handle(LoginUserCommand request, CancellationToken cancellationToken)
    {
        User? user = await _userRepository.GetByEmailAsync(request.Email, cancellationToken);

        if (user is null || !user.PasswordHash.Verify(request.Password))
        {
            return Result.BadRequest<LoginResponse>(DomainErrors.User.InvalidCredentials());
        }

        Domain.Entities.RefreshSessions.RefreshSession refreshSession = user.CreateRefreshSession();
        var accessToken = _jwtProvider.Generate(user);
        
        _sessionService.SetSessionInCookies(refreshSession);
        
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return new LoginResponse(accessToken);
    }
}