using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Equinox.Models;

namespace Equinox.Controllers
{
    public class EquinoxClassController : Controller
    {
        private readonly EquinoxContext _context;

        public EquinoxClassController(EquinoxContext context)
        {
            _context = context;
        }

        public IActionResult Filter(int selectedClubId = 0, int selectedCategoryId = 0)
        {
            HttpContext.Session.SetInt32("SelectedClubId", selectedClubId);
            HttpContext.Session.SetInt32("SelectedCategoryId", selectedCategoryId);

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

            if (equinoxClass == null)
                return NotFound();

            return View("Detail", equinoxClass);
        }

        [HttpPost]
        public IActionResult Book(int id)
        {
            SessionHelper.AddBooking(HttpContext.Session, id);
            TempData["Message"] = "Class successfully booked!";

            var selectedClubId = HttpContext.Session.GetInt32("SelectedClubId") ?? 0;
            var selectedCategoryId = HttpContext.Session.GetInt32("SelectedCategoryId") ?? 0;

            return RedirectToAction("Filter", "EquinoxClass", new
            {
                selectedClubId,
                selectedCategoryId
            });
        }

        public IActionResult Booking()
        {
            var bookings = SessionHelper.GetBookings(HttpContext.Session);

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
            SessionHelper.RemoveBooking(HttpContext.Session, id);
            TempData["Message"] = "Booking cancelled.";

            return RedirectToAction(nameof(Booking));
        }
    }
}
