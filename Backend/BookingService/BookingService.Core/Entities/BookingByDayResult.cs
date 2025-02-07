using MongoDB.Bson.Serialization.Attributes;

namespace BookingService.Core.Entities;

public class BookingByDayResult
{
    [BsonElement("_id")]
    public string Id { get; set; } = null!;

    [BsonElement("count")]
    public int Count { get; set; }
}