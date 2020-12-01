using System.ComponentModel.DataAnnotations;

namespace WebShop.IdentityServer.ViewModels
{
    public class ForgotPasswordViewModel
    {
        [Required] public string ReturnUrl { get; set; }
        [Required(ErrorMessage = "Это поле обязательно")]
        [EmailAddress(ErrorMessage = "Введите настоящий Email")]
        public string Email { get; set; }
    }
}
