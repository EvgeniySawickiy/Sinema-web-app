using System.Linq.Expressions;
using BookingService.Core.Entities;
using BookingService.DataAccess.Cache;
using BookingService.DataAccess.Persistence.Interfaces;
using MongoDB.Driver;

namespace BookingService.DataAccess.Persistence.Repositories
{
    public class BookingRepository : IBookingRepository
    {
        private readonly IMongoCollection<Booking> _collection;
        private readonly IRedisCacheService _cacheService;

        public BookingRepository(MongoContext context, IRedisCacheService cacheService)
        {
            _collection = context.GetCollection<Booking>("Bookings");
            _cacheService = cacheService;
        }

        public async Task<IEnumerable<Booking>> GetAllAsync()
        {
            return await _collection.Find(_ => true).ToListAsync();
        }

        public async Task<Booking> GetByIdAsync(Guid id)
        {
            var cacheKey = $"booking:{id}";
            var cachedBooking = await _cacheService.GetAsync<Booking>(cacheKey);
            if (cachedBooking != null)
            {
                return cachedBooking;
            }

            var booking = await _collection.Find(b => b.Id == id).FirstOrDefaultAsync();
            if (booking != null)
            {
                await _cacheService.SetAsync(cacheKey, booking, TimeSpan.FromMinutes(10));
            }

            return booking;
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
        
        public async Task<int> GetTotalCountAsync()
        {
            return (int)await _collection.CountDocumentsAsync(FilterDefinition<Booking>.Empty);
        }
        
        public async Task<IEnumerable<Booking>> GetPagedAsync(int pageNumber, int pageSize)
        {
            return await _collection.Find(FilterDefinition<Booking>.Empty)
                .SortBy(b => b.Id)
                .Skip((pageNumber - 1) * pageSize)
                .Limit(pageSize)
                .ToListAsync();
        }
    }
}
