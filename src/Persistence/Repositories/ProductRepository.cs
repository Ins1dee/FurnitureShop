using Domain.Entities.Products;
using Microsoft.EntityFrameworkCore;

namespace Persistence.Repositories;

internal sealed class ProductRepository
    : Repository<Product, ProductId>, IProductRepository
{
    public ProductRepository(ApplicationDbContext dbContext) : base(dbContext)
    {
    }

    public override async Task<List<Product>> GetAsync(CancellationToken cancellationToken = default) =>
        await DbContext.Products
            .Include(p => p.Categories)
            .ToListAsync(cancellationToken);

    public override async Task<Product?> GetByIdAsync(
        ProductId id, 
        CancellationToken cancellationToken = default) =>
        await DbContext.Products
            .Include(p => p.Categories)
            .SingleOrDefaultAsync(p => p.Id == id, cancellationToken: cancellationToken);

    public async Task<List<Product>?> GetRangeById(List<ProductId> ids, CancellationToken cancellationToken = default)
    {
        var products = await DbContext.Products
            .Where(p => ids.Contains(p.Id))
            .ToListAsync(cancellationToken);
        
        return ids.All(inputId => ids.Any(id => id == inputId)) 
            ? products 
            : null;
    }
}