namespace Domain.Entities.SupplyProducts;

public interface ISupplyProductRepository
{
    Task AddRangeAsync(List<SupplyProduct> supplyProducts, CancellationToken cancellationToken = default);
}