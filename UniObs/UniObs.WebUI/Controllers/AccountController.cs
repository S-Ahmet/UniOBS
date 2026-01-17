using Microsoft.AspNetCore.Mvc;
using UniObs.Application.Contracts.Services;
using UniObs.Application.Dtos.Auth;
using UniObs.WebUI.Models;

namespace UniObs.WebUI.Controllers
{
    public class AccountController : Controller
    {
        private readonly IAuthService _authService;

        public AccountController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View(new LoginViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            ModelState.Remove(nameof(LoginViewModel.Message));

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var request = new LoginRequestDto
            {
                Email = model.Email,
                Password = model.Password,
                IpAddress = HttpContext.Connection.RemoteIpAddress?.ToString()
            };

            var result = await _authService.LoginAsync(request);

            if (!result.Success)
            {
                model.Message = result.Message;
                return View(model);
            }

            // 2FA gerekiyorsa
            if (result.RequiresTwoFactor)
            {
                HttpContext.Session.SetString("PendingEmail", result.Email ?? "");
                TempData["Info"] = result.Message;
                return RedirectToAction("VerifyCode");
            }

            // (Normalde hep 2FA çalıştığı için buraya düşmeyecek)
            HttpContext.Session.SetString("UserEmail", result.Email ?? "");
            HttpContext.Session.SetString("UserRole", result.Role ?? "");
            return RedirectToAction("Index", "OgrenciPanel");
        }

        [HttpGet]
        public IActionResult VerifyCode()
        {
            var email = HttpContext.Session.GetString("PendingEmail");
            if (string.IsNullOrEmpty(email))
                return RedirectToAction("Login");

            var vm = new VerifyCodeViewModel
            {
                Message = TempData["Info"]?.ToString(),
                RemainingSeconds = 20
            };

            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> VerifyCode(VerifyCodeViewModel model)
        {
            ModelState.Remove(nameof(VerifyCodeViewModel.Message));

            if (!ModelState.IsValid)
            {
                // Kalan süre hidden field’dan geliyor, olduğu gibi kalacak
                return View(model);
            }

            var email = HttpContext.Session.GetString("PendingEmail");
            if (string.IsNullOrEmpty(email))
                return RedirectToAction("Login");

            var result = await _authService.VerifyTwoFactorCodeAsync(email, model.Code);

            if (!result.Success)
            {
                model.Message = result.Message;
                return View(model);
            }

            // Giriş tamamlandı
            HttpContext.Session.Remove("PendingEmail");
            HttpContext.Session.SetString("UserEmail", result.Email ?? "");
            HttpContext.Session.SetString("UserRole", result.Role ?? "");

            // Rol bazlı yönlendirme
            switch (result.Role)
            {
                case "Admin":
                    return RedirectToAction("Index", "AdminPanel");
                case "Akademisyen":
                    return RedirectToAction("Index", "AkademisyenPanel");
                case "Ogrenci":
                default:
                    return RedirectToAction("Index", "OgrenciPanel");
            }
        }

        [HttpPost]
        public async Task<IActionResult> ResendCode()
        {
            var email = HttpContext.Session.GetString("PendingEmail");
            if (string.IsNullOrEmpty(email))
                return Unauthorized();

            var result = await _authService.ResendTwoFactorCodeAsync(email);

            if (!result.Success)
                return BadRequest(result.Message);

            return Ok();
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }
    }
}
