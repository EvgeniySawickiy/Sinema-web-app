namespace BookingService.Core.Exceptions;

public class UserNotFoundException(Guid userId) : Exception($"User with ID {userId} was not found.");
