using Application.Abstractions;
using Application.Abstractions.Messaging;
using Domain.Entities.Roles;
using FluentValidation;
using Domain.Entities.UserRegistrations;
using Domain.Errors;
using Domain.Shared;
using Domain.Shared.ValueObjects;
using Serilog;

namespace Application.Features.UserRegistrations.Commands.Create;

public sealed record CreateUserRegistrationCommand(
    string Firstname,
    string Lastname,
    string Email,
    string Password,
    string Role) : ICommand<Guid>;

public sealed class CreateUserRegistrationCommandValidator : AbstractValidator<CreateUserRegistrationCommand>
{
    public CreateUserRegistrationCommandValidator(IUserRegistrationRepository userRegistrationRepository)
    {
        RuleFor(x => x.Email)
            .NotNull()
            .NotEmpty()
            .EmailAddress()
            .MustAsync(async (email, _) => await userRegistrationRepository.IsEmailUniqueAsync(email))
            .WithMessage("The email must be unique");

        RuleFor(x => x.Firstname)
            .NotNull()
            .NotEmpty()
            .MinimumLength(FullName.MinLength)
            .MaximumLength(FullName.MaxLength);

        RuleFor(x => x.Lastname)
            .NotNull()
            .NotEmpty()
            .MinimumLength(FullName.MinLength)
            .MaximumLength(FullName.MaxLength);

        RuleFor(x => x.Password)
            .NotNull()
            .NotEmpty()
            .MinimumLength(PasswordHash.MinLength)
            .MaximumLength(PasswordHash.MaxLength)
            .Matches(PasswordHash.CorrectPasswordPattern)
            .WithMessage("Password must meet the following criteria: at least one lowercase letter, " +
                         "at least one uppercase letter, at least one digit and at least one special character");
    }
}

internal sealed class CreateUserRegistrationCommandHandler : ICommandHandler<CreateUserRegistrationCommand, Guid>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IUserRegistrationRepository _userRegistrationRepository;
    private readonly ILogger _logger;

    public CreateUserRegistrationCommandHandler(
        IUnitOfWork unitOfWork, 
        IUserRegistrationRepository userRegistrationRepository, 
        ILogger logger)
    {
        _unitOfWork = unitOfWork;
        _userRegistrationRepository = userRegistrationRepository;
        _logger = logger;
    }

    public async Task<Result<Guid>> Handle(CreateUserRegistrationCommand request, CancellationToken cancellationToken)
    {
        Result<Role> roleResult = Role.FromString(request.Role);

        if (roleResult.IsFailure)
        {
            _logger.Error($"An error occured tryng to create user registration. " +
                          $"Error message: {roleResult.Error.Message}. " +
                          $"Status code: 404");

            return Result.NotFound<Guid>(roleResult.Error);
        }

        UserRegistration userRegistration = UserRegistration.Create(
            new UserRegistrationId(Guid.NewGuid()),
            FullName.Create(request.Firstname, request.Lastname),
            Email.Create(request.Email),
            PasswordHash.Hash(request.Password),
            roleResult.Value);

        await _userRegistrationRepository.AddAsync(userRegistration, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        _logger.Information($"New user registration with id {userRegistration.Id.Value} was successfully created, " +
                            $"status code: 200");
        
        return userRegistration.Id.Value;
    }
}