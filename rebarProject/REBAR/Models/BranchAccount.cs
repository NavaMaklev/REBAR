using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Collections.Generic;
namespace REBAR.Models
{
    

    public class BranchAccount
    {
        [BsonId]
        [BsonRepresentation(BsonType.String)]
        public Guid BranchId { get; set; }
        public string ManagerPassword { get; set; }
        public List<Order> Orders { get; set; }
        //public decimal TotalAmount
        //{
        //    get
        //    {
        //        if (Orders == null || Orders.Count == 0) return 0;
        //        decimal total = 0;
        //        foreach (var order in Orders)
        //        {
        //            total += order.TotalPrice(); // הנחה שיש לך שדה בשם TotalPrice במחלקה Order
        //        }
        //        return total;
        //    }
        //}
    }

}
