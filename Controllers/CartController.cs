using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SakeFigureShop.Data;
using SakeFigureShop.Domains;
using SakeFigureShop.Models.Cart;

namespace SakeFigureShop.Controllers
{
    public class CartController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<CartController> _logger;
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;

        public CartController(
            ILogger<CartController> logger,
            UserManager<User> userManager,
            SignInManager<User> signInManager,
            ApplicationDbContext context)
        {
            _logger = logger;
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;
        }

        [AllowAnonymous]
        public IActionResult Index()
        {
            if (_signInManager.IsSignedIn(User))
            {
                var vm = new CartViewModel();
                var user = _context.Users.Where(u => u.Email == User.Identity.Name).FirstOrDefault();
                var cartItems = _context.CartItems
                    .Include(c => c.Product)
                    .Include(c => c.Product.Brand)
                    .Include(c => c.Product.Film)
                    .Where(c => c.UserId == user.Id).ToList();
                vm.cartItems = cartItems;
                return View(vm);
            }
            return View();
        }
        
        public IActionResult OrderDetail(int id)
        {
            var user = _context.Users.Where(u => u.Email == User.Identity.Name).FirstOrDefault();

            var order = _context.Orders
                .Include(o => o.OrderDetails)
                .ThenInclude(od => od.Product)
                .Where(o => o.UserId == user.Id && o.Id == id).FirstOrDefault();

            if (order == null)
            {
                return Redirect("/Identity/Account/Manage/ListOrder");
            }

            return View(order);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateCart([FromBody] List<CartItem> cartItems)
        {
            var user = _context.Users.Where(u => u.Email == User.Identity.Name).FirstOrDefault();
            if (cartItems == null)
            {
                return Json(new { success = false, message = "Bad Request!" });
            }
            foreach (var cartItem in cartItems)
            {
                var userCartItem = _context.CartItems
                .Where(c => c.UserId == user.Id)
                .Where(c => c.ProductId == cartItem.ProductId)
                .FirstOrDefault();
                if (cartItem == null)
                {
                    return Json(new { success = false, message = "Bad Request!" });
                }
                var prod = _context.Products.Where(p => p.Id == cartItem.ProductId).FirstOrDefault();
                if (cartItem.ProductId == null || prod == null)
                {
                    return Json(new { success = false, message = "Invalid ProductId" });
                }

                if (cartItem.quantity == null || prod.Quantity < cartItem.quantity)
                {
                    return Json(new { success = false, message = "Invalid quantity or out of stock" });
                }
                if (userCartItem == null)
                {
                    var newCartItem = new CartItem();
                    newCartItem.ProductId = cartItem.ProductId;
                    newCartItem.UserId = user.Id;
                    newCartItem.quantity = cartItem.quantity;
                    _context.CartItems.Add(newCartItem);
                }
                else
                {
                    userCartItem.quantity = cartItem.quantity;
                    if (userCartItem.quantity > prod.Quantity)
                    {
                        userCartItem.quantity = prod.Quantity;
                    }
                    _context.Update(userCartItem);
                }

                await _context.SaveChangesAsync();
            }
            return Json(new { success = true, message = "Already Added to Cart." });
        }

        [HttpPost]
        public async Task<IActionResult> AddToCart([FromBody] CartItem cartItem)
        {
            var user = _context.Users.Where(u => u.Email == User.Identity.Name).FirstOrDefault();
            var userCartItem = _context.CartItems
                .Where(c => c.UserId == user.Id)
                .Where(c => c.ProductId == cartItem.ProductId)
                .FirstOrDefault();
            if (cartItem == null)
            {
                return Json(new { success = false, message = "Bad Request!"});
            }
            var prod = _context.Products.Where(p => p.Id == cartItem.ProductId).FirstOrDefault();
            if (cartItem.ProductId == null || prod == null)
            {
                return Json(new { success = false, message = "Invalid ProductId" });
            }

            if (cartItem.quantity == null || prod.Quantity < cartItem.quantity)
            {
                return Json(new { success = false, message = "Invalid quantity or out of stock"});
            }
            if (userCartItem == null)
            {
                var newCartItem = new CartItem();
                newCartItem.ProductId = cartItem.ProductId;
                newCartItem.UserId = user.Id;
                newCartItem.quantity = cartItem.quantity;
                _context.CartItems.Add(newCartItem);
            } else
            {
                userCartItem.quantity += cartItem.quantity;
                if (userCartItem.quantity > prod.Quantity)
                {
                    userCartItem.quantity = prod.Quantity;
                }
                _context.Update(userCartItem);
            }

            await _context.SaveChangesAsync();

            return Json(new { success = true, message = "Already Added to Cart." }) ;
        }

        [HttpPost]
        public async Task<IActionResult> RemoveItem(long productId)
        {
            var user = _context.Users.Where(u => u.Email == User.Identity.Name).FirstOrDefault();
            var userCartItem = _context.CartItems
                .Where(c => c.UserId == user.Id)
                .Where(c => c.ProductId == productId)
                .FirstOrDefault();
            _context.CartItems.Remove(userCartItem);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }


        [HttpPost]
        public async Task<IActionResult> UpdateSingleCartItem([FromBody] CartItem cartItem)
        {
            var user = _context.Users.Where(u => u.Email == User.Identity.Name).FirstOrDefault();
            var userCartItem = _context.CartItems
                .Where(c => c.UserId == user.Id)
                .Where(c => c.ProductId == cartItem.ProductId)
                .FirstOrDefault();
            if (cartItem == null)
            {
                return Json(new { success = false, message = "Bad Request!" });
            }
            var prod = _context.Products.Where(p => p.Id == cartItem.ProductId).FirstOrDefault();
            if (cartItem.ProductId == null || prod == null)
            {
                return Json(new { success = false, message = "Invalid ProductId" });
            }

            if (cartItem.quantity == null || prod.Quantity < cartItem.quantity)
            {
                return Json(new { success = false, message = "Invalid quantity or out of stock" });
            }
            if (userCartItem == null)
            {
                var newCartItem = new CartItem();
                newCartItem.ProductId = cartItem.ProductId;
                newCartItem.UserId = user.Id;
                newCartItem.quantity = cartItem.quantity;
                _context.CartItems.Add(newCartItem);
            }
            else
            {
                userCartItem.quantity = cartItem.quantity;
                if (userCartItem.quantity > prod.Quantity)
                {
                    userCartItem.quantity = prod.Quantity;
                }
                _context.Update(userCartItem);
            }

            await _context.SaveChangesAsync();

            return Json(new { success = true, message = "Already Added to Cart." });
        }

        public IActionResult Checkout()
        {
            var vm = new CheckoutViewModel();
            var user = _context.Users.Where(u => u.Email == User.Identity.Name).FirstOrDefault();
            var cartItems = _context.CartItems
                    .Include(c => c.Product)
                    .Include(c => c.Product.Brand)
                    .Include(c => c.Product.Film)
                    .Where(c => c.UserId == user.Id).ToList();
            vm.cartItems = cartItems;
            vm.Email = User.Identity.Name;
            return View(vm);
        }
        [HttpPost]
        public async Task<IActionResult> MakeCheckout(
            [FromForm] Order order)
        {
            var user = _context.Users.Where(u => u.Email == User.Identity.Name).FirstOrDefault();
            order.UserId = user.Id;
            _context.Orders.Add(order);
            await _context.SaveChangesAsync();
            var cartItems = _context.CartItems
                    .Include(c => c.Product)
                    .Include(c => c.Product.Brand)
                    .Include(c => c.Product.Film)
                    .Where(c => c.UserId == user.Id).ToList();
            foreach (var item in cartItems)
            {
                var orderDetails = new OrderDetail();
                orderDetails.ProductId = item.ProductId;
                orderDetails.Quantity = item.quantity;
                orderDetails.OrderId = order.Id;
                _context.OrderDetails.Add(orderDetails);
                _context.CartItems.Remove(item);
            }
            await _context.SaveChangesAsync();

            return Redirect("/Identity/Account/Manage/ListOrder");
        }
    }
}
