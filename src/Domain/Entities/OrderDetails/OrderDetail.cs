using Domain.Abstractions;
using Domain.Entities.Orders;
using Domain.Entities.Products;
using Domain.Shared.ValueObjects;

namespace Domain.Entities.OrderDetails;

public sealed class OrderDetail : Entity<OrderDetailId>
{
    private OrderDetail(
        OrderDetailId id,
        OrderId orderId,
        ProductId productId,
        Quantity quantity,
        Product product) 
        : base(id)
    {
        OrderId = orderId;
        ProductId = productId;
        Quantity = quantity;
        Product = product;
    }

    public OrderDetail()
    {
        // For EF Core
    }

    public OrderId OrderId { get; private set; }
    
    public ProductId ProductId { get; private set; }

    public Quantity Quantity { get; private set; }
    
    public Product Product { get; private set; }

    public static OrderDetail Create(
        OrderDetailId id,
        OrderId orderId, 
        ProductId productId, 
        Product product, 
        Quantity quantity)
    {
        return new OrderDetail(id, orderId, productId, quantity, product);
    }
}