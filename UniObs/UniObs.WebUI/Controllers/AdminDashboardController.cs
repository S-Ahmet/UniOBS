using Microsoft.AspNetCore.Mvc;

namespace UniObs.WebUI.Controllers
{
    public class AdminDashboardController : Controller
    {
        public IActionResult Index()
        {
            var email = HttpContext.Session.GetString("UserEmail");
            var role = HttpContext.Session.GetString("UserRole");

            ViewBag.Email = email;
            ViewBag.Role = role;

            return View();
        }
    }
}
