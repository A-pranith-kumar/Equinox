using Microsoft.AspNetCore.Mvc;

namespace Equinox.Areas.Admin.Controllers
{
    [Area("Admin")] // ✅ Simplified attribute routing
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
