using BookingService.Core.Enums;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace BookingService.Core.Entities
{
    public class Booking
    {
        [BsonId]
        [BsonGuidRepresentation(GuidRepresentation.Standard)]
        public Guid Id { get; set; }

        [BsonGuidRepresentation(GuidRepresentation.Standard)]
        public Guid UserId { get; set; }

        [BsonGuidRepresentation(GuidRepresentation.Standard)]
        public Guid ShowtimeId { get; set; }
        public DateTime BookingTime { get; set; }
        public BookingStatus Status { get; set; }
        public PaymentStatus PaymentStatus { get; set; }
        public decimal TotalAmount { get; set; }
        public List<Seat> Seats { get; set; } = new();
    }
}
