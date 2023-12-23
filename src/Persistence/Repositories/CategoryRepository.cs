using Domain.Entities.Categories;
using Domain.Shared.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace Persistence.Repositories;

internal sealed class CategoryRepository
    : Repository<Category, CategoryId>, ICategoryRepository
{
    public CategoryRepository(ApplicationDbContext dbContext) : base(dbContext)
    {
    }

    public async Task<List<Category>?> GetRangeByName(
        List<string> names, 
        CancellationToken cancellationToken = default)
    {
        var categories = await DbContext.Categories
            .Where(c => names.Select(Name.Create).Contains(c.Name))
            .ToListAsync(cancellationToken);
        
        return names.All(name => categories.Any(category => category.Name.Value == name)) 
            ? categories 
            : null;
    }

    public async Task<bool> IsNameUniqueAsync(string name, CancellationToken cancellationToken = default)
    {
        return !await DbContext.Categories.AnyAsync(
            category => category.Name == Name.Create(name), cancellationToken);
    }

    public void AddAsync(Category category, CancellationToken cancellationToken = default)
    {
        DbContext.Categories.AddAsync(category, cancellationToken);
    }
}