using FurnitureShop.Domain.Entities.Products;
using Microsoft.EntityFrameworkCore;

namespace FurnitureShop.Persistence.Repositories;

internal sealed class ProductRepository
    : Repository<Product, ProductId>, IProductRepository
{
    public ProductRepository(ApplicationDbContext dbContext) : base(dbContext)
    {
    }

    public async Task<List<Product>> GetAsync(CancellationToken cancellationToken = default) =>
        await DbContext.Products
            .Include(p => p.Categories)
            .ToListAsync(cancellationToken);

    public override async Task<Product?> GetByIdAsync(
        ProductId id, 
        CancellationToken cancellationToken = default) =>
        await DbContext.Products
            .Include(p => p.Categories)
            .SingleOrDefaultAsync(p => p.Id == id, cancellationToken);

    public void Delete(Product product)
    {
        DbContext.Products.Remove(product);
    }
}