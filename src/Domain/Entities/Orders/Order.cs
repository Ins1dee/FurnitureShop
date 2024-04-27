using Domain.Abstractions;
using Domain.Entities.Deliveries;
using Domain.Entities.Incomes;
using Domain.Entities.OrderDetails;
using Domain.Entities.Orders.DomainEvents;
using Domain.Entities.Orders.ValueObjects;
using Domain.Entities.Products;
using Domain.Entities.Users;
using Domain.Shared.ValueObjects;

namespace Domain.Entities.Orders;

public sealed class Order : Entity<OrderId>, IAggregateRoot
{
    private readonly List<OrderDetail> _orderDetails = new();

    private readonly List<Income> _incomes = new();

    private Order(
        OrderId id, 
        UserId userId,
        List<OrderDetail> orderDetails,
        List<Income> incomes,
        CustomerDetails customerDetails,
        Amount totalAmount,
        OrderPaymentStatus paymentStatus,
        DateTime createdAtUtc,
        User user)
        : base(id)
    {
        UserId = userId;
        _orderDetails.AddRange(orderDetails);
        _incomes.AddRange(incomes);
        CustomerDetails = customerDetails;
        TotalAmount = totalAmount;
        PaymentStatus = paymentStatus;
        CreatedAtUtc = createdAtUtc;
        User = user;
    }

    public Order()
    {
        // For EF Core
    }

    public UserId UserId { get; private set; }

    public DeliveryId? DeliveryId { get; private set; }
    
    public CustomerDetails CustomerDetails { get; private set; }
    
    public Amount TotalAmount { get; private set; }
    
    public OrderPaymentStatus PaymentStatus { get; private set; }
    
    public DateTime CreatedAtUtc { get; private set; }

    public Delivery? Delivery { get; private set; }
    
    public User User { get; private set; }

    public IReadOnlyList<OrderDetail> OrderDetails => _orderDetails;

    public IReadOnlyList<Income> Incomes => _incomes;

    public static Order Create(
        OrderId id,
        User user,
        Dictionary<Product, int> products,
        CustomerDetails customerDetails, 
        double paymentAmount,
        DeliveryId? deliveryId = default,
        string? address = default)
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

        var incomes = new List<Income> { Income.Create(id, Amount.Create(paymentAmount)) };

        var order = new Order(
            id,
            user.Id,
            orderDetails,
            incomes,
            customerDetails,
            Amount.Create(0), 
            OrderPaymentStatus.InProcess,
            DateTime.UtcNow,
            user);

        order.PaymentStatus = 
            paymentAmount >= Amount.FromOrderDetails(orderDetails).Value 
                ? OrderPaymentStatus.Finished 
                : OrderPaymentStatus.InProcess;

        if (deliveryId is not null && address is not null)
        {
            order.RaiseDomainEvent(
                new OrderShouldBeDeliveredDomainEvent(
                    Guid.NewGuid(),
                    id,
                    Location.Create(address),
                    Amount.Create(orderDetails.Count * 5)));
        }

        return order;
    }

    public void AddOrderDetails(List<OrderDetail> newOrderDetails)
    {
        _orderDetails.AddRange(newOrderDetails);
    }

    public void SetDelivery(DeliveryId id)
    {
        DeliveryId = id;
    }

    public void AddIncomes(List<Income> incomes)
    {
        _incomes.AddRange(incomes);
    }
}