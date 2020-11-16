using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace WebShop.IdentityServer.ViewModels
{
    public class Basket
    {
        [Key] public int Id { get; set; }

        public int OrderId { get; set; }

        public int ProductId { get; set; }

        public decimal Cost { get; set; }

        public int Quantity { get; set; }
    }
}
