using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace REBAR.Models
{
    public class Payment
    {
        [BsonId]
        [BsonRepresentation(BsonType.String)]
        public Guid Id { get; set; }
        public string OrderId { get; set; }
        public DateTime PaymentDate { get; set; }
        public decimal PaymentAmount { get; set; }
    }

}
