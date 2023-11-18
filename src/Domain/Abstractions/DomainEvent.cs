using MediatR;

namespace Domain.Abstractions;

public record DomainEvent(Guid Id) : INotification;