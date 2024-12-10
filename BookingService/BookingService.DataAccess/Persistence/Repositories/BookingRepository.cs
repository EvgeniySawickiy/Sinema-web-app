using System.Linq.Expressions;
using BookingService.Core.Entities;
using BookingService.DataAccess.Persistence.Interfaces;
using MongoDB.Driver;

namespace BookingService.DataAccess.Persistence.Repositories
{
    public class BookingRepository : IBookingRepository
    {
        private readonly IMongoCollection<Booking> _collection;

        public BookingRepository(MongoContext context)
        {
            _collection = context.GetCollection<Booking>("Bookings");
        }

        public async Task<IEnumerable<Booking>> GetAllAsync()
        {
            return await _collection.Find(_ => true).ToListAsync();
        }

        public async Task<Booking> GetByIdAsync(Guid id)
        {
            return await _collection.Find(x => x.Id == id).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Booking>> FindAsync(Expression<Func<Booking, bool>> predicate)
        {
            return await _collection.Find(predicate).ToListAsync();
        }

        public async Task AddAsync(Booking entity)
        {
            await _collection.InsertOneAsync(entity);
        }

        public async Task UpdateAsync(Guid id, Booking entity)
        {
            await _collection.ReplaceOneAsync(x => x.Id == id, entity);
        }

        public async Task DeleteAsync(Guid id)
        {
            await _collection.DeleteOneAsync(x => x.Id == id);
        }

        public async Task<IEnumerable<Booking>> GetByUserIdAsync(Guid userId)
        {
            return await _collection.Find(b => b.UserId == userId).ToListAsync();
        }
    }
}
