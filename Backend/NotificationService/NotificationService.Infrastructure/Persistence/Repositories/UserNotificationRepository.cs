using MongoDB.Driver;
using NotificationService.Core.Entities;
using NotificationService.Core.Interfaces;

namespace NotificationService.Infrastructure.Persistence.Repositories
{
    public class UserNotificationRepository(IMongoDatabase database) : IUserNotificationRepository
    {
        private readonly IMongoCollection<UserNotification> _collection = database.GetCollection<UserNotification>("UserNotifications");

        public async Task<IEnumerable<UserNotification>> GetUserNotificationsAsync(Guid userId)
        {
            return await _collection.Find(un => un.UserId == userId).ToListAsync();
        }

        public async Task AddUserNotificationAsync(UserNotification userNotification)
        {
            await _collection.InsertOneAsync(userNotification);
        }

        public async Task MarkAsReadAsync(Guid userNotificationId)
        {
            var update = Builders<UserNotification>.Update.Set(un => un.IsRead, true);
            await _collection.UpdateOneAsync(un => un.Id == userNotificationId, update);
        }
    }
}
