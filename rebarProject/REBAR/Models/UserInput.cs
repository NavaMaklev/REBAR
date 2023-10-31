namespace REBAR.Models
{
    public class UserInput
    {
        public string CustomerName { get; set; }
        public List<OrderedShake> Shakes { get; set; }
        public Discount Discount { get; set; }
        public Sale Sale { get; set; }
    }
}
