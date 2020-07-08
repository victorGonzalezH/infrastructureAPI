using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

public class Document
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; }
}