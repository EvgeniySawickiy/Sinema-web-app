namespace BookingService.Core.Events
{
    public class BookingCreatedEvent
    {
        public Guid BookingId { get; set; }
        public Guid UserId { get; set; }
        public Guid ShowtimeId { get; set; }
        public List<(int, int)> SeatNumbers { get; set; }
        public decimal TotalPrice { get; set; }
        public DateTime CreatedAt { get; set; }
        public string ShowtimeDateTime { get; set; }
        public string MovieTitle { get; set; }
    }
}
