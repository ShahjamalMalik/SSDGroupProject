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
        public IActionResult Index(Guid cartId)
        {
            //           Guid cartId = Guid.Parse(cartIdString);
            var userName = User.Identity.Name;
            var cartItems = _context.ShoppingCart.Where(c => c.userName.Equals(userName)).ToList();
            decimal taxAmount = 0.13M;
            if(cartItems != null)
            {
                Checkout checkoutModel = new Checkout();
                checkoutModel.CartItems = cartItems;
                checkoutModel.TotalPrice = GetCartPrice(cartItems);
                checkoutModel.TaxAmount = checkoutModel.TotalPrice * taxAmount;
                checkoutModel.FinalPrice = checkoutModel.TotalPrice + checkoutModel.TaxAmount;
                return View(checkoutModel);
            }
            return BadRequest("cart was empty");
        }

        [HttpPost]
        public IActionResult CompletePurchase()
        { //TODO: Implement complete purchaes
            return Ok();
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
    }
}
