using System.Linq;
using System.Threading.Tasks;
using IdentityModel;
using IdentityServer4.Extensions;
using IdentityServer4.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
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
        private readonly ILogger<AuthController> _logger;

        public AuthController(
            SignInManager<IdentityUser> signInManager,
            UserManager<IdentityUser> userManager,
            IClientRepository clientRepository,
            IIdentityServerInteractionService interactionService,
            ILogger<AuthController> logger
        )
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _interactionService = interactionService;
            _clientRepository = clientRepository;
            _logger = logger;
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
                    ViewBag.Message = "Имя пользователя или пароль неправильны.";
                    return View(vm);
                }
            }
            else
            {
                ViewBag.Message = "Имя пользователя или пароль неправильны.";
                return View(vm);
            }

            // Проверка на подтвержденный email
            if (await _userManager.IsEmailConfirmedAsync(user) == false)
            {
                ViewBag.Message = "Ваш Email не подтвержден.";
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
            // добавляем email для юзера
            user.Email = vm.Email;
            var result = await _userManager.CreateAsync(user, vm.Password);

            if (result.Succeeded)
            {
                var userId = (await _userManager.FindByNameAsync(vm.Username)).Id;
                await _clientRepository.Add(new Client
                {
                    Id = userId,
                    Address = "",
                    FirstName = "",
                    LastName = "",
                    PhoneNumber = ""
                });

                // var returnUrl = vm.ReturnUrl;
                // vm.ReturnUrl = null;
                // return Redirect(returnUrl);

                // Отправить письмо со ссылкой для подтверждения почты!
                var confirmationToken = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                var confirmationLink = Url.Action(nameof(EmailConfirmation).ToString(), "Auth", new
                {
                    userId = userId,
                    token = confirmationToken
                }, Request.Scheme);

                // Пока что просто делаем лог, TODO: создать сервис отправки писемь
                _logger.Log(LogLevel.Information, confirmationLink);

                ViewBag.ErrorTitle = "Подтвердите Email";
                ViewBag.Error = $"Ссылка для подтверждения вашей электроной почты отправлена на {user.Email}";
                return View("_Error");
            }

            return View(vm);
        }

        [HttpGet]
        public async Task<IActionResult> EmailConfirmation(string userId, string token)
        {
            if (userId == null || token == null)
            {
                ViewBag.ErrorTitle = "Ошибка подтверждения";
                ViewBag.Error = "Email не может быть подтвержден.";
                return View("_Error");
            }


            var user = await _userManager.FindByIdAsync(userId);

            if (await _userManager.IsEmailConfirmedAsync(user) == true)
            {
                ViewBag.Error = "Адрес электронной почты уже подтвержден";
                return View("_Error");
            }

            var result = await _userManager.ConfirmEmailAsync(user, token);

            if (result.Succeeded)
            {
                return View();
            }

            ViewBag.ErrorTitle = "Ошибка подтверждения";
            ViewBag.Error = "Email не может быть подтвержден.";
            return View("_Error");
        }
    }
}
