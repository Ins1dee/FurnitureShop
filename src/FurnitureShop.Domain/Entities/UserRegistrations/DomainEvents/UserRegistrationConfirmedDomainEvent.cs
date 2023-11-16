using FurnitureShop.Domain.Abstractions;
using FurnitureShop.Domain.Shared.ValueObjects;

namespace FurnitureShop.Domain.Entities.UserRegistrations.DomainEvents;

public sealed record UserRegistrationConfirmedDomainEvent(
    Guid Id,
    UserRegistrationId UserRegistrationIdId,
    FullName FullName,
    Email Email,
    PasswordHash PasswordHash) : DomainEvent(Id);