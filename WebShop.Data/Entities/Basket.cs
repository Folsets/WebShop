namespace WebShop.Data.Entities
{
    public class Basket
    {
        public int OrderId { get; set; }
        public string ProductId { get; set; }
        public decimal Cost { get; set; }
        public int Quantity { get; set; }
    }
}
