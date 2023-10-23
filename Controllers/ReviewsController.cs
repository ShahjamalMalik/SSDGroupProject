using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using GroupProjectDeployment.Data;
using GroupProjectDeployment.Models;
using Microsoft.AspNetCore.Authorization;

namespace GroupProjectDeployment.Controllers
{
    public class ReviewsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ReviewsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // POST: Reviews/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Create([Bind(Prefix = "NewReview")] Review review)
        {
            review.Product = _context.Products.Find(review.Id);
            if (ModelState.IsValid)
            {
                review.Id = Guid.NewGuid();
                _context.Add(review);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(HomeController.Index), "Home");
            }
            return RedirectToAction(nameof(HomeController.Error), "Home");
        }

        // POST: Reviews/Remove/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Remove(Guid id)
        {
            if (_context.Reviews == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Reviews'  is null.");
            }
            var review = await _context.Reviews.FindAsync(id);
            //Only delete if user is the owner of the review or an admin
            if (review != null
                && (User.Identity.Name.Equals(review.Email)
                || User.IsInRole("Admin")))
            {
                _context.Reviews.Remove(review);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index", "Home");
            }

            TempData["ErrorMessage"] = "Error deleting review. You are either not the owner of this review or not an admin.";
            return RedirectToAction("Error", "Home");
        }

        private bool ReviewExists(Guid id)
        {
            return (_context.Reviews?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}