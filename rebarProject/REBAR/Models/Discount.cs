
namespace REBAR.Models
{
    public class Discount
    {
        public string Description { get; set; } // The name or description of the discount.
        public decimal AmountToReduced { get; set; } // The discount percentage.
        //This method calculates the discounted amount based on the original amount.
        public decimal CalculatePrice(decimal originalAmount)
        {
          var  finalPrice= originalAmount - AmountToReduced;
            if(finalPrice < 0) {return 0;}
            return finalPrice;
        }
    }
}