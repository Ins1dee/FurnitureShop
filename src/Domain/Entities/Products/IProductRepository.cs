namespace Domain.Entities.Products;

public interface IProductRepository
{
    Task AddAsync(Product product, CancellationToken cancellationToken = default);

    Task<List<Product>> GetAsync(CancellationToken cancellationToken = default);

    Task<Product?> GetByIdAsync(ProductId id, CancellationToken cancellationToken = default);

    void Delete(Product product);
}