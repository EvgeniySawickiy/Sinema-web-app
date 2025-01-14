using NotificationService.Core.Events;

namespace NotificationService.Application.Interfaces;

public interface INotificationService
{
    Task HandleBookingCreatedAsync(BookingCreatedEvent bookingCreatedEvent);
    Task HandleBookingCancelledAsync(BookingCancelledEvent bookingCancelledEvent);
}