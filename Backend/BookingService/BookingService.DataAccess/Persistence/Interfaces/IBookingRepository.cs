using BookingService.Core.Entities;

namespace BookingService.DataAccess.Persistence.Interfaces
{
    public interface IBookingRepository : IRepository<Booking>
    {
        Task<IEnumerable<Booking>> GetByUserIdAsync(Guid userId);
        Task<int> GetTotalCountAsync();
        Task<IEnumerable<Booking>> GetPagedAsync(int pageNumber, int pageSize);
        Task<decimal> GetTotalRevenueAsync();
        Task<List<BookingByDay>> GetBookingsByDayAsync();
    }
}
