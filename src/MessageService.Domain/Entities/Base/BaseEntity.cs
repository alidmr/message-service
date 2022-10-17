using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace MessageService.Domain.Entities.Base
{
    public class BaseEntity : IEntity<string>
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; } = ObjectId.GenerateNewId().ToString();
    }
}