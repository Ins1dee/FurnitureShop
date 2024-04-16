using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Abstractions;
using Domain.Entities.Products;
using Domain.Entities.Warehouses;
using Domain.Shared.ValueObjects;

namespace Domain.Entities.WarehouseProducts;

public sealed class WarehouseProduct : Entity<WarehouseProductId>, IAggregateRoot
{
    public WarehouseProduct(
        WarehouseProductId id,
        ProductId productId,
        WarehouseId warehouseId,
        Quantity quantity) 
        : base(id)
    {
        ProductId = productId;
        WarehouseId = warehouseId;
        Quantity = quantity;
    }

    public WarehouseProduct()
    {
        // For EF
    }

    public ProductId ProductId { get; private set; }

    public WarehouseId WarehouseId { get; private set; }

    public Quantity Quantity { get; private set; }

    public Product? Product { get; private set; }

    public Warehouse? Warehouse { get; private set; }

    public void Update(ProductId newProductId, WarehouseId newWarehouseId, Quantity newQuantity)
    {
        ProductId = newProductId;
        WarehouseId = newWarehouseId;
        Quantity = newQuantity;
    }
}
