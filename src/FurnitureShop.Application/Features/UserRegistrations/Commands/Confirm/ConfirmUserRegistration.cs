using FluentValidation;
using FurnitureShop.Application.Abstractions;
using FurnitureShop.Application.Abstractions.Messaging;
using FurnitureShop.Domain.Entities.UserRegistrations;
using FurnitureShop.Domain.Errors;
using FurnitureShop.Domain.Shared;

namespace FurnitureShop.Application.Features.UserRegistrations.Commands.Confirm;

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
            return Result.Failure(DomainErrors.UserRegistration.NotFound());
        }

        Result confirmationResult = userRegistration.Confirm(request.ConfirmationCode);
        
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        
        return confirmationResult.IsFailure 
            ? Result.Failure(confirmationResult.Error) 
            : Result.Success();
    }
}