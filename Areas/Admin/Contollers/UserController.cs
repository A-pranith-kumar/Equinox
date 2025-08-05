using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Equinox.Models;
using System.Linq;

namespace Equinox.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class UserController : Controller
    {
        private readonly EquinoxContext _context;

        public UserController(EquinoxContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var users = _context.Users.ToList();
            return View(users);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(User user)
        {
            bool nameExists = _context.Users.Any(u => u.Name == user.Name);
            bool emailExists = _context.Users.Any(u => u.Email == user.Email);
            bool phoneExists = _context.Users.Any(u => u.PhoneNumber == user.PhoneNumber);

            if (nameExists)
                ModelState.AddModelError("Name", "Name already exists.");
            if (emailExists)
                ModelState.AddModelError("Email", "Email already exists.");
            if (phoneExists)
                ModelState.AddModelError("PhoneNumber", "Phone number already exists.");

            if (ModelState.IsValid)
            {
                _context.Users.Add(user);
                _context.SaveChanges();
                TempData["Message"] = "Coach created successfully!";
                return RedirectToAction(nameof(Index));
            }

            TempData["Message"] = "Please fix the error.";
            return View(user);
        }

        public IActionResult Edit(int id)
        {
            var user = _context.Users.Find(id);
            if (user == null)
                return NotFound();

            return View(user);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(User user)
        {
            bool nameExists = _context.Users.Any(u => u.Name == user.Name && u.UserId != user.UserId);
            bool emailExists = _context.Users.Any(u => u.Email == user.Email && u.UserId != user.UserId);
            bool phoneExists = _context.Users.Any(u => u.PhoneNumber == user.PhoneNumber && u.UserId != user.UserId);

            if (nameExists)
                ModelState.AddModelError("Name", "Name already exists.");
            if (emailExists)
                ModelState.AddModelError("Email", "Email already exists.");
            if (phoneExists)
                ModelState.AddModelError("PhoneNumber", "Phone number already exists.");

            if (ModelState.IsValid)
            {
                _context.Users.Update(user);
                _context.SaveChanges();
                TempData["Message"] = "Coach details updated successfully!";
                return RedirectToAction(nameof(Index));
            }

            TempData["Message"] = "Please fix the error.";
            return View(user);
        }

        public IActionResult Details(int id)
        {
            var user = _context.Users.FirstOrDefault(u => u.UserId == id);
            if (user == null)
                return NotFound();

            return View(user);
        }

        public IActionResult Delete(int id)
        {
            var user = _context.Users.Find(id);
            if (user == null)
                return NotFound();

            return View(user);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var user = _context.Users.Find(id);
            if (user != null)
            {
                _context.Users.Remove(user);
                _context.SaveChanges();
                TempData["Message"] = "Coach deleted successfully!";
            }

            return RedirectToAction(nameof(Index));
        }

        // Remote Validations

        [AcceptVerbs("GET", "POST")]
        public IActionResult VerifyPhoneNumber(string phoneNumber, int userId = 0)
        {
            var exists = _context.Users.Any(u => u.PhoneNumber == phoneNumber && u.UserId != userId);
            return Json(!exists);
        }

        [AcceptVerbs("GET", "POST")]
        public IActionResult VerifyName(string name, int userId = 0)
        {
            var exists = _context.Users.Any(u => u.Name == name && u.UserId != userId);
            return Json(!exists);
        }

        [AcceptVerbs("GET", "POST")]
        public IActionResult VerifyEmail(string email, int userId = 0)
        {
            var exists = _context.Users.Any(u => u.Email == email && u.UserId != userId);
            return Json(!exists);
        }
    }
}
