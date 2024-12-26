namespace BookingService.Core.Exceptions;

public class EmptyUserBookingListException(Guid userId) : Exception($"User {userId} don't have any bookings!.");
