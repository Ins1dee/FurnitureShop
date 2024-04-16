using Domain.Abstractions;
using Domain.Entities.Orders;
using Domain.Entities.Users;
using Domain.Shared.ValueObjects;

namespace Domain.Entities.Deliveries;

public class Delivery : Entity<DeliveryId>, IAggregateRoot
{
    public Delivery(
        DeliveryId id, 
        OrderId orderId, 
        DateTime createdAtUtc, 
        DateTime arrivesAtc, 
        bool delivered, 
        Order? order, 
        Location address) 
        : base(id)
    {
        OrderId = orderId;
        CreatedAtUtc = createdAtUtc;
        ArrivesAtc = arrivesAtc;
        Delivered = delivered;
        Order = order;
        Address = address;
    }

    public Delivery()
    {
        // For EF
    }

    public OrderId OrderId { get; private set; }

    public UserId? UserId { get; private set; }

    public DateTime CreatedAtUtc { get; private set; }

    public DateTime ArrivesAtc { get; private set; }

    public Location Address { get; private set; }

    public bool Delivered { get; private set; }

    public Order? Order { get; private set; }

    public User? User { get; private set; }

    public void Update(OrderId orderId, DateTime createdAtUtc, Location location, DateTime arriesAtUtc, bool delivered)
    {
        OrderId = orderId;
        CreatedAtUtc = createdAtUtc;
        ArrivesAtc = arriesAtUtc;
        Delivered = delivered;
    }

    public static Delivery Create(OrderId orderId, Location address)
    {
        return new Delivery(
            new DeliveryId(Guid.NewGuid()),
            orderId,
            DateTime.UtcNow,
            DateTime.UtcNow.AddDays(3), 
            false,
            null,
            address);
    }

    public void AttachToUser(UserId userId)
    {
        UserId = userId;
    }
}