using System.ComponentModel.DataAnnotations;

namespace WebShop.IdentityServer.ViewModels
{
    public class LoginViewModel
    {
        public string RedirectUrl { get; set; }

        [Required]
        public string Username { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
