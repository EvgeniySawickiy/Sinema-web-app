namespace NotificationService.Application.DTO.Request;

public class GroupPushNotificationRequest
{
    public List<string> UserIds { get; set; }
    public string Message { get; set; }
}