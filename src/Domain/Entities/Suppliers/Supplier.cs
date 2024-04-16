using Domain.Abstractions;
using Domain.Entities.Orders.ValueObjects;
using Domain.Entities.Suppliers.ValueObjects;
using Domain.Entities.SupplyProducts;
using Domain.Entities.Warehouses.ValueObjects;

namespace Domain.Entities.Suppliers;

public sealed class Supplier : Entity<SupplierId>, IAggregateRoot
{
    private readonly List<SupplyProduct> _products = new();

    public Supplier(
        SupplierId id, 
        CompanyName companyName, 
        ContactDetails contactDetails) 
        : base(id)
    {
        CompanyName = companyName;
        ContactDetails = contactDetails;
    }

    public Supplier()
    {
        // For EF
    }

    public CompanyName CompanyName { get; private set; }

    public ContactDetails ContactDetails { get; private set; }

    public IReadOnlyList<SupplyProduct> Products => _products;
}