using BookingService.Core.Entities;
using BookingService.DataAccess.Persistence.Interfaces;
using MongoDB.Driver;
using System.Linq.Expressions;

namespace BookingService.DataAccess.Persistence.Repositories
{
    public class SeatRepository : IRepository<Seat>
    {
        private readonly IMongoCollection<Seat> _collection;

        public SeatRepository(MongoContext context)
        {
            _collection = context.GetCollection<Seat>("Seats");
        }

        public async Task<IEnumerable<Seat>> GetAllAsync()
        {
            return await _collection.Find(_ => true).ToListAsync();
        }

        public async Task<Seat> GetByIdAsync(Guid id)
        {
            return await _collection.Find(s => s.Id == id).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Seat>> FindAsync(Expression<Func<Seat, bool>> predicate)
        {
            return await _collection.Find(predicate).ToListAsync();
        }

        public async Task AddAsync(Seat entity)
        {
            await _collection.InsertOneAsync(entity);
        }

        public async Task UpdateAsync(Guid id, Seat entity)
        {
            await _collection.ReplaceOneAsync(s => s.Id == id, entity);
        }

        public async Task DeleteAsync(Guid id)
        {
            await _collection.DeleteOneAsync(s => s.Id == id);
        }
    }
}
