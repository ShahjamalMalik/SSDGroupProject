using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using GroupProjectDeployment.Data;
using GroupProjectDeployment.Models;

namespace GroupProjectDeployment.Controllers
{
    public class ShoppingCartsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ShoppingCartsController(ApplicationDbContext context)
        {
            _context = context;
        }



        public async Task<IActionResult> Index()
        {
            List<ShoppingCart> shoppingCarts = new List<ShoppingCart>();
            shoppingCarts.AddRange(await _context.ShoppingCart.ToListAsync());

            List<ShoppingCartViewModel> cart = new List<ShoppingCartViewModel>();

            foreach (var productInCart in shoppingCarts)
            {
                var product = await _context.Products
                    .FirstOrDefaultAsync(m => m.Id == productInCart.ProductId);

                if (User.Identity.Name == productInCart.userName) {
                    cart.Add(new ShoppingCartViewModel()
                    {
                        CartId = productInCart.CartId,
                        userId = productInCart.UserId,
                        productId = productInCart.ProductId,

                        productName = product.Name,
                        productDescription = product.Description,
                        ImageUrl = product.ImageUrl,
                        Price = product.Price,
                    });
                }
            }
            return View(cart);
        }


        // GET: ShoppingCarts/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null || _context.ShoppingCart == null)
            {
                return NotFound();
            }

            var shoppingCart = await _context.ShoppingCart
                .Include(s => s.User)
                .FirstOrDefaultAsync(m => m.CartId == id);
            if (shoppingCart == null)
            {
                return NotFound();
            }

            return View(shoppingCart);
        }



        public async Task<IActionResult> AddToCart(Guid productId, string userName)
        {
            var product = await _context.Products.FirstOrDefaultAsync(p => p.Id == productId);
            var user = await _context.Users.FirstOrDefaultAsync(u => u.UserName.Equals(userName));

            if (product != null && user != null && product.quantity > 0)
            {
                //Build the cart Item
                var cartItem = new ShoppingCart();
                cartItem.ProductId = product.Id;
                cartItem.Product = product;
                cartItem.UserId = user.Id;
                cartItem.userName = user.UserName;
                user.Cart.Add(cartItem);

                product.quantity--;

                await _context.AddAsync(cartItem);
                await _context.SaveChangesAsync();

                return Ok("Item added to cart.");
            }
            return Ok("Item was not added to cart.");
        }



        // POST: ShoppingCarts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CartId,UserId")] ShoppingCart shoppingCart)
        {
            if (ModelState.IsValid)
            {
                shoppingCart.CartId = Guid.NewGuid();
                _context.Add(shoppingCart);
                await _context.SaveChangesAsync();
                return Ok();
            }
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", shoppingCart.UserId);
            return BadRequest();
        }


        // POST: ShoppingCarts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RemoveCartItem(Guid cartId, Guid productId)
        {
            if (_context.ShoppingCart == null)
            {
                return Problem("Entity set 'ApplicationDbContext.ShoppingCart'  is null.");
            }
            var cartItem = await _context.ShoppingCart
                .FirstOrDefaultAsync(c => c.ProductId == productId && c.CartId == cartId);
            if (cartItem != null)
            {
                _context.Remove(cartItem);
                await _context.SaveChangesAsync();
                return Ok("Item removed from cart.");
            }
            return BadRequest("Could not find item in cart.");   
        }

        public async Task<IActionResult> EmptyCart(Guid cartId)
        {
            if (_context.ShoppingCart == null)
            {
                return Problem("Shopping Cart is null");
            }
            //Get shopping cart by cart id
            var shoppingCart = await _context.ShoppingCart
                .FirstOrDefaultAsync(c => c.CartId.Equals(cartId));
            if(shoppingCart != null)
            {
                shoppingCart = new ShoppingCart(); //reset shopping cart
                await _context.SaveChangesAsync();
                return Ok("Cart successfully deleted.");
            }
            return BadRequest("Cart was already empty.");
        }


        private bool ShoppingCartExists(Guid id)
        {
          return (_context.ShoppingCart?.Any(e => e.CartId == id)).GetValueOrDefault();
        }
    }
}
