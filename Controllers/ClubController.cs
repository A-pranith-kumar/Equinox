using Microsoft.AspNetCore.Mvc;

namespace Equinox.Controllers
{
    public class ClubController : Controller
    {
        public IActionResult List(string id = "All")
        {
            return Content($"Main Area - ClubController - List() - ID = {id}");
        }
    }
}
