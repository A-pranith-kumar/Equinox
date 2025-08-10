using Microsoft.AspNetCore.Mvc;
using System.Linq;
using Equinox.Models;                      // EquinoxContext
using Equinox.Models.DomainModels;         // User, Booking, EquinoxClass
using Equinox.Models.Data.Repository;      // Repository + QueryOptions

namespace Equinox.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class UserController : Controller
    {
        private readonly Repository<User> _users;
        private readonly Repository<Booking> _bookings;
        private readonly Repository<EquinoxClass> _classes;   // ← added

        public UserController(EquinoxContext context)
        {
            _users    = new Repository<User>(context);
            _bookings = new Repository<Booking>(context);
            _classes  = new Repository<EquinoxClass>(context); // ← added
        }

        // LIST
        public IActionResult Index()
        {
            var items = _users.List(new QueryOptions<User> {
                OrderBy = u => u.Name,
                OrderByDirection = "asc"
            });
            return View(items);
        }

        // CREATE (GET)
        public IActionResult Create() => View();

        // CREATE (POST)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(User user)
        {
            // Server-side uniqueness checks
            if (_users.Get(new QueryOptions<User> { Where = u => u.Name == user.Name }) != null)
                ModelState.AddModelError(nameof(user.Name), "Name already exists.");
            if (_users.Get(new QueryOptions<User> { Where = u => u.Email == user.Email }) != null)
                ModelState.AddModelError(nameof(user.Email), "Email already exists.");
            if (_users.Get(new QueryOptions<User> { Where = u => u.PhoneNumber == user.PhoneNumber }) != null)
                ModelState.AddModelError(nameof(user.PhoneNumber), "Phone number already exists.");

            if (!ModelState.IsValid) return View(user);

            _users.Insert(user);
            _users.Save();
            TempData["Message"] = "Coach created successfully!";
            return RedirectToAction(nameof(Index));
        }

        // EDIT (GET)
        public IActionResult Edit(int id)
        {
            var item = _users.Get(id);
            if (item == null) return NotFound();
            return View(item);
        }

        // EDIT (POST)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(User user)
        {
            // Exclude current record from duplicates
            if (_users.Get(new QueryOptions<User> { Where = u => u.Name == user.Name && u.UserId != user.UserId }) != null)
                ModelState.AddModelError(nameof(user.Name), "Name already exists.");
            if (_users.Get(new QueryOptions<User> { Where = u => u.Email == user.Email && u.UserId != user.UserId }) != null)
                ModelState.AddModelError(nameof(user.Email), "Email already exists.");
            if (_users.Get(new QueryOptions<User> { Where = u => u.PhoneNumber == user.PhoneNumber && u.UserId != user.UserId }) != null)
                ModelState.AddModelError(nameof(user.PhoneNumber), "Phone number already exists.");

            if (!ModelState.IsValid) return View(user);

            _users.Update(user);
            _users.Save();
            TempData["Message"] = "Coach details updated successfully!";
            return RedirectToAction(nameof(Index));
        }

        // DETAILS
        public IActionResult Details(int id)
        {
            var item = _users.Get(id);
            if (item == null) return NotFound();
            return View(item);
        }

        // DELETE (GET)
        public IActionResult Delete(int id)
        {
            var item = _users.Get(id);
            if (item == null) return NotFound();
            return View(item);
        }

        // DELETE (POST)
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            // 1) Phase 4 rule: block if any booking exists for classes taught by this coach.
            var hasBooked = _bookings.List(new QueryOptions<Booking> {
                Includes = "EquinoxClass",
                Where = b => b.EquinoxClass != null && b.EquinoxClass.CoachId == id
            }).Any();
            if (hasBooked)
            {
                TempData["ErrorMessage"] = "Cannot delete coach. One or more of their classes have bookings.";
                return RedirectToAction(nameof(Index));
            }

            // 2) Also block if any class still references this coach (avoids FK error).
            var inUseByClasses = _classes.List(new QueryOptions<EquinoxClass> {
                Where = c => c.CoachId == id
            }).Any();
            if (inUseByClasses)
            {
                TempData["ErrorMessage"] = "Cannot delete coach. They are still assigned to classes. Reassign or delete those classes first.";
                return RedirectToAction(nameof(Index));
            }

            var item = _users.Get(id);
            if (item == null) return NotFound();

            try
            {
                _users.Delete(item);
                _users.Save();
                TempData["Message"] = "Coach deleted successfully!";
            }
            catch
            {
                TempData["ErrorMessage"] = "Delete failed due to related data. Please remove or reassign related records first.";
            }

            return RedirectToAction(nameof(Index));
        }

        // ---------------------------
        // Remote Validation Endpoints
        // ---------------------------

        [AcceptVerbs("GET", "POST")]
        public IActionResult VerifyPhoneNumber(string phoneNumber, int userId = 0)
        {
            var exists = _users.Get(new QueryOptions<User> {
                Where = u => u.PhoneNumber == phoneNumber && u.UserId != userId
            }) != null;
            return Json(!exists);
        }

        [AcceptVerbs("GET", "POST")]
        public IActionResult VerifyName(string name, int userId = 0)
        {
            var exists = _users.Get(new QueryOptions<User> {
                Where = u => u.Name == name && u.UserId != userId
            }) != null;
            return Json(!exists);
        }

        [AcceptVerbs("GET", "POST")]
        public IActionResult VerifyEmail(string email, int userId = 0)
        {
            var exists = _users.Get(new QueryOptions<User> {
                Where = u => u.Email == email && u.UserId != userId
            }) != null;
            return Json(!exists);
        }
    }
}
