namespace Domain.Entities.Warehouses;

public interface IWarehouseRepository
{
    Task AddRangeAsync(List<Warehouse> warehouses, CancellationToken cancellationToken = default);
}