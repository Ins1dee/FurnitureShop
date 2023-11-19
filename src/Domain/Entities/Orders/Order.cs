using Domain.Abstractions;
using Domain.Entities.OrderDetails;
using Domain.Entities.Orders.ValueObjects;
using Domain.Entities.Users;

namespace Domain.Entities.Orders;

public sealed class Order : Entity<OrderId>, IAggregateRoot
{
    private Order(OrderId id) 
        : base(id)
    {
        
    }

    private Order()
    {
        // For EF Core
    }

    public UserId UserId { get; private set; }
    
    public List<OrderDetail> OrderDetails { get; private set; }
    
    public CustomerDetails CustomerDetails { get; private set; }
    
    public TotalAmount TotalAmount { get; private set; }
    
    public OrderStatus Status { get; private set; }
    
    public DateTime CreatedAtUtc { get; private set; }
    
    public User User { get; private set; }

}