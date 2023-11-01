using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System.ComponentModel.DataAnnotations;

namespace REBAR.Models
{ 
 public class Order
    {
        [BsonId]
        [BsonRepresentation(BsonType.String)]
        public Guid Id { get; set; }
        [Required]
        public string CustomerName { get; set; } // The name of the customer.
        public DateTime Date { get; set; } // The date the order was placed.
        public DateTime CompletionTime { get; set; }
        [Required]
        [MaxLength(10)]
        public List <string> ShakesId { get; set; } // List of shakes ordered.
        public decimal TotalPrice { get; set; }
        public decimal FinalPrice { get; set; }
        public Discount Discounts { get; set; } // Discounts or offers applied to this order.
        public Sale Sale { get; set; }
        // Calculates the total price of the order considering discounts.                                            
        //public decimal TotalPrice
        //{
        //    get
        //    {
        //        decimal total = Shakes.Sum(s => s.PriceAndSize.Price); // Sum of shake prices.
        //        decimal totalDiscount = Discounts.Sum(d => d.Amount); // Sum of discounts.
        //        return total - totalDiscount;
        //    }
        //}
    }
}

