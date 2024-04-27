using Application.Abstractions;
using Application.Abstractions.Messaging;
using FluentValidation;
using Domain.Entities.UserRegistrations;
using Domain.Errors;
using Domain.Shared;
using Serilog;

namespace Application.Features.UserRegistrations.Commands.Confirm;

public sealed record ConfirmUserRegistrationCommand(
    Guid UserRegistrationId,
    string ConfirmationCode) : ICommand;

public sealed class ConfirmUserRegistrationCommandValidator : AbstractValidator<ConfirmUserRegistrationCommand>
{
    public ConfirmUserRegistrationCommandValidator()
    {
        RuleFor(x => x.UserRegistrationId)
            .NotNull();

        RuleFor(x => x.ConfirmationCode)
            .NotNull();
    }
}

public sealed class ConfirmUserRegistrationCommandHandler : ICommandHandler<ConfirmUserRegistrationCommand>
{
    private readonly IUserRegistrationRepository _userRegistrationRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger _logger;

    public ConfirmUserRegistrationCommandHandler(
        IUserRegistrationRepository userRegistrationRepository,
        IUnitOfWork unitOfWork,
        ILogger logger)
    {
        _userRegistrationRepository = userRegistrationRepository;
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<Result> Handle(ConfirmUserRegistrationCommand request, CancellationToken cancellationToken)
    {
        UserRegistration? userRegistration = await _userRegistrationRepository
            .GetByIdAsync(new UserRegistrationId(request.UserRegistrationId), cancellationToken);

        if (userRegistration is null)
        {
            _logger.Error($"An error occured tryng to confirm user registration" +
                          $" with id {request.UserRegistrationId}. " +
                          $"Error message: {DomainErrors.UserRegistration.NotFound()}. " +
                          $"Status code: 404");

            return Result.NotFound(DomainErrors.UserRegistration.NotFound());
        }

        Result confirmationResult = userRegistration.Confirm(request.ConfirmationCode);
        
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return confirmationResult.IsFailure 
            ? confirmationResult
            : Result.Success();
    }
}