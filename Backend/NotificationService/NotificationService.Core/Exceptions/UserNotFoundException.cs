namespace NotificationService.Core.Exceptions;

public class UserNotFoundException(Guid userId) : Exception($"User with id: {userId} not found");
