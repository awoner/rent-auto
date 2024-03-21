using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace RentAutoPoc.Api.Entities;

public class User
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }
    
    public string Name { get; set; }
    
    public int BirthYear { get; set; }
}