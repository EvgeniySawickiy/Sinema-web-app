using NotificationService.Core.Entities;

namespace NotificationService.Infrastructure.Services;

public class UserService : IUserService
{
    public Task<User?> GetUserByIdAsync(Guid userId)
    {
        throw new NotImplementedException();
    }
}