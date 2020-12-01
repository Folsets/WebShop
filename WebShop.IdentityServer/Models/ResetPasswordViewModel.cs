using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;
using IdentityServer4.Models;

namespace WebShop.IdentityServer.ViewModels
{
    public class ResetPasswordViewModel
    {
        [Required] public string ReturnUrl { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        public string Token { get; set; }
        [Required(ErrorMessage = "Поле необходимо заполнить")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Required(ErrorMessage = "Поле необходимо заполнить")]
        [Compare("Password", ErrorMessage = "Пароли не совпадают")]
        public string ConfirmPassword { get; set; }
    }
}
