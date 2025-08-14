using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Equinox.Models;
using Equinox.Helpers;

namespace Equinox.Controllers
{
    public class BookingController : Controller
    {
        private readonly EquinoxContext _context;

        public BookingController(EquinoxContext context)
        {
            _context = context;
        }

        // ✅ POST: Book a class
        [HttpPost]
        public IActionResult Book(int id)
        {
            var session = new EquinoxSession(HttpContext);
            session.AddBookingId(id);
            session.SetBookingCount(session.GetBookingIds().Count);

            TempData["Message"] = "Class successfully booked!";
            return RedirectToAction("Filter", "EquinoxClass");
        }

        // ✅ GET: Show booked classes
        public IActionResult Index()
        {
            var session = new EquinoxSession(HttpContext);
            var bookings = session.GetBookingIds();

            var classes = _context.EquinoxClasses
                .Include(c => c.Club)
                .Include(c => c.ClassCategory)
                .Include(c => c.Coach)
                .Where(c => bookings.Contains(c.EquinoxClassId))
                .ToList();

            return View(classes);
        }

        // ✅ POST: Cancel a booking
        [HttpPost]
        public IActionResult Cancel(int id)
        {
            var session = new EquinoxSession(HttpContext);
            session.RemoveBookingId(id);
            session.SetBookingCount(session.GetBookingIds().Count);

            TempData["Message"] = "Booking cancelled.";
            return RedirectToAction("Index");
        }
    }
}
