using NotificationService.Application.Interfaces;

namespace NotificationService.Infrastructure.Services;

public class EmailService : IEmailService
{
    public async Task SendEmailAsync(string recipientEmail, string subject, string message)
    {
        Console.WriteLine($"Отправка email на {recipientEmail}: {subject} - {message}");
        await Task.CompletedTask;
    }
}