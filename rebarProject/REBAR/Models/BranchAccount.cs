using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using REBAR.Services;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace REBAR.Models
{
    public class BranchAccount
    {
        [BsonId]
        [BsonRepresentation(BsonType.String)]
        public Guid BranchId { get; set; }
        [Required]
        public string ManagerPassword { get; set; }
        public List<Order> Orders { get; set; }
        public decimal TotalAmount { get; set; }
        public List<DailyReport> Reports { get; set; }
    }

}
