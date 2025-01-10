using BookingService.Core.Entities;

namespace BookingService.Core.Interfaces
{
    public interface IBookingService
    {
        Task<Booking> CreateBookingAsync(Booking booking);
        Task<Booking> GetBookingByIdAsync(Guid id);
        Task<IEnumerable<Booking>> GetBookingsByUserIdAsync(Guid userId);
        Task CancelBookingAsync(Guid id);
    }
}
