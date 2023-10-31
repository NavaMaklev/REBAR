using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace REBAR.Models
{
    public class Shake
    {
        [BsonId]
        [BsonRepresentation(BsonType.String)]
        public Guid Id { get; set; }// Shake's unique ID in MongoDB
        /* private int _uniqueId; // This will hold the unique id for each instance*/
        public string Name { get; set; } // Shake name
        public string Description { get; set; } // Short description
        public bool IsSpecial { get; set; }=false;
        //public int UniqueId// Unique identifier for the shake
        //{
        //    get { return _uniqueId; }
        //    private set { _uniqueId = ++_currentId; } 
        //} 
    }
}
