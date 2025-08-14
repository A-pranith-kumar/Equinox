using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Text.RegularExpressions;
using Equinox.Models;                      // EquinoxContext
using Equinox.Models.DomainModels;         // Club, Booking, EquinoxClass
using Equinox.Models.Data.Repository;      // Repository + QueryOptions
using Equinox.Models.ViewModels;           // PagedResult<T>

namespace Equinox.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ClubController : Controller
    {
        private readonly Repository<Club> _clubs;
        private readonly Repository<Booking> _bookings;
        private readonly Repository<EquinoxClass> _classes;

        public ClubController(EquinoxContext context)
        {
            _clubs    = new Repository<Club>(context);
            _bookings = new Repository<Booking>(context);
            _classes  = new Repository<EquinoxClass>(context);
        }

        private static string Norm(string? s) => (s ?? string.Empty).Trim();

        // ---------- List (Paged) ----------
        // /Admin/Club?page=1&pageSize=10
        public IActionResult Index(int page = 1, int pageSize = 10)
        {
            if (page < 1) page = 1;
            if (pageSize <= 0 || pageSize > 100) pageSize = 10;

            var items = _clubs.List(new QueryOptions<Club> {
                OrderBy = c => c.Name,
                OrderByDirection = "asc",
                PageNumber = page,
                PageSize   = pageSize
            }).ToList();

            var model = new PagedResult<Club>
            {
                Items      = items,
                Page       = page,
                PageSize   = pageSize,
                TotalCount = _clubs.Count
            };

            return View(model);
        }

        // ---------- Create ----------
        // Return a model so hidden inputs (if any) render with defaults (ClubId=0)
        public IActionResult Create() => View(new Club());

        [HttpPost]
        [ValidateAntiForgeryToken]
        // ⬇ Do NOT bind ClubId on Create
        public IActionResult Create([Bind("Name,PhoneNumber")] Club club)
        {
            // In case the view still posts ClubId (empty), nuke binder error.
            ModelState.Remove(nameof(club.ClubId));

            club.Name        = Norm(club.Name);
            club.PhoneNumber = Norm(club.PhoneNumber);

            // Validate phone ourselves (avoid over-strict [Phone])
            ModelState.Remove(nameof(club.PhoneNumber));
            if (string.IsNullOrWhiteSpace(club.Name))
                ModelState.AddModelError(nameof(club.Name), "Club name is required.");

            if (!string.IsNullOrEmpty(club.PhoneNumber) &&
                !Regex.IsMatch(club.PhoneNumber, @"^\d{10}$"))
                ModelState.AddModelError(nameof(club.PhoneNumber), "Enter a 10-digit phone number (digits only).");

            // Uniqueness checks
            if (_clubs.Get(new QueryOptions<Club> { Where = c => c.Name == club.Name }) != null)
                ModelState.AddModelError(nameof(club.Name), "Club name already exists.");

            if (!string.IsNullOrEmpty(club.PhoneNumber) &&
                _clubs.Get(new QueryOptions<Club> { Where = c => c.PhoneNumber == club.PhoneNumber }) != null)
                ModelState.AddModelError(nameof(club.PhoneNumber), "Phone number already exists.");

            if (!ModelState.IsValid)
            {
                TempData["ErrorMessage"] = "Please fix the errors below.";
                return View(club);
            }

            _clubs.Insert(club);
            _clubs.Save();
            TempData["Message"] = "Club created.";
            return RedirectToAction(nameof(Index));
        }

        // ---------- Edit ----------
        public IActionResult Edit(int id)
        {
            var club = _clubs.Get(id);
            if (club == null) return NotFound();
            return View(club);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit([Bind("ClubId,Name,PhoneNumber")] Club club)
        {
            club.Name        = Norm(club.Name);
            club.PhoneNumber = Norm(club.PhoneNumber);

            ModelState.Remove(nameof(club.PhoneNumber));
            if (string.IsNullOrWhiteSpace(club.Name))
                ModelState.AddModelError(nameof(club.Name), "Club name is required.");
            if (!string.IsNullOrEmpty(club.PhoneNumber) &&
                !Regex.IsMatch(club.PhoneNumber, @"^\d{10}$"))
                ModelState.AddModelError(nameof(club.PhoneNumber), "Enter a 10-digit phone number (digits only).");

            if (_clubs.Get(new QueryOptions<Club> { Where = c => c.ClubId != club.ClubId && c.Name == club.Name }) != null)
                ModelState.AddModelError(nameof(club.Name), "Club name already exists.");
            if (!string.IsNullOrEmpty(club.PhoneNumber) &&
                _clubs.Get(new QueryOptions<Club> { Where = c => c.ClubId != club.ClubId && c.PhoneNumber == club.PhoneNumber }) != null)
                ModelState.AddModelError(nameof(club.PhoneNumber), "Phone number already exists.");

            if (!ModelState.IsValid)
            {
                TempData["ErrorMessage"] = "Please fix the errors below.";
                return View(club);
            }

            _clubs.Update(club);
            _clubs.Save();
            TempData["Message"] = "Club updated.";
            return RedirectToAction(nameof(Index));
        }

        // ---------- Details ----------
        public IActionResult Details(int id)
        {
            var club = _clubs.Get(id);
            if (club == null) return NotFound();
            return View(club);
        }

        // ---------- Delete ----------
        public IActionResult Delete(int id)
        {
            var club = _clubs.Get(id);
            if (club == null) return NotFound();
            return View(club);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var hasBooked = _bookings.List(new QueryOptions<Booking> {
                Includes = "EquinoxClass",
                Where = b => b.EquinoxClass != null && b.EquinoxClass.ClubId == id
            }).Any();

            if (hasBooked)
            {
                TempData["ErrorMessage"] = "Cannot delete club. One or more classes in this club have bookings.";
                return RedirectToAction(nameof(Index));
            }

            var inUseByClasses = _classes.List(new QueryOptions<EquinoxClass> {
                Where = c => c.ClubId == id
            }).Any();

            if (inUseByClasses)
            {
                TempData["ErrorMessage"] = "Cannot delete club. There are classes assigned to this club. Delete or reassign them first.";
                return RedirectToAction(nameof(Index));
            }

            var club = _clubs.Get(id);
            if (club == null) return NotFound();

            _clubs.Delete(club);
            _clubs.Save();
            TempData["Message"] = "Club deleted.";
            return RedirectToAction(nameof(Index));
        }
    }
}
