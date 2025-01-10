using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace NotificationService.Core.Entities;

public class User
{
    [BsonId]
    [BsonGuidRepresentation(GuidRepresentation.Standard)]
    public Guid Id { get; set; }
    public string Email { get; set; }
    public string? PhoneNumber { get; set; }
    public string? Name { get; set; }
    public string? Surname { get; set; }
}