using Domain.Abstractions;
using Domain.Entities.OrderDetails.ValueObjects;
using Domain.Entities.Products;

namespace Domain.Entities.OrderDetails;

public sealed class OrderDetail : Entity<OrderDetailId>
{
    private OrderDetail(
        OrderDetailId id,
        ProductId productId,
        Quantity quantity,
        Product product) 
        : base(id)
    {
        ProductId = productId;
        Quantity = quantity;
        Product = product;
    }

    private OrderDetail()
    {
        // For EF Core
    }

    public ProductId ProductId { get; private set; }

    public Quantity Quantity { get; private set; }
    
    public Product Product { get; private set; }
}