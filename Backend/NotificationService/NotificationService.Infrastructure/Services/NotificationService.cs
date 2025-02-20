using Hangfire;
using NotificationService.Application.Interfaces;
using NotificationService.Core.Entities;
using NotificationService.Core.Enums;
using NotificationService.Core.Events;
using NotificationService.Core.Exceptions;
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

    private static readonly Dictionary<Guid, List<string>> _scheduledJobs = new();

    public async void ScheduleEmailReminders(BookingCreatedEvent bookingEvent)
    {
        var user = await _userService.GetUserByIdAsync(bookingEvent.UserId);
        if (user == null)
        {
            throw new UserNotFoundException(bookingEvent.UserId);
        }

        var reminder1 = DateTime.Parse(bookingEvent.ShowtimeDateTime).AddDays(-1);
        var reminder2 = DateTime.Parse(bookingEvent.ShowtimeDateTime).AddHours(-1);

        var jobIds = new List<string>();

        if (reminder1 > DateTime.UtcNow)
        {
            var notification = new Notification
            {
                Title = "Напоминание о киносеансе",
                Message =
                    $"Ваш сеанс на фильм {bookingEvent.MovieTitle} состоится завтра {bookingEvent.ShowtimeDateTime}.",
                Recipient = user.Email,
                Type = NotificationType.Email,
                CreatedAt = DateTime.UtcNow,
                Status = NotificationStatus.Pending,
            };

            string jobId1 = BackgroundJob.Schedule(
                () => SendNotificationAsync(notification),
                reminder1 - DateTime.UtcNow);

            jobIds.Add(jobId1);
        }

        if (reminder2 > DateTime.UtcNow)
        {
            var notification = new Notification
            {
                Title = "Напоминание о киносеансе",
                Message =
                    $"Ваш сеанс на фильм {bookingEvent.MovieTitle} состоится через один час в {bookingEvent.ShowtimeDateTime:HH:mm}.",
                Recipient = user.Email,
                Type = NotificationType.Email,
                CreatedAt = DateTime.UtcNow,
                Status = NotificationStatus.Pending,
            };

            string jobId2 = BackgroundJob.Schedule(
                () => SendNotificationAsync(notification),
                reminder2 - DateTime.UtcNow);

            jobIds.Add(jobId2);
        }

        if (jobIds.Count > 0)
        {
            _scheduledJobs[bookingEvent.BookingId] = jobIds;
        }
    }

    public void DeleteScheduleEmailReminders(BookingCancelledEvent bookingCancelledEvent)
    {
        if (_scheduledJobs.TryGetValue(bookingCancelledEvent.BookingId, out var jobIds))
        {
            foreach (var jobId in jobIds)
            {
                BackgroundJob.Delete(jobId);
            }

            _scheduledJobs.Remove(bookingCancelledEvent.BookingId);
        }
    }

    public async Task HandleBookingCreatedAsync(BookingCreatedEvent bookingCreatedEvent)
    {
        var user = await _userService.GetUserByIdAsync(bookingCreatedEvent.UserId);
        if (user == null)
        {
            throw new UserNotFoundException(bookingCreatedEvent.UserId);
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
            throw new UserNotFoundException(bookingCancelledEvent.UserId);
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
            UserId = user.Id,
            NotificationId = notification.Id,
            Notification = notification,
            IsRead = false,
            CreatedAt = DateTime.UtcNow,
        };

        await _userNotificationRepository.AddUserNotificationAsync(userNotification);

        await SendNotificationAsync(notification);
    }

    public async Task SendNotificationAsync(Notification notification)
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

            notification.Status = NotificationStatus.Sent;
            notification.SentAt = DateTime.UtcNow;

            await _notificationRepository.UpdateAsync(notification);
        }
        catch (Exception ex)
        {
            notification.Status = NotificationStatus.Failed;

            await _notificationRepository.UpdateAsync(notification);

            throw new SendingEmailErrorException(ex);
        }
    }
}