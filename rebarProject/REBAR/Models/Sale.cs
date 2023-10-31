namespace REBAR.Models
{
    public class Sale
    {
        public string Description { get; set; } // The name or description of the discount.
        public decimal PercentagesReduction { get; set; } // The discount .
        public decimal CalculatePrice(decimal originalAmount)
        {
           var finalPrice= originalAmount * (1 - (decimal)PercentagesReduction / 100);
            if(finalPrice < 0) { return 0; }
            return finalPrice;
        }
    }
}
