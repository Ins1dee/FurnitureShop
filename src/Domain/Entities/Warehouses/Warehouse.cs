using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Abstractions;
using Domain.Entities.OrderDetails;
using Domain.Entities.Orders.ValueObjects;
using Domain.Entities.WarehouseProducts;
using Domain.Entities.Warehouses.ValueObjects;
using Domain.Shared.ValueObjects;

namespace Domain.Entities.Warehouses;

public sealed class Warehouse : Entity<WarehouseId>, IAggregateRoot
{
    private readonly List<WarehouseProduct> _products = new();

    public Warehouse(
        WarehouseId id, 
        Location location, 
        QuantityLimit quantityLimit) 
        : base(id)
    {
        Location = location;
        QuantityLimit = quantityLimit;
    }

    public Warehouse()
    {
        // For EF
    }

    public Location Location { get; private set; }

    public QuantityLimit QuantityLimit { get; private set; }

    public IReadOnlyList<WarehouseProduct> Products => _products;
}