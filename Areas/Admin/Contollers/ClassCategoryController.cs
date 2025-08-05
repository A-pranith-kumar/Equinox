using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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

        // GET: /Admin/ClassCategory
        public IActionResult Index()
        {
            var categories = _context.ClassCategories
                                     .OrderBy(c => c.Name)
                                     .ToList();
            return View(categories);
        }

        // GET: /Admin/ClassCategory/Create
        public IActionResult Create() => View();

        // POST: /Admin/ClassCategory/Create
        [HttpPost]
        [ValidateAntiForgeryToken]   // ✅ Anti-forgery
        public IActionResult Create(ClassCategory category)
        {
            // Optional: simple duplicate check
            if (_context.ClassCategories.Any(c => c.Name == category.Name))
            {
                ModelState.AddModelError(nameof(category.Name), "Category name already exists.");
            }

            if (!ModelState.IsValid) return View(category);

            _context.ClassCategories.Add(category);
            _context.SaveChanges();
            TempData["Success"] = "Category created successfully.";
            return RedirectToAction(nameof(Index));   // ✅ PRG
        }

        // GET: /Admin/ClassCategory/Edit/5
        public IActionResult Edit(int id)
        {
            var category = _context.ClassCategories.Find(id);
            if (category == null) return NotFound();
            return View(category);
        }

        // POST: /Admin/ClassCategory/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]   // ✅ Anti-forgery
        public IActionResult Edit(int id, ClassCategory category)
        {
            if (id != category.ClassCategoryId) return NotFound();

            // Optional: duplicate check (ignore current record)
            if (_context.ClassCategories.Any(c => c.Name == category.Name && c.ClassCategoryId != id))
            {
                ModelState.AddModelError(nameof(category.Name), "Category name already exists.");
            }

            if (!ModelState.IsValid) return View(category);

            _context.ClassCategories.Update(category);
            _context.SaveChanges();
            TempData["Success"] = "Category updated successfully.";
            return RedirectToAction(nameof(Index));   // ✅ PRG
        }

        // GET: /Admin/ClassCategory/Details/5
        public IActionResult Details(int id)
        {
            var category = _context.ClassCategories
                                   .FirstOrDefault(c => c.ClassCategoryId == id);
            if (category == null) return NotFound();
            return View(category);
        }

        // GET: /Admin/ClassCategory/Delete/5
        public IActionResult Delete(int id)
        {
            var category = _context.ClassCategories.Find(id);
            if (category == null) return NotFound();
            return View(category);
        }

        // POST: /Admin/ClassCategory/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]   // ✅ Anti-forgery
        public IActionResult DeleteConfirmed(int id)
        {
            var category = _context.ClassCategories.Find(id);
            if (category != null)
            {
                _context.ClassCategories.Remove(category);
                _context.SaveChanges();
                TempData["Success"] = "Category deleted successfully.";
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
