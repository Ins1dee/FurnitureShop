using FurnitureShop.Domain.Abstractions;
using FurnitureShop.Domain.Entities.UserRegistrations.DomainEvents;
using FurnitureShop.Domain.Entities.UserRegistrations.ValueObjects;
using FurnitureShop.Domain.Errors;
using FurnitureShop.Domain.Shared;
using FurnitureShop.Domain.Shared.ValueObjects;

namespace FurnitureShop.Domain.Entities.UserRegistrations;

public sealed class UserRegistration : Entity<UserRegistrationId>, IAggregateRoot
{
    private UserRegistration(
        UserRegistrationId id, 
        FullName fullName, 
        Email email, 
        PasswordHash passwordHash, 
        DateTime registeredAtUtc,
        UserRegistrationStatus status, 
        ConfirmationCode confirmationCode) 
        : base(id)
    {
        FullName = fullName;
        Email = email;
        PasswordHash = passwordHash;
        RegisteredAtUtc = registeredAtUtc;
        Status = status;
        ConfirmationCode = confirmationCode;
    }

    private UserRegistration()
    {
        // For EF Core
    }
    
    public FullName FullName { get; private set; }
    
    public Email Email { get; private set; }

    public PasswordHash PasswordHash { get; private set; }
    
    public DateTime RegisteredAtUtc { get; private set; }
    
    public UserRegistrationStatus Status { get; private set; }
    
    public ConfirmationCode ConfirmationCode { get; private set; }
    
    public DateTime? ConfirmedAtUtc { get; private set; }

    public static UserRegistration Create(
        UserRegistrationId id,
        FullName fullName,
        Email email,
        PasswordHash passwordHash)
    {
        UserRegistration userRegistration = new(
            id, 
            fullName,
            email,
            passwordHash,
            DateTime.UtcNow,
            UserRegistrationStatus.WaitingForConfirmation,
            ConfirmationCode.Create(out var code));

        userRegistration.RaiseDomainEvent(
            new UserRegistrationCreatedDomainEvent(Guid.NewGuid(), email.Value, code));
        
        return userRegistration;
    }

    public Result Confirm(string code)
    {
        if (Status == UserRegistrationStatus.Confirmed)
        {
            return Result.Failure(DomainErrors.UserRegistration.AlreadyConfirmed());
        }
        
        if (!ConfirmationCode.Verify(code))
        {
            return Result.Failure(DomainErrors.UserRegistration.InvalidOrExpiredConfirmationCode());
        }
        
        Status = UserRegistrationStatus.Confirmed;
        ConfirmedAtUtc = DateTime.UtcNow;
        
        RaiseDomainEvent(
            new UserRegistrationConfirmedDomainEvent(
                Guid.NewGuid(), 
                Id,
                FullName.Create(FullName.Firstname, FullName.Lastname),
                Email.Create(Email.Value),
                PasswordHash.Create(PasswordHash.Value)));
        
        return Result.Success();
    }
}