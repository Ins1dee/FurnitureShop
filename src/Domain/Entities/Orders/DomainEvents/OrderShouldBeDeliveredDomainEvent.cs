using Domain.Abstractions;
using Domain.Shared.ValueObjects;
using MediatR;

namespace Domain.Entities.Orders.DomainEvents;

public sealed record OrderShouldBeDeliveredDomainEvent(
    Guid Id,
    OrderId OrderId,
    Location Address) : DomainEvent(Id);