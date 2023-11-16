namespace FurnitureShop.Domain.Entities.Categories;

public interface ICategoryRepository
{
    Task<Category?> GetByIdAsync(CategoryId id, CancellationToken cancellationToken = default);

    Task<List<Category>?> GetRangeByName(List<string> names, CancellationToken cancellationToken = default);
}