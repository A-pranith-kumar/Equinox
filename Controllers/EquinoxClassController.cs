using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;                      // <-- for Where/Include LINQ ops
using Equinox.Models;                   // EquinoxContext
using Equinox.Helpers;                  // EquinoxSession
using Equinox.Models.ViewModels;        // <-- EquinoxFilterViewModel

namespace Equinox.Controllers
{
    public class EquinoxClassController : Controller
    {
        private readonly EquinoxContext _context;

        public EquinoxClassController(EquinoxContext context)
        {
            _context = context;
        }

        public IActionResult Index() => RedirectToAction("Filter");

        public IActionResult Filter(int selectedClubId = 0, int selectedCategoryId = 0)
        {
            var session = new EquinoxSession(HttpContext);
            session.SetSelectedClubId(selectedClubId);
            session.SetSelectedCategoryId(selectedCategoryId);

            var classesQuery = _context.EquinoxClasses
                .Include(c => c.Club)
                .Include(c => c.ClassCategory)
                .Include(c => c.Coach)
                .AsQueryable();

            if (selectedClubId != 0)
                classesQuery = classesQuery.Where(c => c.ClubId == selectedClubId);

            if (selectedCategoryId != 0)
                classesQuery = classesQuery.Where(c => c.ClassCategoryId == selectedCategoryId);

            var vm = new EquinoxFilterViewModel
            {
                EquinoxClasses = classesQuery.ToList(),
                Clubs = _context.Clubs.ToList(),
                Categories = _context.ClassCategories.ToList(),
                SelectedClubId = selectedClubId,
                SelectedCategoryId = selectedCategoryId
            };

            return View("Filter", vm);
        }

        public IActionResult Detail(int id)
        {
            var equinoxClass = _context.EquinoxClasses
                .Include(c => c.Club)
                .Include(c => c.ClassCategory)
                .Include(c => c.Coach)
                .FirstOrDefault(c => c.EquinoxClassId == id);

            if (equinoxClass == null) return NotFound();

            return View("Detail", equinoxClass);
        }

        [HttpPost]
        public IActionResult Book(int id)
        {
            var session = new EquinoxSession(HttpContext);
            session.AddBookingId(id);
            session.SetBookingCount(session.GetBookingIds().Count);

            TempData["Message"] = "Class successfully booked!";

            return RedirectToAction("Filter", new
            {
                selectedClubId = session.GetSelectedClubId(),
                selectedCategoryId = session.GetSelectedCategoryId()
            });
        }

        public IActionResult Booking()
        {
            var session = new EquinoxSession(HttpContext);
            var bookings = session.GetBookingIds();

            var bookedClasses = _context.EquinoxClasses
                .Include(c => c.Club)
                .Include(c => c.ClassCategory)
                .Include(c => c.Coach)
                .Where(c => bookings.Contains(c.EquinoxClassId))
                .ToList();

            return View("Booking", bookedClasses);
        }

        [HttpPost]
        public IActionResult Cancel(int id)
        {
            var session = new EquinoxSession(HttpContext);
            session.RemoveBookingId(id);
            session.SetBookingCount(session.GetBookingIds().Count);

            TempData["Message"] = "Booking cancelled.";
            return RedirectToAction("Booking");
        }
    }
}
