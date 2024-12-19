using NotificationService.Application.Interfaces;
using NotificationService.Core.Entities;
using NotificationService.Core.Enums;
using NotificationService.Core.Events;
using NotificationService.Core.Interfaces;

namespace NotificationService.Infrastructure.Services;

public class NotificationService : INotificationService
{
    private readonly INotificationRepository _notificationRepository;
    private readonly IUserNotificationRepository _userNotificationRepository;
    private readonly IEmailService _emailService;
    private readonly IUserService _userService;

    public NotificationService(
        INotificationRepository notificationRepository,
        IUserNotificationRepository userNotificationRepository,
        IEmailService emailService,
        IUserService userService)
    {
        _notificationRepository = notificationRepository;
        _userNotificationRepository = userNotificationRepository;
        _emailService = emailService;
        _userService = userService;
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

        await SendNotificationAsync(notification);
    }

    public async Task HandleBookingCancelledAsync(BookingCancelledEvent bookingCancelledEvent)
    {
        var user = await _userService.GetUserByIdAsync(bookingCancelledEvent.UserId);
        if (user == null)
        {
            throw new Exception("User Not Found");
            return;
        }

        var notification = new Notification
        {
            Title = "Отмена бронирования",
            Message = $"Ваше бронирование с ID {bookingCancelledEvent.BookingId} было отменено. Причина: {bookingCancelledEvent.Reason}",
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

        await SendNotificationAsync(notification);
    }

    private async Task SendNotificationAsync(Notification notification)
    {
        if (notification.Type == NotificationType.Email)
        {
            await _emailService.SendEmailAsync(notification.Recipient, notification.Title, notification.Message);
        }

        notification.Status = NotificationStatus.Sent;
        notification.SentAt = DateTime.UtcNow;
        await _notificationRepository.UpdateAsync(notification);
    }
}