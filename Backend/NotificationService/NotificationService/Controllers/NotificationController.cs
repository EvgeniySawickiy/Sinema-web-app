using Microsoft.AspNetCore.Mvc;
using NotificationService.Application.DTO.Request;
using NotificationService.Application.Interfaces;

namespace NotificationService.Controllers;

[ApiController]
[Route("api/notifications")]
public class NotificationController(
    IEmailService emailService,
    IPushNotificationService pushNotificationService,
    ILogger<NotificationController> logger)
    : ControllerBase
{
    [HttpPost("email")]
    public async Task<IActionResult> SendEmail([FromBody] EmailNotificationRequest request)
    {
        logger.LogInformation("Sending email to {Email}", request.Email);

        await emailService.SendEmailAsync(request.Email, request.Subject, request.Message, true);

        logger.LogInformation("Email sent successfully to {Email}", request.Email);

        return Ok(new { Message = "Email sent successfully." });
    }

    [HttpPost("emails")]
    public async Task<IActionResult> SendEmails([FromBody] BulkEmailNotificationRequest request)
    {
        logger.LogInformation("Sending emails to multiple recipients.");

        var tasks = request.Emails.Select(email =>
        {
            logger.LogInformation("Sending email to {Email}", email);
            return emailService.SendEmailAsync(email, request.Subject, request.Message, true);
        });

        await Task.WhenAll(tasks);

        logger.LogInformation("All emails sent successfully.");

        return Ok(new { Message = "Emails sent successfully." });
    }

    [HttpPost("push")]
    public async Task<IActionResult> SendPushNotification([FromBody] PushNotificationRequest request)
    {
        logger.LogInformation("Sending push notification to user {UserId}", request.UserId);

        await pushNotificationService.SendToUserAsync(request.UserId, request.Message);

        logger.LogInformation("Push notification sent successfully to user {UserId}", request.UserId);

        return Ok(new { Message = "Push notification sent successfully." });
    }

    [HttpPost("push/group")]
    public async Task<IActionResult> SendPushNotificationsToGroup([FromBody] GroupPushNotificationRequest request)
    {
        logger.LogInformation("Sending push notifications to group of users.");

        await pushNotificationService.SendToGroupAsync(request.UserIds, request.Message);

        logger.LogInformation("Push notifications sent successfully to group.");

        return Ok(new { Message = "Push notifications sent successfully." });
    }

    [HttpPost("push/broadcast")]
    public async Task<IActionResult> BroadcastPushNotification([FromBody] BroadcastNotificationRequest request)
    {
        logger.LogInformation("Broadcasting push notification to all users.");

        await pushNotificationService.BroadcastAsync(request.Message);

        logger.LogInformation("Broadcast notification sent successfully.");

        return Ok(new { Message = "Broadcast notification sent successfully." });
    }
}