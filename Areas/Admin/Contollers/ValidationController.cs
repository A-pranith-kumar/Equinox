// Areas/Admin/Controllers/ValidationController.cs
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Text.RegularExpressions;
using Equinox.Models;
using Equinox.Models.DomainModels;
using Equinox.Models.Data.Repository;

namespace Equinox.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ValidationController : Controller
    {
        private readonly Repository<User> _users;

        public ValidationController(EquinoxContext ctx)
        {
            _users = new Repository<User>(ctx);
        }

        // Name must be unique (exclude current record)
        [AcceptVerbs("GET", "POST")]
        public IActionResult CheckUserName(string name, int userId = 0)
        {
            name = (name ?? string.Empty).Trim();

            var exists = _users.List(new QueryOptions<User>
            {
                Where = u => u.Name == name && u.UserId != userId
            }).Any();

            // ✅ true means "valid", string means "error"
            return Json(exists ? $"Name '{name}' is already in use." : (object)true);
        }

        // Phone must be unique (digits only; exclude current)
        [AcceptVerbs("GET", "POST")]
        public IActionResult CheckPhone(string phoneNumber, int userId = 0)
        {
            phoneNumber = Regex.Replace(phoneNumber ?? string.Empty, @"\D+", ""); // digits only

            var exists = _users.List(new QueryOptions<User>
            {
                Where = u => u.PhoneNumber == phoneNumber && u.UserId != userId
            }).Any();

            return Json(exists ? $"Phone number {phoneNumber} is already in use." : (object)true);
        }

        // Email must be unique (exclude current)
        [AcceptVerbs("GET", "POST")]
        public IActionResult CheckEmail(string email, int userId = 0)
        {
            email = (email ?? string.Empty).Trim();

            var exists = _users.List(new QueryOptions<User>
            {
                Where = u => u.Email == email && u.UserId != userId
            }).Any();

            return Json(exists ? $"Email '{email}' is already in use." : (object)true);
        }
    }
}
