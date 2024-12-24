using Microsoft.AspNetCore.SignalR;
using NotificationService.Application.Interfaces;
using NotificationService.Core.Entities;
using NotificationService.Core.Enums;
using NotificationService.Core.Events;
using NotificationService.Core.Interfaces;
using NotificationService.Infrastructure.Hubs;

namespace NotificationService.Infrastructure.Services;

public class NotificationService : INotificationService
{
    private readonly INotificationRepository _notificationRepository;
    private readonly IUserNotificationRepository _userNotificationRepository;
    private readonly IHubContext<NotificationHub> _hubContext;
    private readonly IEmailService _emailService;
    private readonly IUserService _userService;

    public NotificationService(
        INotificationRepository notificationRepository,
        IUserNotificationRepository userNotificationRepository,
        IEmailService emailService,
        IUserService userService,
        IHubContext<NotificationHub> hubContext)
    {
        _notificationRepository = notificationRepository;
        _userNotificationRepository = userNotificationRepository;
        _emailService = emailService;
        _userService = userService;
        _hubContext = hubContext;
    }

    public async Task HandleBookingCreatedAsync(BookingCreatedEvent bookingCreatedEvent)
    {
        var user = await _userService.GetUserByIdAsync(bookingCreatedEvent.UserId);
        if (user == null)
        {
            throw new Exception($"User {bookingCreatedEvent.UserId} not found");
        }

        var notification = new Notification
        {
            Title = "Подтверждение бронирования",
            Message = $"Ваше бронирование с ID {bookingCreatedEvent.BookingId} успешно создано.",
            Recipient = user.Email,
            Type = NotificationType.Email,
            CreatedAt = DateTime.UtcNow,
            Status = NotificationStatus.Pending,
        };

        await _notificationRepository.AddAsync(notification);

        var userNotification = new UserNotification
        {
            UserId = bookingCreatedEvent.UserId,
            NotificationId = notification.Id,
            Notification = notification,
            IsRead = false,
            CreatedAt = DateTime.UtcNow,
        };

        await _userNotificationRepository.AddUserNotificationAsync(userNotification);

        await SendNotificationAsync(notification, user.Id);
    }

    public async Task HandleBookingCancelledAsync(BookingCancelledEvent bookingCancelledEvent)
    {
        var user = await _userService.GetUserByIdAsync(bookingCancelledEvent.UserId);
        if (user == null)
        {
            throw new Exception("User Not Found");
        }

        var notification = new Notification
        {
            Title = "Отмена бронирования",
            Message =
                $"Ваше бронирование с ID {bookingCancelledEvent.BookingId} было отменено. Причина: {bookingCancelledEvent.Reason}",
            Recipient = user.Email,
            Type = NotificationType.Email,
            CreatedAt = DateTime.UtcNow,
            Status = NotificationStatus.Pending,
        };

        await _notificationRepository.AddAsync(notification);

        var userNotification = new UserNotification
        {
            UserId = bookingCancelledEvent.UserId,
            NotificationId = notification.Id,
            Notification = notification,
            IsRead = false,
            CreatedAt = DateTime.UtcNow,
        };

        await _userNotificationRepository.AddUserNotificationAsync(userNotification);

        await SendNotificationAsync(notification, user.Id);
    }

    private async Task SendNotificationAsync(Notification notification, Guid userId)
    {
        try
        {
            if (notification.Type == NotificationType.Email)
            {
                await _emailService.SendEmailAsync(
                    to: notification.Recipient,
                    subject: notification.Title,
                    body: notification.Message,
                    isHtml: true);
            }

            await _hubContext.Clients.User(userId.ToString()).SendAsync("ReceiveNotification", new
            {
                Title = notification.Title,
                Message = notification.Message,
                CreatedAt = notification.CreatedAt,
            });

            notification.Status = NotificationStatus.Sent;
            notification.SentAt = DateTime.UtcNow;

            await _notificationRepository.UpdateAsync(notification);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error sending notification: {ex.Message}");

            notification.Status = NotificationStatus.Failed;

            await _notificationRepository.UpdateAsync(notification);
        }
    }
}