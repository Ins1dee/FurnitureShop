using Application.Abstractions;
using Application.Abstractions.Messaging;
using FluentValidation;
using Domain.Entities.UserRegistrations;
using Domain.Errors;
using Domain.Shared;

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

    public ConfirmUserRegistrationCommandHandler(IUserRegistrationRepository userRegistrationRepository, IUnitOfWork unitOfWork)
    {
        _userRegistrationRepository = userRegistrationRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(ConfirmUserRegistrationCommand request, CancellationToken cancellationToken)
    {
        UserRegistration? userRegistration = await _userRegistrationRepository
            .GetByIdAsync(new UserRegistrationId(request.UserRegistrationId), cancellationToken);

        if (userRegistration is null)
        {
            return Result.NotFound(DomainErrors.UserRegistration.NotFound());
        }

        Result confirmationResult = userRegistration.Confirm(request.ConfirmationCode);
        
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        
        return confirmationResult.IsFailure 
            ? confirmationResult
            : Result.Success();
    }
}