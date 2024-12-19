namespace BookingService.Core.Exceptions
{
    public class BookingNotFoundException(Guid bookingId) : Exception($"Booking with ID {bookingId} was not found.");
}
