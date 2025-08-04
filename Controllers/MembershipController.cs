using Microsoft.AspNetCore.Mvc;

namespace Equinox.Controllers
{
    public class MembershipController : Controller
    {
        public IActionResult List()
        {
            ViewBag.Controller = "MembershipController";
            return View();
        }
    }
}
