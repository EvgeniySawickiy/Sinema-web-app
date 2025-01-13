namespace NotificationService.Application.Interfaces;

public interface IPushNotificationService
{
    Task SendToUserAsync(string userId, string message);
    Task SendToGroupAsync(List<string> userIds, string message);
    Task BroadcastAsync(string message);
}