using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using GroupProjectDeployment.Data;
using GroupProjectDeployment.Models;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Mvc.ViewComponents;

namespace GroupProjectDeployment.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;
        private IWebHostEnvironment _environment;


        public HomeController(ApplicationDbContext context, IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }

        // GET: Products
        public async Task<IActionResult> Index()
        {
            return _context.Products != null ?
                        View(await _context.Products.ToListAsync()) :
                        Problem("Entity set 'ApplicationDbContext.Products'  is null.");
        }




        // GET: Products/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null || _context.Products == null)
            {
                return NotFound();
            }

            var product = await _context.Products
                .FirstOrDefaultAsync(m => m.Id == id);
            if (product == null)
            {
                return NotFound();
            }

            ProductReviewViewModel model = new ProductReviewViewModel();
            model.Product = product;
            return View(model);
        }






        private bool ProductExists(Guid id)
        {
            return (_context.Products?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
