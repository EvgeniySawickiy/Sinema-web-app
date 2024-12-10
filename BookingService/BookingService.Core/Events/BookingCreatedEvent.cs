
namespace BookingService.Core.Events
{
    public class BookingCreatedEvent
    {
        public Guid BookingId { get; set; }
        public Guid UserId { get; set; }
        public decimal TotalAmount { get; set; }
    }
}
