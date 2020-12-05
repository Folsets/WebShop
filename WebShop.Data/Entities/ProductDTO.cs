using WebShop.Data.Enums;

namespace WebShop.Data.Entities
{
    public class ProductDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public CategoryEnum Category { get; set; }
        public string Characteristics { get; set; }
        public int Discount { get; set; }
        public string DiscountEnds { get; set; }
        public string Photos { get; set; }
    }
}
