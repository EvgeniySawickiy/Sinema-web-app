namespace NotificationService.Application.DTO.Request;

public class BulkEmailNotificationRequest
{
    public List<string> Emails { get; set; }
    public string Subject { get; set; }
    public string Message { get; set; }
}