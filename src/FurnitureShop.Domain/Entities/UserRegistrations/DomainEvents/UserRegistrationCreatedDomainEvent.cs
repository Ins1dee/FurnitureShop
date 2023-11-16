using FurnitureShop.Domain.Abstractions;

namespace FurnitureShop.Domain.Entities.UserRegistrations.DomainEvents;

public sealed record UserRegistrationCreatedDomainEvent(
    Guid Id,
    string Email,
    string Code) : DomainEvent(Id);