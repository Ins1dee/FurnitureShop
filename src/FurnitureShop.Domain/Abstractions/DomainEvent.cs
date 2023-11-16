using MediatR;

namespace FurnitureShop.Domain.Abstractions;

public record DomainEvent(Guid Id) : INotification;