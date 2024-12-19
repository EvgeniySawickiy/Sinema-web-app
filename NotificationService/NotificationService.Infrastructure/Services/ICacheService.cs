
namespace NotificationService.Infrastructure.Services
{
    public interface ICacheService
    {
        Task<T?> GetCacheAsync<T>(string key);
        Task SetCacheAsync<T>(string key, T value, TimeSpan expirationTime);
        Task RemoveCacheAsync(string key);
    }
}
