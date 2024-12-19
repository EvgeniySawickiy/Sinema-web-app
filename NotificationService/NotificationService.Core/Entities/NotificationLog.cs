
namespace NotificationService.Core.Entities
{
    public class NotificationLog
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Recipient { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;
        public string Status { get; set; } = "Sent";
        public string ErrorMessage { get; set; } = string.Empty;
    }
}
