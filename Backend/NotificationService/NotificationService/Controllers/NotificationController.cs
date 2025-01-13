using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NotificationService.Application.DTO.Request;
using NotificationService.Application.Interfaces;
using NotificationService.Core.Entities;

namespace NotificationService.Controllers;

[ApiController]
[Route("api/[controller]")]
public class NotificationController(IEmailService emailService, IPushNotificationService pushNotificationService)
    : ControllerBase
{
    [HttpPost("send-email")]
    public async Task<IActionResult> SendEmail([FromBody] EmailNotificationRequest request)
    {
        await emailService.SendEmailAsync(request.Email, request.Subject, request.Message, true);
        return Ok(new { Message = "Email sent successfully." });
    }

    [HttpPost("send-emails")]
    public async Task<IActionResult> SendEmails([FromBody] BulkEmailNotificationRequest request)
    {
        foreach (var email in request.Emails)
        {
            await emailService.SendEmailAsync(email, request.Subject, request.Message, true);
        }

        return Ok(new { Message = "Emails sent successfully." });
    }

    [HttpPost("send-push")]
    public async Task<IActionResult> SendPushNotification([FromBody] PushNotificationRequest request)
    {
        await pushNotificationService.SendToUserAsync(request.UserId, request.Message);
        return Ok(new { Message = "Push notification sent successfully." });
    }

    [HttpPost("send-push-group")]
    public async Task<IActionResult> SendPushNotificationsToGroup([FromBody] GroupPushNotificationRequest request)
    {
        await pushNotificationService.SendToGroupAsync(request.UserIds, request.Message);
        return Ok(new { Message = "Push notifications sent successfully." });
    }

    [HttpPost("broadcast-push")]
    public async Task<IActionResult> BroadcastPushNotification([FromBody] BroadcastNotificationRequest request)
    {
        await pushNotificationService.BroadcastAsync(request.Message);
        return Ok(new { Message = "Broadcast notification sent successfully." });
    }
}