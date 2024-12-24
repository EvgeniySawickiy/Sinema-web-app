using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace NotificationService.Core.Entities
{
    public class NotificationLog
    {
        [BsonId]
        [BsonGuidRepresentation(GuidRepresentation.Standard)]
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Recipient { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;
        public string Status { get; set; } = "Sent";
        public string ErrorMessage { get; set; } = string.Empty;
    }
}
