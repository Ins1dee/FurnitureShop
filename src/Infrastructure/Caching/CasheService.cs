using Application.Abstractions.Caching;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;

namespace Infrastructure.Caching;

public class CasheService : ICashService
{
    private readonly IDistributedCache _distributedCache;

    public CasheService(IDistributedCache distributedCache)
    {
        _distributedCache = distributedCache;
    }

    public async Task<T?> GetAsync<T>(string key, CancellationToken cancellationToken = default)
        where T : class
    {
        var cashedValue = await _distributedCache.GetStringAsync(key, cancellationToken);

        if (cashedValue is null)
        {
            return null;
        }

        T? value = JsonConvert.DeserializeObject<T>(cashedValue);

        return value;
    }

    public async Task SetAsync<T>(string key, T value, CancellationToken cancellationToken = default)
        where T : class
    {
        var cashedValue = JsonConvert.SerializeObject(value);

        await _distributedCache.SetStringAsync(key, cashedValue, cancellationToken);
    }

    public async Task RemoveAsync(string key, CancellationToken cancellationToken)
    {
        await _distributedCache.RemoveAsync(key, cancellationToken);
    }

    public async Task<bool> AnyAsync(string key, CancellationToken cancellationToken = default)
    {
        var cashedValue = await _distributedCache.GetStringAsync(key, cancellationToken);

        return cashedValue is not null;
    }
}