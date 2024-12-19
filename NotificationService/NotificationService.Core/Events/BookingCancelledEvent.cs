namespace NotificationService.Core.Events;

public class BookingCancelledEvent
{
    public Guid BookingId { get; set; }
    public Guid UserId { get; set; }
    public Guid ShowtimeId { get; set; }
    public List<(int, int)> SeatNumbers { get; set; }
    public DateTime CancelledAt { get; set; }
    public string Reason { get; set; }
}