using NotificationService.Core.Entities;

namespace NotificationService.Infrastructure.Services;

public interface IUserService
{
    Task<User?> GetUserByIdAsync(Guid userId);
}