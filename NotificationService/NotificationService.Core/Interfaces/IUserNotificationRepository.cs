using NotificationService.Core.Entities;

namespace NotificationService.Core.Interfaces
{
    public interface IUserNotificationRepository
    {
        Task<IEnumerable<UserNotification>> GetUserNotificationsAsync(Guid userId);
        Task AddUserNotificationAsync(UserNotification userNotification);
        Task MarkAsReadAsync(Guid userNotificationId);
    }
}
