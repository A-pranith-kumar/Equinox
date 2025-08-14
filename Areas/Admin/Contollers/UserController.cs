using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Text.RegularExpressions;
using Equinox.Models;                      // EquinoxContext
using Equinox.Models.DomainModels;         // User, Booking, EquinoxClass
using Equinox.Models.Data.Repository;      // Repository + QueryOptions
using Equinox.Models.ViewModels;           // PagedResult<T>

namespace Equinox.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class UserController : Controller
    {
        private readonly Repository<User> _users;
        private readonly Repository<Booking> _bookings;
        private readonly Repository<EquinoxClass> _classes;

        public UserController(EquinoxContext context)
        {
            _users    = new Repository<User>(context);
            _bookings = new Repository<Booking>(context);
            _classes  = new Repository<EquinoxClass>(context);
        }

        private static string Norm(string? s) => (s ?? string.Empty).Trim();

        // ---------- LIST (Paged) ----------
        // Page 1 → 4 records, Page 2 → remaining (with your current data)
        public IActionResult Index(int page = 1)
        {
            const int pageSize = 4;  // fixed at 4 per page
            if (page < 1) page = 1;

            var total = _users.Count;   // total WITHOUT paging

            var items = _users.List(new QueryOptions<User>
            {
                OrderBy = u => u.Name,
                OrderByDirection = "asc",
                PageNumber = page,
                PageSize = pageSize
            }).ToList();

            var model = new PagedResult<User>
            {
                Items      = items,
                Page       = page,
                PageSize   = pageSize,
                TotalCount = total
            };

            return View(model);
        }

        // ---------- CREATE ----------
        [HttpGet]
        public IActionResult Create() => View(new User()); // safe defaults

        [HttpPost]
        [ValidateAntiForgeryToken]
        // Do NOT bind UserId on create; DOB can be empty
        public IActionResult Create([Bind("Name,Email,PhoneNumber,DOB,IsCoach")] User user)
        {
            // clear binder noise like "The value '' is invalid."
            ModelState.Remove(nameof(user.UserId));
            ModelState.Remove(nameof(user.DOB));

            user.Name        = Norm(user.Name);
            user.Email       = Norm(user.Email);
            user.PhoneNumber = Norm(user.PhoneNumber);

            if (string.IsNullOrWhiteSpace(user.Name))
                ModelState.AddModelError(nameof(user.Name), "Name is required.");
            if (string.IsNullOrWhiteSpace(user.Email))
                ModelState.AddModelError(nameof(user.Email), "Email is required.");

            // phone optional but if present → 10 digits
            if (!string.IsNullOrEmpty(user.PhoneNumber) &&
                !Regex.IsMatch(user.PhoneNumber, @"^\d{10}$"))
                ModelState.AddModelError(nameof(user.PhoneNumber), "Enter a 10-digit phone number (digits only).");

            // uniqueness
            if (_users.Get(new QueryOptions<User> { Where = u => u.Name == user.Name }) != null)
                ModelState.AddModelError(nameof(user.Name), "Name already exists.");
            if (_users.Get(new QueryOptions<User> { Where = u => u.Email == user.Email }) != null)
                ModelState.AddModelError(nameof(user.Email), "Email already exists.");
            if (!string.IsNullOrEmpty(user.PhoneNumber) &&
                _users.Get(new QueryOptions<User> { Where = u => u.PhoneNumber == user.PhoneNumber }) != null)
                ModelState.AddModelError(nameof(user.PhoneNumber), "Phone number already exists.");

            if (!ModelState.IsValid) return View(user);

            _users.Insert(user);
            _users.Save();
            TempData["Message"] = "Coach created successfully!";
            return RedirectToAction(nameof(Index));
        }

        // ---------- EDIT ----------
        [HttpGet]
        public IActionResult Edit(int id)
        {
            var item = _users.Get(id);
            if (item == null) return NotFound();
            return View(item);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit([Bind("UserId,Name,Email,PhoneNumber,DOB,IsCoach")] User user)
        {
            user.Name        = Norm(user.Name);
            user.Email       = Norm(user.Email);
            user.PhoneNumber = Norm(user.PhoneNumber);

            // allow empty DOB on edit
            ModelState.Remove(nameof(user.DOB));

            if (string.IsNullOrWhiteSpace(user.Name))
                ModelState.AddModelError(nameof(user.Name), "Name is required.");
            if (string.IsNullOrWhiteSpace(user.Email))
                ModelState.AddModelError(nameof(user.Email), "Email is required.");

            if (!string.IsNullOrEmpty(user.PhoneNumber) &&
                !Regex.IsMatch(user.PhoneNumber, @"^\d{10}$"))
                ModelState.AddModelError(nameof(user.PhoneNumber), "Enter a 10-digit phone number (digits only).");

            // uniqueness excluding current record
            if (_users.Get(new QueryOptions<User> { Where = u => u.Name == user.Name && u.UserId != user.UserId }) != null)
                ModelState.AddModelError(nameof(user.Name), "Name already exists.");
            if (_users.Get(new QueryOptions<User> { Where = u => u.Email == user.Email && u.UserId != user.UserId }) != null)
                ModelState.AddModelError(nameof(user.Email), "Email already exists.");
            if (!string.IsNullOrEmpty(user.PhoneNumber) &&
                _users.Get(new QueryOptions<User> { Where = u => u.PhoneNumber == user.PhoneNumber && u.UserId != user.UserId }) != null)
                ModelState.AddModelError(nameof(user.PhoneNumber), "Phone number already exists.");

            if (!ModelState.IsValid) return View(user);

            _users.Update(user);
            _users.Save();
            TempData["Message"] = "Coach details updated successfully!";
            return RedirectToAction(nameof(Index));
        }

        // ---------- DETAILS ----------
        public IActionResult Details(int id)
        {
            var item = _users.Get(id);
            if (item == null) return NotFound();
            return View(item);
        }

        // ---------- DELETE ----------
        [HttpGet]
        public IActionResult Delete(int id)
        {
            var item = _users.Get(id);
            if (item == null)
            {
                TempData["ErrorMessage"] = "Coach not found or already deleted.";
                return RedirectToAction(nameof(Index));
            }
            return View(item);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var item = _users.Get(id);
            if (item == null)
            {
                TempData["ErrorMessage"] = "Coach not found or already deleted.";
                return RedirectToAction(nameof(Index));
            }

            var hasBooked = _bookings.List(new QueryOptions<Booking> {
                Includes = "EquinoxClass",
                Where = b => b.EquinoxClass != null && b.EquinoxClass.CoachId == id
            }).Any();

            if (hasBooked)
            {
                TempData["ErrorMessage"] = "Cannot delete coach. One or more of their classes have bookings.";
                return RedirectToAction(nameof(Index));
            }

            var inUseByClasses = _classes.List(new QueryOptions<EquinoxClass> {
                Where = c => c.CoachId == id
            }).Any();

            if (inUseByClasses)
            {
                TempData["ErrorMessage"] = "Cannot delete coach. They are still assigned to classes. Reassign or delete those classes first.";
                return RedirectToAction(nameof(Index));
            }

            _users.Delete(item);
            _users.Save();
            TempData["Message"] = "Coach deleted successfully!";
            return RedirectToAction(nameof(Index));
        }

        // ---------- Remote Validation Endpoints ----------
        // Return TRUE when value is unique (valid)
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
