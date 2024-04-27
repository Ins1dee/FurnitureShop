using Application.Abstractions;
using Domain.Entities.Deliveries;
using Domain.Entities.Orders.DomainEvents;
using MediatR;

namespace Application.Features.Orders.Commands.Create;

public class OrderShouldBeDeliveredDomainEventHandler : INotificationHandler<OrderShouldBeDeliveredDomainEvent>
{
    private readonly IDeliveryRepository _deliveryRepository;
    private readonly IUnitOfWork _unitOfWork;

    public OrderShouldBeDeliveredDomainEventHandler(
        IDeliveryRepository deliveryRepository,
        IUnitOfWork unitOfWork)
    {
        _deliveryRepository = deliveryRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(OrderShouldBeDeliveredDomainEvent notification, CancellationToken cancellationToken)
    {
        var delivery = Delivery.Create(notification.OrderId, notification.Address, notification.Cost);

        await _deliveryRepository.AddAsync(delivery, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}