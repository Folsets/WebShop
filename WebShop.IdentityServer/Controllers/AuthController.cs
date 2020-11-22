using System.Linq;
using System.Threading.Tasks;
using IdentityModel;
using IdentityServer4.Extensions;
using IdentityServer4.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WebShop.Data.Entities;
using WebShop.Data.Interfaces;
using WebShop.IdentityServer.ViewModels;

namespace WebShop.IdentityServer.Controllers
{
    public class AuthController : Controller
    {
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IClientRepository _clientRepository;
        private readonly IIdentityServerInteractionService _interactionService;

        public AuthController(
            SignInManager<IdentityUser> signInManager,
            UserManager<IdentityUser> userManager,
            IClientRepository clientRepository,
            IIdentityServerInteractionService interactionService
        )
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _interactionService = interactionService;
            _clientRepository = clientRepository;
        }

        [HttpGet]
        public async Task<IActionResult> Logout(string logoutId)
        {
            await _signInManager.SignOutAsync();

            var logoutRequest = await _interactionService.GetLogoutContextAsync(logoutId);

            if (string.IsNullOrEmpty(logoutRequest.PostLogoutRedirectUri))
            {
                return RedirectToAction("Index", "Home");
            }

            return Redirect(logoutRequest.PostLogoutRedirectUri);
        }

        [HttpGet]
        public async Task<IActionResult> Login([FromQuery]string returnUrl)
        {
            return View(new LoginViewModel
            {
                ReturnUrl = returnUrl
            });
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel vm)
        {
            if (ModelState.IsValid == false)
            {
                return View(vm);
            }

            var user = await _userManager.FindByNameAsync(vm.Username);
            if (user != null)
            {
                if (await _signInManager.UserManager.CheckPasswordAsync(user, vm.Password) == false)
                {
                    vm.Failed = true;
                    return View(vm);
                }
            }
            else
            {
                vm.Failed = true;
                return View(vm);
            }


            var result = await _signInManager.PasswordSignInAsync(vm.Username, vm.Password, false, false);
            if (result.Succeeded)
            {
                return Redirect(vm.ReturnUrl);
            }

            return View(vm);
        }

        [HttpGet]
        public IActionResult Register([FromQuery]string returnUrl)
        {
            return View(new RegisterViewModel
            {
                ReturnUrl = returnUrl
            });
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel vm)
        {
            if (ModelState.IsValid == false)
            {
                return View(vm);
            }

            var user = new IdentityUser(vm.Username);
            var result = await _userManager.CreateAsync(user, vm.Password);

            if (result.Succeeded)
            {
                await _signInManager.PasswordSignInAsync(user, vm.Password, false, false);
                var userId = (await _userManager.FindByNameAsync(vm.Username)).Id;
                await _clientRepository.Add(new Client
                {
                    Id = userId,
                    Address = "",
                    FirstName = "",
                    LastName = "",
                    PhoneNumber = ""
                });

                var returnUrl = vm.ReturnUrl;
                vm.ReturnUrl = null;
                return Redirect(returnUrl);
            }

            return View(vm);
        }
    }
}
