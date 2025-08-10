using Microsoft.AspNetCore.Mvc;
using System.Linq;
using Equinox.Models;                      // EquinoxContext
using Equinox.Models.DomainModels;         // Club, Booking, EquinoxClass
using Equinox.Models.Data.Repository;      // Repository + QueryOptions

namespace Equinox.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ClubController : Controller
    {
        private readonly Repository<Club> _clubs;
        private readonly Repository<Booking> _bookings;
        private readonly Repository<EquinoxClass> _classes;   // ← added

        public ClubController(EquinoxContext context)
        {
            _clubs    = new Repository<Club>(context);
            _bookings = new Repository<Booking>(context);
            _classes  = new Repository<EquinoxClass>(context);  // ← added
        }

        public IActionResult Index()
        {
            var items = _clubs.List(new QueryOptions<Club> {
                OrderBy = c => c.Name,
                OrderByDirection = "asc"
            });
            return View(items);
        }

        public IActionResult Create() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Club club)
        {
            if (!ModelState.IsValid) return View(club);
            _clubs.Insert(club);
            _clubs.Save();
            TempData["Message"] = "Club created.";
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Edit(int id)
        {
            var club = _clubs.Get(id);
            if (club == null) return NotFound();
            return View(club);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Club club)
        {
            if (!ModelState.IsValid) return View(club);
            _clubs.Update(club);
            _clubs.Save();
            TempData["Message"] = "Club updated.";
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Details(int id)
        {
            var club = _clubs.Get(id);
            if (club == null) return NotFound();
            return View(club);
        }

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
            // 1) Block if any booking exists for any class in this club.
            var hasBooked = _bookings.List(new QueryOptions<Booking> {
                Includes = "EquinoxClass",
                Where = b => b.EquinoxClass != null && b.EquinoxClass.ClubId == id
            }).Any();
            if (hasBooked)
            {
                TempData["ErrorMessage"] = "Cannot delete club. One or more classes in this club have bookings.";
                return RedirectToAction(nameof(Index));
            }

            // 2) Block if any class still references this club (avoids FK error).
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

            try
            {
                _clubs.Delete(club);
                _clubs.Save();
                TempData["Message"] = "Club deleted.";
            }
            catch
            {
                TempData["ErrorMessage"] = "Delete failed due to related data. Please remove or reassign related records first.";
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
