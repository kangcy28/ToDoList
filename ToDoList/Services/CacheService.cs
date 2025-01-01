using Microsoft.Extensions.Caching.Memory;
using ToDoList.DTOs;
using ToDoList.Services.Interfaces;

public class CacheService : ICacheService
{
    private readonly IMemoryCache _cache;
    private readonly ILogger<CacheService> _logger;

    public CacheService(IMemoryCache cache, ILogger<CacheService> logger)
    {
        _cache = cache;
        _logger = logger;
    }

    public async Task<TodoDto?> GetTodoAsync(string key)
    {
        return await Task.FromResult(_cache.Get<TodoDto>(key));
    }

    public async Task<IEnumerable<TodoDto>?> GetTodoListAsync(string key)
    {
        return await Task.FromResult(_cache.Get<IEnumerable<TodoDto>>(key));
    }

    public async Task SetTodoAsync(string key, TodoDto value, TimeSpan? expirationTime = null)
    {
        var options = new MemoryCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = expirationTime ?? TimeSpan.FromMinutes(10),
            SlidingExpiration = TimeSpan.FromMinutes(2)
        };

        options.RegisterPostEvictionCallback((k, v, reason, state) =>
        {
            _logger.LogInformation($"Cache entry {k} was removed. Reason: {reason}");
        });

        await Task.FromResult(_cache.Set(key, value, options));
    }

    public async Task SetTodoListAsync(string key, IEnumerable<TodoDto> value, TimeSpan? expirationTime = null)
    {
        var options = new MemoryCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = expirationTime ?? TimeSpan.FromMinutes(10),
            SlidingExpiration = TimeSpan.FromMinutes(2)
        };

        options.RegisterPostEvictionCallback((k, v, reason, state) =>
        {
            _logger.LogInformation($"Cache entry {k} was removed. Reason: {reason}");
        });

        await Task.FromResult(_cache.Set(key, value, options));
    }

    public async Task RemoveAsync(string key)
    {
        _cache.Remove(key);
        await Task.CompletedTask;
    }
}
