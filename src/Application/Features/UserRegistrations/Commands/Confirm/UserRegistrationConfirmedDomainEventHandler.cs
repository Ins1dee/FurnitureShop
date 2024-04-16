using Application.Abstractions;
using Domain.Entities.UserRegistrations.DomainEvents;
using Domain.Entities.Users;
using MediatR;

namespace Application.Features.UserRegistrations.Commands.Confirm;

public sealed class UserRegistrationConfirmedDomainEventHandler 
    : INotificationHandler<UserRegistrationConfirmedDomainEvent>
{
    private readonly IUserRepository _userRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UserRegistrationConfirmedDomainEventHandler(IUserRepository userRepository, IUnitOfWork unitOfWork)
    {
        _userRepository = userRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(UserRegistrationConfirmedDomainEvent notification, CancellationToken cancellationToken)
    {
        User user = User.CreateFromUserRegistration(
            notification.UserRegistrationId,
            notification.FullName,
            notification.Email,
            notification.PasswordHash,
            notification.Role);

        await _userRepository.AddAsync(user, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}