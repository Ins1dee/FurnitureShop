namespace Domain.Entities.WarehouseProducts;

public interface IWarehouseProductRepository
{
    Task AddRangeAsync(List<WarehouseProduct> warehouseProducts, CancellationToken cancellationToken = default);
}