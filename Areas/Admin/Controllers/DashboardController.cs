using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Tommava.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class DashboardController : Controller
    {
        public IActionResult Index()
        {
            ViewData["CurrentPage"] = "Dashboard";
            return View();
        }
    }
}
