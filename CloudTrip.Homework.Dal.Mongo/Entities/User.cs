using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace CloudTrip.Homework.Dal.Mongo.Entities;

internal sealed class User
{
    [BsonRepresentation(BsonType.ObjectId)]
    public ObjectId Id { get; set; }
    public required string Email { get; set; }
    public required string PasswordHash { get; set; }
    public required DateTime CreatedAt { get; set; }
}
