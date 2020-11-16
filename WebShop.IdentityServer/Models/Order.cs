using System.ComponentModel.DataAnnotations;

namespace WebShop.IdentityServer.ViewModels
{
    public class Order
    {
        [Key]
        public int Id { get; set; }

        public string EmployeeId { get; set; }

        public string ClientId { get; set; }

        public string City { get; set; }

        public string Index { get; set; }

        public string Address { get; set; }
    }
}
