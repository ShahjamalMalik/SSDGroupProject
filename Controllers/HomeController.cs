using GroupProjectDeployment.Data;
using GroupProjectDeployment.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace GroupProjectDeployment.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        readonly ApplicationDbContext _context;

        public HomeController(ApplicationDbContext context, ILogger<HomeController> logger)
        {
            _logger = logger;
            _context = context;
        }

        /*public IActionResult Index()
        {

            return View();
        }*/
        public async Task<IActionResult> Index()
        {
            return _context.Products != null ?
                        View(await _context.Products.ToListAsync()) :
                        Problem("Entity set 'ApplicationDbContext.Products'  is null.");
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}