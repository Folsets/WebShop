using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using WebShop.IdentityServer.ViewModels;

namespace WebShop.IdentityServer.Pages
{
    public class Login : PageModel
    {
        [BindProperty] public LoginViewModel Form { get; set; }

        public void OnGet([FromQuery] string ReturnUrl)
        {
            Form = new LoginViewModel();
            Form.RedirectUrl = ReturnUrl;
        }

        public async Task<IActionResult> OnPostAsync(
            [FromServices] SignInManager<IdentityUser> signInManager)
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var result = await signInManager.PasswordSignInAsync(Form.Username, Form.Password, false, false);

            if (result.Succeeded)
            {
                return Redirect(Form.RedirectUrl);
            }

            return Page();
        }
    }
}
