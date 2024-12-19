using System.Text.Json;
using StackExchange.Redis;

namespace NotificationService.Infrastructure.Services
{
    public class RedisCacheService : ICacheService
    {
        private readonly IDatabase _cache;

        public RedisCacheService(IConnectionMultiplexer connectionMultiplexer)
        {
            _cache = connectionMultiplexer.GetDatabase();
        }

        public async Task<T?> GetCacheAsync<T>(string key)
        {
            var value = await _cache.StringGetAsync(key);
            if (value.IsNullOrEmpty)
            {
                return default;
            }

            return JsonSerializer.Deserialize<T>(value);
        }

        public async Task SetCacheAsync<T>(string key, T value, TimeSpan expirationTime)
        {
            var json = JsonSerializer.Serialize(value);
            await _cache.StringSetAsync(key, json, expirationTime);
        }

        public async Task RemoveCacheAsync(string key)
        {
            await _cache.KeyDeleteAsync(key);
        }
    }
}
