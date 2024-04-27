using Domain.Abstractions;
using Domain.Entities.Orders;
using Domain.Entities.Users;
using Domain.Errors;
using Domain.Shared;
using Domain.Shared.ValueObjects;

namespace Domain.Entities.Deliveries;

public class Delivery : Entity<DeliveryId>, IAggregateRoot
{
    public Delivery(
        DeliveryId id, 
        OrderId orderId, 
        DateTime createdAtUtc, 
        DateTime arrivesAtUtc, 
        bool delivered, 
        Order? order, 
        Location address, 
        Amount cost) 
        : base(id)
    {
        OrderId = orderId;
        CreatedAtUtc = createdAtUtc;
        ArrivesAtUtc = arrivesAtUtc;
        Delivered = delivered;
        Order = order;
        Address = address;
        Cost = cost;
    }

    public Delivery()
    {
        // For EF
    }

    public OrderId OrderId { get; private set; }

    public UserId? UserId { get; private set; }

    public DateTime CreatedAtUtc { get; private set; }

    public DateTime ArrivesAtUtc { get; private set; }

    public Location Address { get; private set; }

    public Amount Cost { get; private set; }

    public bool Delivered { get; private set; }

    public Order? Order { get; private set; }

    public User? User { get; private set; }

    public void Update(OrderId orderId, DateTime createdAtUtc, Location location, DateTime arriesAtUtc, bool delivered)
    {
        OrderId = orderId;
        CreatedAtUtc = createdAtUtc;
        ArrivesAtUtc = arriesAtUtc;
        Delivered = delivered;
    }

    public static Delivery Create(OrderId orderId, Location address, Amount cost)
    {
        return new Delivery(
            new DeliveryId(Guid.NewGuid()),
            orderId,
            DateTime.UtcNow,
            DateTime.UtcNow.AddDays(3), 
            false,
            null,
            address,
            cost);
    }

    public Result AttachToUser(UserId userId)
    {
        if (UserId is not null || Delivered)
        {
            return Result.BadRequest(DomainErrors.Delivery.CantAttach());
        }
        UserId = userId;

        return Result.Success();
    }
}