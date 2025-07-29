using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Equinox.Models;

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
            // Add classId to session booking list
            SessionHelper.AddBooking(HttpContext.Session, id);

            TempData["Message"] = "Class successfully booked!";
            return RedirectToAction("Filter", "Class");
        }

        // ✅ GET: Show booked classes
        public IActionResult Index()
        {
            var bookings = SessionHelper.GetBookings(HttpContext.Session);
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
            SessionHelper.RemoveBooking(HttpContext.Session, id);
            TempData["Message"] = "Booking cancelled.";
            return RedirectToAction("Index");
        }
    }
}
