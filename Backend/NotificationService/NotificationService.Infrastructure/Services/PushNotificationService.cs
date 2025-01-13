using Microsoft.AspNetCore.SignalR;
using NotificationService.Application.Interfaces;
using NotificationService.Infrastructure.Hubs;

namespace NotificationService.Infrastructure.Services;

public class PushNotificationService : IPushNotificationService
{
    private readonly IHubContext<NotificationHub> _hubContext;

    public PushNotificationService(IHubContext<NotificationHub> hubContext)
    {
        _hubContext = hubContext;
    }

    public async Task SendToUserAsync(string userId, string message)
    {
        await _hubContext.Clients.User(userId).SendAsync("ReceiveNotification", message);
    }

    public async Task SendToGroupAsync(List<string> userIds, string message)
    {
        foreach (var userId in userIds)
        {
            await _hubContext.Clients.User(userId).SendAsync("ReceiveNotification", message);
        }
    }

    public async Task BroadcastAsync(string message)
    {
        await _hubContext.Clients.All.SendAsync("ReceiveNotification", message);
    }
}