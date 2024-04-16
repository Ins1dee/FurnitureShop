using Domain.Abstractions;
using Domain.Entities.Roles;
using Domain.Shared.ValueObjects;

namespace Domain.Entities.UserRegistrations.DomainEvents;

public sealed record UserRegistrationConfirmedDomainEvent(
    Guid Id,
    UserRegistrationId UserRegistrationId,
    FullName FullName,
    Email Email,
    PasswordHash PasswordHash,
    Role Role) : DomainEvent(Id);