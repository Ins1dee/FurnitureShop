using Domain.Abstractions;

namespace Domain.Entities.UserRegistrations.DomainEvents;

public sealed record UserRegistrationCreatedDomainEvent(
    Guid Id,
    string Email,
    string Code) : DomainEvent(Id);