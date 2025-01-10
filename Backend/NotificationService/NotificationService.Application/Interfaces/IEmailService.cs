using NotificationService.Core.Entities;

namespace NotificationService.Application.Interfaces;

public interface IEmailService
{
    Task SendEmailAsync(string to, string subject, string body, bool isHtml = false);
}