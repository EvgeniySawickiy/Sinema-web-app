namespace BookingService.Application.DTO
{
    public class BookingDTO
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public Guid ShowtimeId { get; set; }
        public decimal TotalAmount { get; set; }
        public List<SeatDTO> Seats { get; set; } = new();
    }
}
