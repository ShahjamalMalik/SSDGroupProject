using GroupProjectDeployment.Data;
using Microsoft.AspNetCore.Mvc;
using GroupProjectDeployment.Models;
using Microsoft.EntityFrameworkCore;

namespace GroupProjectDeployment.Controllers
{
    public class CheckoutController : Controller
    {

        ApplicationDbContext _context;

        public CheckoutController(ApplicationDbContext context)
        {
            _context = context;
        }


        // GET: CheckoutController
        [HttpGet]
        public IActionResult Index()
        {
            var userName = User.Identity.Name;
            var cartItems = _context.ShoppingCart.Where(c => c.userName.Equals(userName)).ToList();
            foreach (var cartItem in cartItems)
            {
                cartItem.Product = GetProduct(cartItem.ProductId);
            }
            decimal taxAmount = 0.13M;
            if(cartItems.Count() > 0)
            {
                Checkout checkoutModel = new Checkout();
                checkoutModel.CartItems = cartItems;
                checkoutModel.TotalPrice = GetCartPrice(cartItems);
                checkoutModel.TaxAmount = checkoutModel.TotalPrice * taxAmount;
                checkoutModel.FinalPrice = Math.Round(checkoutModel.TotalPrice + checkoutModel.TaxAmount, 2);

                return View(checkoutModel);
            }
            ViewBag.ErrorMessage = "There were no items in the shopping cart.";
            return View("Error", new ErrorViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> CompletePurchase()
        { //TODO: Implement complete purchaes
            var userName = User.Identity.Name;
            var cartItems = _context.ShoppingCart.Where(c => c.userName.Equals(userName));
            if (cartItems != null)
            {
                foreach (var cartItem in cartItems)
                {
                    var deleteCartItem = await _context.ShoppingCart.FirstOrDefaultAsync(c => c.CartId.Equals(cartItem.CartId));
                    _context.Remove(deleteCartItem);
                }
                _context.SaveChanges();
                return Ok("Items purchased successfully.");
            }
            ViewBag.ErrorMessage = "There were no items in the shopping cart.";
            return View("Error", new ErrorViewModel());
        }


        private decimal GetCartPrice(List<ShoppingCart> cartItems) 
        { 
            decimal price = 0;
            foreach(ShoppingCart cartItem in cartItems)
            {
                price += cartItem.Product.Price;
            }
            return price;
        }

        private Product GetProduct(Guid productId)
        {
            var product = _context.Products.FirstOrDefault(p => p.Id.Equals(productId));
            return product;
        }
    }
}
