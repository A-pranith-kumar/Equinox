using Microsoft.AspNetCore.Mvc;

namespace Equinox.Controllers
{
    public class ClassController : Controller
    {
        public IActionResult List(string id = "All")
        {
            return Content($"Main Area - ClassController - List() - ID = {id}");
        }
    }
}
