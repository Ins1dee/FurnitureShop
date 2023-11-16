using FurnitureShop.Application.Abstractions;
using FurnitureShop.Domain.Entities.UserRegistrations.DomainEvents;
using MailKit.Net.Smtp;
using MailKit.Security;
using MediatR;
using MimeKit;
using MimeKit.Text;

namespace FurnitureShop.Application.Features.UserRegistrations.Commands.Create;

internal sealed class UserRegistrationCreatedDomainEventHandler 
    : INotificationHandler<UserRegistrationCreatedDomainEvent>
{
    private readonly IEmailService _emailService;

    public UserRegistrationCreatedDomainEventHandler(IEmailService emailService)
    {
        _emailService = emailService;
    }

    public async Task Handle(
        UserRegistrationCreatedDomainEvent notification,
        CancellationToken cancellationToken)
    {
        const string subject = "Finish you registration";
        var body = $"Code : {notification.Code}";
        
        await _emailService.SendEmailAsync(notification.Email, subject, body, cancellationToken);
    }
}