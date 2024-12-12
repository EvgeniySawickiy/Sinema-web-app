using BookingService.Core.Entities;

namespace BookingService.Core.Interfaces
{
    public interface ISeatReservationService
    {
        Task<decimal> ReserveSeatsAsync(Guid showtimeId, List<Seat> seats);
        Task ReleaseSeatsAsync(Guid showtimeId, List<Seat> seats);
    }
}
