using BookingService.Core.Entities;

namespace BookingService.Application.DTO
{
    public class CreateBookingRequestDTO
    {
        public Guid UserId { get; set; }
        public Guid ShowtimeId { get; set; }
        public List<Seat> Seats { get; set; } = new();
        public decimal TotalAmount { get; set; }
    }
}
