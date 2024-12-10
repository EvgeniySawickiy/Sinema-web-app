using BookingService.Core.Entities;

namespace BookingService.DataAccess.Persistence.Interfaces
{
    public interface IBookingRepository : IRepository<Booking>
    {
        Task<IEnumerable<Booking>> GetByUserIdAsync(Guid userId);
    }
}
