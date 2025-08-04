using Microsoft.AspNetCore.Mvc;
using Equinox.Models;
using System.Linq;

namespace Equinox.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ClubController : Controller
    {
        private readonly EquinoxContext _context;

        public ClubController(EquinoxContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var clubs = _context.Clubs.ToList();
            return View(clubs);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Club club)
        {
            if (ModelState.IsValid)
            {
                _context.Clubs.Add(club);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(club);
        }

        public IActionResult Edit(int id)
        {
            var club = _context.Clubs.Find(id);
            if (club == null)
                return NotFound();

            return View(club);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Club club)
        {
            if (ModelState.IsValid)
            {
                _context.Clubs.Update(club);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(club);
        }

        public IActionResult Details(int id)
        {
            var club = _context.Clubs.FirstOrDefault(c => c.ClubId == id);
            if (club == null)
                return NotFound();

            return View(club);
        }

        public IActionResult Delete(int id)
        {
            var club = _context.Clubs.Find(id);
            if (club == null)
                return NotFound();

            return View(club);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var club = _context.Clubs.Find(id);
            if (club != null)
            {
                _context.Clubs.Remove(club);
                _context.SaveChanges();
            }
            return RedirectToAction("Index");
        }
    }
}
