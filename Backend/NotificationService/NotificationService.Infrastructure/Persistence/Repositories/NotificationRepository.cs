using MongoDB.Driver;
using NotificationService.Core.Entities;
using NotificationService.Core.Enums;
using NotificationService.Core.Interfaces;

namespace NotificationService.Infrastructure.Persistence.Repositories
{
    public class NotificationRepository(IMongoDatabase database) : INotificationRepository
    {
        private readonly IMongoCollection<Notification> _collection = database.GetCollection<Notification>("Notifications");

        public async Task<Notification> GetByIdAsync(Guid id)
        {
            return await _collection.Find(n => n.Id == id).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Notification>> GetPendingNotificationsAsync()
        {
            return await _collection.Find(n => n.Status == NotificationStatus.Pending).ToListAsync();
        }

        public async Task AddAsync(Notification notification)
        {
            await _collection.InsertOneAsync(notification);
        }

        public async Task UpdateAsync(Notification notification)
        {
            await _collection.ReplaceOneAsync(n => n.Id == notification.Id, notification);
        }

        public async Task DeleteAsync(Guid id)
        {
            await _collection.DeleteOneAsync(n => n.Id == id);
        }
    }
}
