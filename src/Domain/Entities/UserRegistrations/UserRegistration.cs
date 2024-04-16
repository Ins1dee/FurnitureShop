using Domain.Abstractions;
using Domain.Entities.Roles;
using Domain.Entities.UserRegistrations.DomainEvents;
using Domain.Entities.UserRegistrations.ValueObjects;
using Domain.Errors;
using Domain.Shared;
using Domain.Shared.ValueObjects;

namespace Domain.Entities.UserRegistrations;

public sealed class UserRegistration : Entity<UserRegistrationId>, IAggregateRoot
{
    private UserRegistration(
        UserRegistrationId id, 
        FullName fullName, 
        Email email, 
        PasswordHash passwordHash,
        Role role,
        DateTime registeredAtUtc,
        UserRegistrationStatus status, 
        ConfirmationCode confirmationCode) 
        : base(id)
    {
        FullName = fullName;
        Email = email;
        PasswordHash = passwordHash;
        Role = role;
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

    public Role Role { get; private set; }
    
    public DateTime RegisteredAtUtc { get; private set; }
    
    public UserRegistrationStatus Status { get; private set; }
    
    public ConfirmationCode ConfirmationCode { get; private set; }
    
    public DateTime? ConfirmedAtUtc { get; private set; }

    public static UserRegistration Create(
        UserRegistrationId id,
        FullName fullName,
        Email email,
        PasswordHash passwordHash,
        Role role)
    {
        UserRegistration userRegistration = new(
            id, 
            fullName,
            email,
            passwordHash,
            role,
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
            return Result.BadRequest(DomainErrors.UserRegistration.AlreadyConfirmed());
        }
        
        if (!ConfirmationCode.Verify(code))
        {
            return Result.BadRequest(DomainErrors.UserRegistration.InvalidOrExpiredConfirmationCode());
        }
        
        Status = UserRegistrationStatus.Confirmed;
        ConfirmedAtUtc = DateTime.UtcNow;
        
        RaiseDomainEvent(
            new UserRegistrationConfirmedDomainEvent(
                Guid.NewGuid(), 
                Id,
                FullName.Create(FullName.Firstname, FullName.Lastname),
                Email.Create(Email.Value),
                PasswordHash.Create(PasswordHash.Value),
                Role));
        
        return Result.Success();
    }
}