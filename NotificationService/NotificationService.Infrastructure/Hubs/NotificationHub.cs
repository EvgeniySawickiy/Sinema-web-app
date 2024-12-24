using Microsoft.AspNetCore.SignalR;

namespace NotificationService.Infrastructure.Hubs;

public class NotificationHub : Hub
{
    public async Task SendNotification(string userId, string message)
    {
        Console.WriteLine($"Sending notification: {Context.ConnectionId}");

        await Clients.User(userId).SendAsync("ReceiveNotification", message);
    }

    public async Task BroadcastNotification(string message)
    {
        await Clients.All.SendAsync("ReceiveNotification", message);
    }
}