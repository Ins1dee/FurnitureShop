using Domain.Abstractions;
using Domain.Shared.ValueObjects;

namespace Domain.Entities.UserRegistrations.DomainEvents;

public sealed record UserRegistrationConfirmedDomainEvent(
    Guid Id,
    UserRegistrationId UserRegistrationIdId,
    FullName FullName,
    Email Email,
    PasswordHash PasswordHash) : DomainEvent(Id);