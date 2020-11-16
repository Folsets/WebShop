using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace WebShop.IdentityServer.ViewModels
{
    public class Delivery
    {
        [Key] public int Id  { get; set; }

        public string Name { get; set; }

        public string Phone { get; set; }
    }
}
