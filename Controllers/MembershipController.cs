using Microsoft.AspNetCore.Mvc;

namespace Equinox.Controllers
{
    public class MembershipController : Controller
    {
        public IActionResult List(string id = "All")
        {
            return Content($"Main Area - MembershipController - List() - ID = {id}");
        }
    }
}
