using WebShop.Data.Enums;

namespace WebShop.Data.Entities
{
    public class Order
    {
        public int Id { get; set; }
        public string CustomerId { get; set; }
        public string ClientId { get; set; }
        public string City { get; set; }
        public int DeliveryId { get; set; }
        public string Address { get; set; }
        public OrderStatus Status { get; set; }
    }
}
