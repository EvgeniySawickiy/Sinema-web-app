namespace NotificationService.Core.Exceptions;

public class SendingEmailErrorException(Exception exception) : Exception($"Error sending notification: {exception.Message}");