using Grpc.Core;
using NotificationService.Application.Interfaces;
using NotificationService.Protos;

namespace NotificationService.Infrastructure.Protos
{
    public class NotificationServiceGrpc : Notification.NotificationBase
    {
        private readonly IEmailService _emailService;

        public NotificationServiceGrpc(IEmailService emailService)
        {
            _emailService = emailService;
        }

        public override async Task<EmailResponse> SendEmail(EmailRequest request, ServerCallContext context)
        {
            if (string.IsNullOrEmpty(request.To))
            {
                return new EmailResponse
                {
                    Success = false,
                    Message = "Recipient email is required.",
                };
            }

            await _emailService.SendEmailAsync(
                    to: request.To,
                    subject: request.Subject,
                    body: request.Body,
                    isHtml: true);

            return new EmailResponse
            {
                Success = true,
                Message = "Email sent successfully.",
            };
        }
    }
}
