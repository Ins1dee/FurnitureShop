using FurnitureShop.Domain.Entities.Products;

namespace FurnitureShop.Persistence.Repositories;

internal sealed class ProductRepository
    : Repository<Product, ProductId>, IProductRepository
{
    public ProductRepository(ApplicationDbContext dbContext) : base(dbContext)
    {
    }
}