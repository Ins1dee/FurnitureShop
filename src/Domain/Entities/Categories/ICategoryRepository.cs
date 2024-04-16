namespace Domain.Entities.Categories;

public interface ICategoryRepository
{
    Task<Category?> GetByIdAsync(CategoryId id, CancellationToken cancellationToken = default);

    Task<List<Category>?> GetRangeByName(List<string> names, CancellationToken cancellationToken = default);
    
    Task<bool> IsNameUniqueAsync(string name, CancellationToken cancellationToken = default);
    
    Task AddAsync(Category category, CancellationToken cancellationToken = default);
    
    void Delete(Category category);

    Task AddRangeAsync(List<Category> categories, CancellationToken cancellationToken = default);

    Task<List<Category?>> GetAsync(CancellationToken cancellationToken = default);
}