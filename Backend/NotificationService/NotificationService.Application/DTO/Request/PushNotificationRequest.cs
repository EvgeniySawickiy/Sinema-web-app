namespace NotificationService.Application.DTO.Request;

public class PushNotificationRequest
{
    public string UserId { get; set; }
    public string Message { get; set; }
}