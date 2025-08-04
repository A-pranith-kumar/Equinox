using Microsoft.AspNetCore.Mvc;
using Equinox.Models;
using System.Linq;

namespace Equinox.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ClassCategoryController : Controller
    {
        private readonly EquinoxContext _context;

        public ClassCategoryController(EquinoxContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var categories = _context.ClassCategories.ToList();
            return View(categories);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(ClassCategory category)
        {
            if (ModelState.IsValid)
            {
                _context.ClassCategories.Add(category);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(category);
        }

        public IActionResult Edit(int id)
        {
            var category = _context.ClassCategories.Find(id);
            if (category == null)
                return NotFound();

            return View(category);
        }

        [HttpPost]
        public IActionResult Edit(ClassCategory category)
        {
            if (ModelState.IsValid)
            {
                _context.ClassCategories.Update(category);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(category);
        }

        // ✅ NEW Details action
        public IActionResult Details(int id)
        {
            var category = _context.ClassCategories.FirstOrDefault(c => c.ClassCategoryId == id);
            if (category == null)
                return NotFound();

            return View(category); // Expects Views/ClassCategory/Details.cshtml
        }

        public IActionResult Delete(int id)
        {
            var category = _context.ClassCategories.Find(id);
            if (category == null)
                return NotFound();

            return View(category);
        }

        [HttpPost, ActionName("Delete")]
        public IActionResult DeleteConfirmed(int id)
        {
            var category = _context.ClassCategories.Find(id);
            if (category != null)
            {
                _context.ClassCategories.Remove(category);
                _context.SaveChanges();
            }
            return RedirectToAction("Index");
        }
    }
}