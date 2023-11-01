namespace REBAR.Models
{
    public class OrderedShake
    {
        public string Shake { get; set; } // The ordered shake.
        public char Size { get; set; } // The size of the shake (L, M, S) and price.
        public int Quantity { get; set; }
    }
}
