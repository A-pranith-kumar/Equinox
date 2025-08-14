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
        private readonly Repository<Club> _clubs;                     // ✅ added
        private readonly Repository<ClassCategory> _categories;       // ✅ added

        public ValidationController(EquinoxContext ctx)
        {
            _users      = new Repository<User>(ctx);
            _clubs      = new Repository<Club>(ctx);                  // ✅
            _categories = new Repository<ClassCategory>(ctx);         // ✅
        }

       [AcceptVerbs("GET","POST")]
public IActionResult CheckClubName(string name, int clubId = 0)
{
    name = (name ?? string.Empty).Trim();
    var exists = _clubs.List(new QueryOptions<Club> {
        Where = c => c.Name == name && c.ClubId != clubId
    }).Any();
    return Json(exists ? $"Club name '{name}' is already in use." : (object)true);
}

[AcceptVerbs("GET","POST")]
public IActionResult CheckCategoryName(string name, int classCategoryId = 0)
{
    name = (name ?? string.Empty).Trim();
    var exists = _categories.List(new QueryOptions<ClassCategory> {
        Where = c => c.Name == name && c.ClassCategoryId != classCategoryId
    }).Any();
    return Json(exists ? $"Category name '{name}' is already in use." : (object)true);
}


        // ===================== USERS (existing) =====================
        [AcceptVerbs("GET", "POST")]
        public IActionResult CheckUserName(string name, int userId = 0)
        {
            name = (name ?? string.Empty).Trim();
            var exists = _users.List(new QueryOptions<User> {
                Where = u => u.Name == name && u.UserId != userId
            }).Any();
            return Json(exists ? $"Name '{name}' is already in use." : (object)true);
        }

        [AcceptVerbs("GET", "POST")]
        public IActionResult CheckPhone(string phoneNumber, int userId = 0)
        {
            phoneNumber = (phoneNumber ?? string.Empty).Trim();

            // optional: enforce digits only
            if (!Regex.IsMatch(phoneNumber, @"^\d+$"))
                return Json("Phone must contain digits only.");

            var exists = _users.List(new QueryOptions<User> {
                Where = u => u.PhoneNumber == phoneNumber && u.UserId != userId
            }).Any();

            return Json(exists ? $"Phone number {phoneNumber} is already in use." : (object)true);
        }

        [AcceptVerbs("GET", "POST")]
        public IActionResult CheckEmail(string email, int userId = 0)
        {
            email = (email ?? string.Empty).Trim();

            var exists = _users.List(new QueryOptions<User> {
                Where = u => u.Email == email && u.UserId != userId
            }).Any();

            return Json(exists ? $"Email '{email}' is already in use." : (object)true);
        }
    }
}