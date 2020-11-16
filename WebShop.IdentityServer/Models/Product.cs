using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WebShop.IdentityServer.ViewModels
{
    public class Product
    {
        [Key] public int Id { get; set; }

        public decimal Price { get; set; }

        public string CategoryId { get; set; }
    }
}
