using DotnetMastery.DataAccess.Data;
using Dotnet.Models;
using DotnetMastery.DataAccess.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Dotnet.Controllers
{
    [Area("Customer")]
    public class CartController : Controller
    {
        private readonly CartService _cartService;
        private readonly UserManager<IdentityUser> _userManager;  // Use IdentityUser here
        private readonly ApplicationDbContext _context;

        public CartController(CartService cartService, UserManager<IdentityUser> userManager, ApplicationDbContext context)
        {
            _cartService = cartService;
            _userManager = userManager;  // Use IdentityUser here
            _context = context;
        }

        public async Task<IActionResult> AddToCart(int id, int quantity = 1)
        {
            var user = await _userManager.GetUserAsync(User);  // Ensure correct user fetch
            if (user == null)
            {
                return RedirectToAction("Login", "Account");
            }

            await _cartService.AddToCartAsync(user, id, quantity);

            return RedirectToAction("ViewCart");
        }

        public async Task<IActionResult> ViewCart()
        {
            var user = await _userManager.GetUserAsync(User);  // Ensure correct user fetch
            if (user == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var cart = await _context.UserCarts
                .Include(c => c.Items)
                .ThenInclude(i => i.Product)
                .FirstOrDefaultAsync(c => c.UserId == user.Id);

            var items = cart?.Items ?? new List<UserCartItem>();

            return View(items);
        }
        public async Task<IActionResult> RemoveFromCart(int productId)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToAction("Login", "Account");
            }

            // Find the user's cart
            var userCart = await _context.UserCarts
                .Include(c => c.Items)
                .FirstOrDefaultAsync(c => c.UserId == user.Id);

            if (userCart != null)
            {
                // Find the cart item
                var itemToRemove = userCart.Items.FirstOrDefault(i => i.ProductId == productId);

                if (itemToRemove != null)
                {
                    // Remove the item from the cart
                    userCart.Items.Remove(itemToRemove);
                    await _context.SaveChangesAsync();
                }
            }

            return RedirectToAction("ViewCart");
        }
        public async Task<JsonResult> GetCartCount()
        {
            var user = await _userManager.GetUserAsync(User);
            int cartCount = 0;

            if (user != null)
            {
                var userCart = await _context.UserCarts
                    .Include(c => c.Items)
                    .FirstOrDefaultAsync(c => c.UserId == user.Id);

                cartCount = userCart?.Items.Sum(i => i.Quantity) ?? 0;
            }

            return Json(new { cartCount });
        }
        
        
        [HttpGet]
        public async Task<IActionResult> PayWithKhalti()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
                return RedirectToAction("Login", "Account");

            var cart = await _context.UserCarts
                .Include(c => c.Items)
                .ThenInclude(i => i.Product)
                .FirstOrDefaultAsync(c => c.UserId == user.Id);

            if (cart == null || !cart.Items.Any())
                return RedirectToAction("ViewCart");

            var totalAmount = cart.Items.Sum(i => i.Product.Price * i.Quantity);

            ViewBag.TotalAmount = totalAmount;
            return View(cart.Items);
        }




    }
}
