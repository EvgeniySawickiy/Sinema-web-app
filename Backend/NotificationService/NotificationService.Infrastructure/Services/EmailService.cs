using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Configuration;
using MimeKit;
using NotificationService.Application.Interfaces;
using NotificationService.Core.Entities;

namespace NotificationService.Infrastructure.Services;

public class EmailService : IEmailService
{
    private readonly EmailSettings _emailSettings;

    public EmailService(EmailSettings emailSettings)
    {
        _emailSettings = emailSettings;
    }

    public async Task SendEmailAsync(string to, string subject, string body, bool isHtml = false)
    {
        var email = new MimeMessage();
        email.From.Add(new MailboxAddress(_emailSettings.DisplayName, _emailSettings.From));
        email.To.Add(MailboxAddress.Parse(to));
        email.Subject = subject;

        if (isHtml)
        {
            email.Body = new TextPart("html") { Text = body };
        }
        else
        {
            email.Body = new TextPart("plain") { Text = body };
        }

        using var smtp = new SmtpClient();
        try
        {
            await smtp.ConnectAsync(_emailSettings.SmtpServer, _emailSettings.Port, _emailSettings.UseSsl);
            await smtp.AuthenticateAsync(_emailSettings.Username, _emailSettings.Password);
            await smtp.SendAsync(email);
        }
        catch (Exception ex)
        {
            throw new Exception($"Error sending email: {ex.Message}");
        }
        finally
        {
            await smtp.DisconnectAsync(true);
        }
    }
}