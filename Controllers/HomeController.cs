using Microsoft.AspNetCore.Mvc;

namespace Equinox.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Contact()
        {
            return Content("Main Area - HomeController - Contact()");
        }

        public IActionResult Privacy()
        {
            return Content("Main Area - HomeController - Privacy()");
        }

        public IActionResult Terms()
        {
            return Content("Main Area - HomeController - Terms()");
        }

        public IActionResult Cookie()
        {
            return Content("Main Area - HomeController - Cookie()");
        }
    }
}
