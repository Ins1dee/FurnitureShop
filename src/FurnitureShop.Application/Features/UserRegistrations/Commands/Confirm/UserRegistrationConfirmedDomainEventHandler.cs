using FurnitureShop.Application.Abstractions;
using FurnitureShop.Domain.Entities.UserRegistrations.DomainEvents;
using FurnitureShop.Domain.Entities.Users;
using MediatR;

namespace FurnitureShop.Application.Features.UserRegistrations.Commands.Confirm;

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
            notification.UserRegistrationIdId,
            notification.FullName,
            notification.Email,
            notification.PasswordHash);

        await _userRepository.AddAsync(user);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}