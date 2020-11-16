using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WebShop.IdentityServer.ViewModels;

namespace WebShop.IdentityServer.Pages
{
    public class Register : PageModel
    {
        [BindProperty] public RegisterViewModel Form { get; set; }

        public void OnGet()
        {
            Form = new RegisterViewModel();
        }

        public async Task<IActionResult> OnPostAsync(
            [FromServices] SignInManager<IdentityUser> signInManager,
            [FromServices] UserManager<IdentityUser> userManager)
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            if (userManager.FindByNameAsync(Form.Username) != null)
            {
                // Пользователь с таким именем уже существует
                return Page();
            }

            if (userManager.FindByEmailAsync(Form.Email) != null)
            {
                // Пользователь с таким адресом эл. почты уже зарегистрирован
                return Page();
            }

            var user = new IdentityUser(Form.Username);
            var result = await userManager.CreateAsync(user, Form.Password);

            if (result.Succeeded)
            {
                return Redirect("TODO: Email confirmation url");
            }

            return Page();
        }
    }
}
