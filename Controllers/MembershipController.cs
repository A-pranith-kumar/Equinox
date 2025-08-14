using Microsoft.AspNetCore.Mvc;
using Equinox.Models;                       // EquinoxContext
using Equinox.Models.DomainModels;          // Membership
using Equinox.Models.Data.Repository;       // Repository, QueryOptions

namespace Equinox.Areas.Admin.Controllers
{
    [Area("Admin")]
    // Single base route to avoid overlaps
    [Route("Admin/Membership")]
    public class MembershipController : Controller
    {
        private readonly Repository<Membership> _memberships;

        public MembershipController(EquinoxContext context)
        {
            _memberships = new Repository<Membership>(context);
        }

        // GET /Admin/Membership  and  /Admin/Membership/Index
        [HttpGet("")]
        [HttpGet("Index")]
        public IActionResult Index()
        {
            var items = _memberships.List(new QueryOptions<Membership>
            {
                OrderBy = m => m.Name,
                OrderByDirection = "asc"
            });
            return View(items);
        }

        // GET /Admin/Membership/Create
        [HttpGet("Create")]
        public IActionResult Create() => View(new Membership());

        // POST /Admin/Membership/Create
        [HttpPost("Create")]
        [ValidateAntiForgeryToken]
        public IActionResult CreatePost(Membership model)
        {
            if (!ModelState.IsValid) return View("Create", model);

            _memberships.Insert(model);
            _memberships.Save();
            TempData["Success"] = "Membership created.";
            return RedirectToAction(nameof(Index));
        }

        // GET /Admin/Membership/Edit/5
        [HttpGet("Edit/{id:int}")]
        public IActionResult Edit(int id)
        {
            var item = _memberships.Get(id);
            if (item == null) return NotFound();
            return View(item);
        }

        // POST /Admin/Membership/Edit
        [HttpPost("Edit")]
        [ValidateAntiForgeryToken]
        public IActionResult EditPost(Membership model)
        {
            if (!ModelState.IsValid) return View("Edit", model);

            _memberships.Update(model);
            _memberships.Save();
            TempData["Success"] = "Membership updated.";
            return RedirectToAction(nameof(Index));
        }

        // GET /Admin/Membership/Delete/5
        [HttpGet("Delete/{id:int}")]
        public IActionResult Delete(int id)
        {
            var item = _memberships.Get(id);
            if (item == null) return NotFound();
            return View(item);
        }

        // POST /Admin/Membership/DeleteConfirmed
        [HttpPost("DeleteConfirmed")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int membershipId)
        {
            var item = _memberships.Get(membershipId);
            if (item == null) return NotFound();

            _memberships.Delete(item);
            _memberships.Save();
            TempData["Success"] = "Membership deleted.";
            return RedirectToAction(nameof(Index));
        }
    }
}
