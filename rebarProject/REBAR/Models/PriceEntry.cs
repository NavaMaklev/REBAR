using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace REBAR.Models
{
    public class PriceEntry
    {
        [BsonId]
        [BsonRepresentation(BsonType.String)]
        public Guid Id { get; set; }
        public char Size { get; set; }//  S/M/L
        public decimal Price { get; set; }
        public bool IsSpecial { get; set; }
    }
}
