using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace SharpHub.Models
{
    public class SSHKey : DB_SaveableObject
    {
        [BsonId]
        public ObjectId UserID { get; set; }

        public string PublicKey { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public bool Revoked { get; set; } = false;
    }
}
