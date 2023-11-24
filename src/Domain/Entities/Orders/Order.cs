using Domain.Abstractions;
using Domain.Entities.OrderDetails;
using Domain.Entities.OrderDetails.ValueObjects;
using Domain.Entities.Orders.ValueObjects;
using Domain.Entities.Products;
using Domain.Entities.Users;

namespace Domain.Entities.Orders;

public sealed class Order : Entity<OrderId>, IAggregateRoot
{
    private readonly List<OrderDetail> _orderDetails = new();

    private Order(
        OrderId id, 
        UserId userId,
        List<OrderDetail> orderDetails,
        CustomerDetails customerDetails, 
        TotalAmount totalAmount,
        OrderStatus status,
        DateTime createdAtUtc,
        User user)
        : base(id)
    {
        UserId = userId;
        _orderDetails.AddRange(orderDetails);
        CustomerDetails = customerDetails;
        TotalAmount = totalAmount;
        Status = status;
        CreatedAtUtc = createdAtUtc;
        User = user;
    }

    private Order()
    {
        // For EF Core
    }

    public UserId UserId { get; private set; }
    
    public CustomerDetails CustomerDetails { get; private set; }
    
    public TotalAmount TotalAmount { get; private set; }
    
    public OrderStatus Status { get; private set; }
    
    public DateTime CreatedAtUtc { get; private set; }
    
    public User User { get; private set; }

    public IReadOnlyList<OrderDetail> OrderDetails => _orderDetails;

    public static Order Create(
        OrderId id, 
        User user, 
        Dictionary<Product, int> products,
        CustomerDetails customerDetails)
    {
        var orderDetails = products
            .Select(p => OrderDetail
                .Create(
                    new OrderDetailId(Guid.NewGuid()),
                    id,
                    p.Key.Id,
                    p.Key,
                    Quantity.Create(p.Value)))
            .ToList();
        
        return new Order(
            id,
            user.Id,
            orderDetails,
            customerDetails,
            TotalAmount.FromOrderDetails(orderDetails),
            OrderStatus.InProcess,
            DateTime.UtcNow,
            user);
    }
}