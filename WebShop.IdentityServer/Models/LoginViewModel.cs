using System.ComponentModel.DataAnnotations;

namespace WebShop.IdentityServer.ViewModels
{
    public class LoginViewModel
    {
        public bool Failed { get; set; } = false;

        public string ReturnUrl { get; set; }

        [Required]
        public string Username { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
