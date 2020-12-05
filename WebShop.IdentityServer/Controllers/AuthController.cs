using System.Linq;
using System.Security.Policy;
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
using EmailService;
using Org.BouncyCastle.Asn1.X509;

namespace WebShop.IdentityServer.Controllers
{
    public class AuthController : Controller
    {
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IClientRepository _clientRepository;
        private readonly IIdentityServerInteractionService _interactionService;
        private readonly ILogger<AuthController> _logger;
        private readonly IMailService _emailService;

        public AuthController(
            SignInManager<IdentityUser> signInManager,
            UserManager<IdentityUser> userManager,
            IClientRepository clientRepository,
            IIdentityServerInteractionService interactionService,
            IMailService emailService,
            ILogger<AuthController> logger
        )
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _interactionService = interactionService;
            _clientRepository = clientRepository;
            _emailService = emailService;
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
        public async Task<IActionResult> Login([FromQuery] string returnUrl)
        {
            if (returnUrl.Contains("mode=register"))
            {
                return RedirectToAction("Register", "Auth",
                new RegisterViewModel{
                    ReturnUrl = returnUrl
                });
            }
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
        public IActionResult Register([FromQuery] string returnUrl)
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

                // Отправить письмо со ссылкой для подтверждения почты!
                var confirmationToken = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                var confirmationLink = Url.Action(nameof(EmailConfirmation).ToString(), "Auth", new
                {
                    userId = userId,
                    token = confirmationToken,
                    returnUrl = vm.ReturnUrl
                }, Request.Scheme);

                // Пока что просто делаем лог, TODO: создать сервис отправки писемь
                _logger.Log(LogLevel.Information, confirmationLink);
                await _emailService.SendAsync(
                        user.Email,
                        user.UserName,
                        "Подтверждение эл. почты",
                    $"<h1>Спасибо за регистрацию, чтобы активировать вашу учетную запись " +
                         $"пройдите по следующей ссылке: {confirmationLink}</h1>");

                ViewBag.Title = "Подтвердите Email";
                ViewBag.Message = $"Ссылка для подтверждения вашей электроной почты отправлена на {user.Email}";
                return View("_Confirmation");
            }
            return View(vm);
        }

        [HttpGet]
        public async Task<IActionResult> EmailConfirmation(string userId, string token, string returnUrl)
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
                ViewBag.Message = "Вы успешно подтвердили адрес электронной почты! Вы можете зайти на сайт указав свой логин и пароль.";
                return View("Login", new LoginViewModel
                {
                    ReturnUrl = returnUrl
                });
            }

            ViewBag.ErrorTitle = "Ошибка подтверждения";
            ViewBag.Error = "Email не может быть подтвержден.";
            return View("_Error");
        }

        [HttpGet]
        public IActionResult ForgotPassword([FromQuery] string returnUrl)
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordViewModel vm)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(vm.Email);
                if (user != null && await _userManager.IsEmailConfirmedAsync(user))
                {

                    var resetPasswordToken = await _userManager.GeneratePasswordResetTokenAsync(user);
                    var resetPasswordLink = Url.Action("ResetPassword", "Auth", new
                    {
                        email = vm.Email,
                        token = resetPasswordToken,
                        returnUrl = vm.ReturnUrl
                    }, Request.Scheme);

                    await _emailService.SendAsync(
                        vm.Email,
                        user.UserName,
                        "Восстановление пароля",
                        $"<h3>{resetPasswordLink}</h3>");

                    ViewBag.Title = "Успешно отправлен";
                    ViewBag.Message = "На вашу почту отправлена ссылка для восстановления пароля, перейдите по ссылке чтобы восстановить пароль";
                    return View("_Confirmation");
                }
                ViewBag.Message = "Пользователь с таким email не существует, либо email не подтвержден";
            }

            return View(vm);
        }

        [HttpGet]
        public async Task<IActionResult> ResetPassword(string token, string email, string returnUrl)
        {
            if (token == null || email == null || returnUrl == null)
            {
                ViewBag.Title = "Ошибка восстановления";
                ViewBag.Error = "Невалидный токен восстановления пароля";
                return View("_Error");
            }

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel vm)
        {
            var user = await _userManager.FindByEmailAsync(vm.Email);

            if (user != null)
            {
                var result = await _userManager.ResetPasswordAsync(user, vm.Token, vm.Password);
                if (result.Succeeded)
                {
                    ViewBag.Message = "Можете зайти в учетную запись с новыми данными";
                    return View("Login", new LoginViewModel
                    {
                        ReturnUrl = vm.ReturnUrl
                    });
                }
            }

            ViewBag.Title = "Ошибка восстановления";
            ViewBag.Error = "Невалидный токен восстановления пароля";
            return View("_Error");
        }
    }
}
