

namespace BookingService.Core.Events
{
    public class BookingCancelledEvent
    {
        public Guid BookingId { get; set; }
        public string Reason { get; set; }
    }
}
