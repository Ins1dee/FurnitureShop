namespace Application.Abstractions.Caching;

public interface ICashService
{
    Task<T?> GetAsync<T>(string key, CancellationToken cancellationToken = default)
        where T: class;

    Task SetAsync<T>(string key, T value, CancellationToken cancellationToken = default)
        where T : class;

    Task RemoveAsync(string key, CancellationToken cancellationToken);

    Task<bool> AnyAsync(string key, CancellationToken cancellationToken = default);
}