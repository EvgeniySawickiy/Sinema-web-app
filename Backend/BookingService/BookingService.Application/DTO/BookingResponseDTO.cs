using BookingService.Core.Entities;
using BookingService.Core.Enums;

namespace BookingService.Application.DTO
{
    public class BookingResponseDTO
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public Guid ShowtimeId { get; set; }
        public DateTime BookingTime { get; set; }
        public BookingStatus Status { get; set; }
        public PaymentStatus PaymentStatus { get; set; }
        public decimal TotalAmount { get; set; }
        public List<Seat> Seats { get; set; } = new();
    }
}
