using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dotnet.Models
{
    public class UserCart
    {
        public int Id { get; set; }
        public string UserId { get; set; }  // Link to the user
        public IdentityUser User { get; set; }  // Navigation property to User

        public List<UserCartItem> Items { get; set; }  // List of cart items
    }

    public class UserCartItem
    {
        public int Id { get; set; }
        public int ProductId { get; set; }  // Link to the product
        public Product Product { get; set; }  // Navigation property to Product

        public int Quantity { get; set; }  // Quantity of the product in the cart
    }

}
