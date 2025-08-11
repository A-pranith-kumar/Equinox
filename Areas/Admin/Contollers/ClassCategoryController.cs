using Microsoft.AspNetCore.Mvc;
using System.Linq;
using Equinox.Models;                      // EquinoxContext
using Equinox.Models.DomainModels;         // ClassCategory, Booking, EquinoxClass
using Equinox.Models.Data.Repository;      // Repository + QueryOptions

namespace Equinox.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ClassCategoryController : Controller
    {
        private readonly Repository<ClassCategory> _categories;
        private readonly Repository<EquinoxClass>  _classes;
        private readonly Repository<Booking>       _bookings;

        public ClassCategoryController(EquinoxContext context)
        {
            _categories = new Repository<ClassCategory>(context);
            _classes    = new Repository<EquinoxClass>(context);
            _bookings   = new Repository<Booking>(context);
        }

        private static string Norm(string? s) => (s ?? string.Empty).Trim();

        // GET: /Admin/ClassCategory
        public IActionResult Index()
        {
            var items = _categories.List(new QueryOptions<ClassCategory> {
                OrderBy = c => c.Name,
                OrderByDirection = "asc"
            });
            return View(items);
        }

        // GET: /Admin/ClassCategory/Create
        public IActionResult Create() => View(new ClassCategory());  // ensure defaults (Id = 0)

        // POST: /Admin/ClassCategory/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("Name,Image")] ClassCategory category)
        {
            // If the view posts an empty Id, clear the binder error
            ModelState.Remove(nameof(category.ClassCategoryId));

            // Normalize
            category.Name  = Norm(category.Name);
            category.Image = Norm(category.Image);

            // Required + uniqueness
            if (string.IsNullOrWhiteSpace(category.Name))
                ModelState.AddModelError(nameof(category.Name), "Category name is required.");

            var exists = _categories.Get(new QueryOptions<ClassCategory> {
                Where = c => c.Name == category.Name
            }) != null;
            if (exists)
                ModelState.AddModelError(nameof(category.Name), "Category name already exists.");

            if (!ModelState.IsValid) return View(category);

            _categories.Insert(category);
            _categories.Save();
            TempData["Success"] = "Category created successfully.";
            return RedirectToAction(nameof(Index));
        }

        // GET: /Admin/ClassCategory/Edit/5
        public IActionResult Edit(int id)
        {
            var category = _categories.Get(id);
            if (category == null) return NotFound();
            return View(category);
        }

        // POST: /Admin/ClassCategory/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit([Bind("ClassCategoryId,Name,Image")] ClassCategory category)
        {
            category.Name  = Norm(category.Name);
            category.Image = Norm(category.Image);

            if (string.IsNullOrWhiteSpace(category.Name))
                ModelState.AddModelError(nameof(category.Name), "Category name is required.");

            var exists = _categories.Get(new QueryOptions<ClassCategory> {
                Where = c => c.ClassCategoryId != category.ClassCategoryId && c.Name == category.Name
            }) != null;
            if (exists)
                ModelState.AddModelError(nameof(category.Name), "Category name already exists.");

            if (!ModelState.IsValid) return View(category);

            _categories.Update(category);
            _categories.Save();
            TempData["Success"] = "Category updated successfully.";
            return RedirectToAction(nameof(Index));
        }

        // GET: /Admin/ClassCategory/Details/5
        public IActionResult Details(int id)
        {
            var category = _categories.Get(id);
            if (category == null) return NotFound();
            return View(category);
        }

        // GET: /Admin/ClassCategory/Delete/5
        public IActionResult Delete(int id)
        {
            var category = _categories.Get(id);
            if (category == null) return NotFound();
            return View(category);
        }

        // POST: /Admin/ClassCategory/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            // 1) Block if any booking references a class in this category.
            var hasBooked = _bookings.List(new QueryOptions<Booking> {
                Includes = "EquinoxClass",
                Where = b => b.EquinoxClass != null && b.EquinoxClass.ClassCategoryId == id
            }).Any();

            if (hasBooked)
            {
                TempData["ErrorMessage"] =
                    "Cannot delete category. One or more classes in this category have bookings.";
                return RedirectToAction(nameof(Index));
            }

            // 2) Block if any class still uses this category (avoids FK error).
            var inUseByClasses = _classes.List(new QueryOptions<EquinoxClass> {
                Where = c => c.ClassCategoryId == id
            }).Any();

            if (inUseByClasses)
            {
                TempData["ErrorMessage"] =
                    "Cannot delete category. There are classes assigned to this category. " +
                    "Delete or reassign those classes first.";
                return RedirectToAction(nameof(Index));
            }

            var category = _categories.Get(id);
            if (category == null) return NotFound();

            _categories.Delete(category);
            _categories.Save();
            TempData["Success"] = "Category deleted successfully.";
            return RedirectToAction(nameof(Index));
        }
    }
}