using System.ComponentModel.DataAnnotations;

namespace WebShop.IdentityServer.ViewModels
{
    public class RegisterViewModel
    {
        public string ReturnUrl { get; set; }

        [Required(ErrorMessage = "Это обязательное поле")]
        public string Username { get; set; }

        [EmailAddress(ErrorMessage = "Введите корректный e-mail")]
        public string Email { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        [Required(ErrorMessage = "Это обязательное поле")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Это обязательное поле")]
        [Compare("Password", ErrorMessage = "Пароли не совпадают")]
        public string ConfirmPassword { get; set; }
    }
}
