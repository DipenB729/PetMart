using Dotnet.Models;
using DotnetMastery.DataAccess.Data;
using DotnetMastery.DataAccess.Repository;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Dotnet.Controllers
{
    [Area("Customer")]
    public class OrderController : Controller
    {
        private readonly CartService _cartService;
        private readonly UserManager<IdentityUser> _userManager;  // Use IdentityUser here
        private readonly ApplicationDbContext _context;

        public OrderController(CartService cartService, UserManager<IdentityUser> userManager, ApplicationDbContext context)
        {
            _cartService = cartService;
            _userManager = userManager;  // Use IdentityUser here
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> ConfirmOrder([FromBody] dynamic paymentPayload)
        {
            // Extract useful fields like mobile, token, etc.
            string userId = User.Identity.Name; // or get current logged-in user ID
            decimal amount = 0; // You can calculate from cart or get from payload

            var order = new Order
            {
                UserId = userId,
                PaymentId = paymentPayload.idx,
                PaymentStatus = "Paid",
                PaymentMethod = "Khalti",
                OrderDate = DateTime.Now
            };

            _context.Orders.Add(order);
            await _context.SaveChangesAsync();

            return Json(new { status = "success" });
        }
        public async Task<IActionResult> MyOrders()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
                return RedirectToAction("Login", "Account");

            var orders = await _context.Orders
                .
                Include(i => i.PaymentId)
                .Where(o => o.UserId == user.Id)
                .OrderByDescending(o => o.OrderDate)
                .ToListAsync();

            return View(orders);
        }


        public IActionResult ThankYou()
        {
            return View();
        }
    }
}
