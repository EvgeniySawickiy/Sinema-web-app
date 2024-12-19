namespace BookingService.Core.Exceptions;

public class InvalidShowtimeException(Guid showtimeId) : Exception($"Showtime {showtimeId} is invalid or inactive.");