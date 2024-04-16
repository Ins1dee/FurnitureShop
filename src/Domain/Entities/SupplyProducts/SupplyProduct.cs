using Domain.Abstractions;
using Domain.Entities.Expenses;
using Domain.Entities.Products;
using Domain.Entities.Suppliers;
using Domain.Shared.ValueObjects;

namespace Domain.Entities.SupplyProducts;

public sealed class SupplyProduct: Entity<SupplyProductId>, IAggregateRoot
{
    private readonly List<Expense> _expenses = new();

    public SupplyProduct(
        SupplyProductId id, 
        ProductId productId, 
        SupplierId supplierId, 
        Quantity quantity) 
        : base(id)
    {
        ProductId = productId;
        SupplierId = supplierId;
        Quantity = quantity;
    }

    public SupplyProduct()
    {
        // For EF
    }

    public ProductId ProductId { get; private set; }

    public SupplierId SupplierId { get; private set; }

    public DateTime CreatedAtUtc { get; private set; }

    public Quantity Quantity { get; private set; }

    public Product? Product { get; private set; }

    public Supplier? Supplier { get; private set; }

    public IReadOnlyList<Expense> Expenses => _expenses;

    public void AddExpenses(List<Expense> expenses)
    {
        _expenses.AddRange(expenses);
    }
}