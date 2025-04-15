using Dotnet.Models;
using DotnetMastery.DataAccess.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotnetMastery.DataAccess.Repository
{
    public class CartService
    {
        private readonly ApplicationDbContext _context;

        public CartService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task AddToCartAsync(IdentityUser user, int productId, int quantity)
        {
            var userCart = await _context.UserCarts
                .Include(c => c.Items)
                .FirstOrDefaultAsync(c => c.UserId == user.Id);

            if (userCart == null)
            {
                userCart = new UserCart
                {
                    UserId = user.Id,
                    Items = new List<UserCartItem>()
                };
                _context.UserCarts.Add(userCart);
            }

            var existingItem = userCart.Items.FirstOrDefault(i => i.ProductId == productId);
            if (existingItem != null)
            {
                existingItem.Quantity += quantity;
            }
            else
            {
                var product = await _context.Products.FindAsync(productId);
                if (product != null)
                {
                    userCart.Items.Add(new UserCartItem
                    {
                        ProductId = productId,
                        Product = product,
                        Quantity = quantity
                    });
                }
            }

            await _context.SaveChangesAsync();
        }
    }
}
