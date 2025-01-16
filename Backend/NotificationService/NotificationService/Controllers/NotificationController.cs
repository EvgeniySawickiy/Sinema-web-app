﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NotificationService.Application.DTO.Request;
using NotificationService.Application.Interfaces;
using NotificationService.Core.Entities;

namespace NotificationService.Controllers;

[ApiController]
[Route("api/[controller]")]
public class NotificationController : ControllerBase
{
    private readonly IEmailService _emailService;
    private readonly IPushNotificationService _pushNotificationService;
    private readonly ILogger<NotificationController> _logger;

    public NotificationController(
        IEmailService emailService,
        IPushNotificationService pushNotificationService,
        ILogger<NotificationController> logger)
    {
        _emailService = emailService;
        _pushNotificationService = pushNotificationService;
        _logger = logger;
    }

    [HttpPost("send-email")]
    public async Task<IActionResult> SendEmail([FromBody] EmailNotificationRequest request)
    {
        _logger.LogInformation("Sending email to {Email}", request.Email);
        await _emailService.SendEmailAsync(request.Email, request.Subject, request.Message, true);
        _logger.LogInformation("Email sent successfully to {Email}", request.Email);
        return Ok(new { Message = "Email sent successfully." });
    }

    [HttpPost("send-emails")]
    public async Task<IActionResult> SendEmails([FromBody] BulkEmailNotificationRequest request)
    {
        _logger.LogInformation("Sending emails to multiple recipients.");
        var tasks = request.Emails.Select(email =>
        {
            _logger.LogInformation("Sending email to {Email}", email);
            return _emailService.SendEmailAsync(email, request.Subject, request.Message, true);
        });

        await Task.WhenAll(tasks);

        _logger.LogInformation("All emails sent successfully.");
        return Ok(new { Message = "Emails sent successfully." });
    }

    [HttpPost("send-push")]
    public async Task<IActionResult> SendPushNotification([FromBody] PushNotificationRequest request)
    {
        _logger.LogInformation("Sending push notification to user {UserId}", request.UserId);
        await _pushNotificationService.SendToUserAsync(request.UserId, request.Message);
        _logger.LogInformation("Push notification sent successfully to user {UserId}", request.UserId);
        return Ok(new { Message = "Push notification sent successfully." });
    }

    [HttpPost("send-push-group")]
    public async Task<IActionResult> SendPushNotificationsToGroup([FromBody] GroupPushNotificationRequest request)
    {
        _logger.LogInformation("Sending push notifications to group of users.");
        await _pushNotificationService.SendToGroupAsync(request.UserIds, request.Message);
        _logger.LogInformation("Push notifications sent successfully to group.");
        return Ok(new { Message = "Push notifications sent successfully." });
    }

    [HttpPost("broadcast-push")]
    public async Task<IActionResult> BroadcastPushNotification([FromBody] BroadcastNotificationRequest request)
    {
        _logger.LogInformation("Broadcasting push notification to all users.");
        await _pushNotificationService.BroadcastAsync(request.Message);
        _logger.LogInformation("Broadcast notification sent successfully.");
        return Ok(new { Message = "Broadcast notification sent successfully." });
    }
}