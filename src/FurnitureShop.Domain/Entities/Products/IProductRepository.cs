namespace FurnitureShop.Domain.Entities.Products;

public interface IProductRepository
{
    Task AddAsync(Product product, CancellationToken cancellationToken = default);
}