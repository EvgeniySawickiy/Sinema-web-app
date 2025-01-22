using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace BookingService.Core.Entities
{
    public class Seat
    {
        [BsonId]
        [BsonGuidRepresentation(GuidRepresentation.Standard)]
        public Guid Id { get; set; }
        public int Row { get; set; }
        public int Number { get; set; }
        [BsonGuidRepresentation(GuidRepresentation.Standard)]
        public Guid HallId { get; set; }
        [BsonGuidRepresentation(GuidRepresentation.Standard)]
        public Guid ShowTimeId { get; set; }
        public bool IsReserved { get; set; }
    }
}
