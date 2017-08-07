using BethanysPieShop.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BethanysPieShop.Models
{
    public class ShoppingCart
    {
        private readonly AppDbContext _appDbContext;

        public string ShoppingCartId { get; set; }

        public List<ShoppingCartItem> ShoppingCartItems { get; set; }



        private ShoppingCart(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public static ShoppingCart GetCart(IServiceProvider services)
        {
            // retrieve session from current http context
            ISession session = services.GetRequiredService<IHttpContextAccessor>()?
                .HttpContext.Session;

            var context = services.GetService<AppDbContext>();
            // Try to extract cart id from current session if it exists or generate a new one
            string cartId = session.GetString("CartId") ?? Guid.NewGuid().ToString();
            // tie session to the ShoppingCart
            session.SetString("CartId", cartId);

            return new ShoppingCart(context) { ShoppingCartId = cartId };
        }

        public void AddToCart(Pie pie, int amount)
        {
            // try to get shopping cart item (if it exists already) that is associated with current Shopping cart by id
            var shoppingCartItem =
                    _appDbContext.ShoppingCartItems.SingleOrDefault(
                        item => item.Pie.PieId == pie.PieId && item.ShoppingCartId == ShoppingCartId);

            if (shoppingCartItem == null)
            {
                // Shopping Cart Item doesn't exist yet. creating a new one
                shoppingCartItem = new ShoppingCartItem
                {
                    ShoppingCartId = ShoppingCartId,
                    Pie = pie,
                    Amount = amount
                };

                _appDbContext.ShoppingCartItems.Add(shoppingCartItem);
            }
            else
            {
                // shopping cart item for selected pies already exists. just increase pie count
                shoppingCartItem.Amount += amount;
            }
            _appDbContext.SaveChanges();
        }

        /// <summary>
        /// Removes specified pie from the cart (decreases amount property or removes record completely from the database)
        /// </summary>
        /// <param name="pie"></param>
        /// <returns>amount of pie of that type left in the cart</returns>
        public int RemoveFromCart(Pie pie)
        {
            var shoppingCartItem =
                    _appDbContext.ShoppingCartItems.SingleOrDefault(
                        s => s.Pie.PieId == pie.PieId && s.ShoppingCartId == ShoppingCartId);

            var localAmount = 0;

            if (shoppingCartItem != null)
            {
                if (shoppingCartItem.Amount > 1)
                {
                    shoppingCartItem.Amount--;
                    localAmount = shoppingCartItem.Amount;
                }
                else
                {
                    // was last item in basket. remove whole shopping cart item
                    _appDbContext.ShoppingCartItems.Remove(shoppingCartItem);
                }

                _appDbContext.SaveChanges();
            }

            return localAmount;
        }

        public List<ShoppingCartItem> GetShoppingCartItems()
        {
            // return stored ShoppingCartItems or fetch and return them from the database
            return ShoppingCartItems ??
                   (ShoppingCartItems =
                       _appDbContext.ShoppingCartItems.Where(c => c.ShoppingCartId == ShoppingCartId)
                           .Include(s => s.Pie)
                           .ToList());
        }

        public void ClearCart()
        {
            var cartItems = _appDbContext
                .ShoppingCartItems
                .Where(cart => cart.ShoppingCartId == ShoppingCartId);

            _appDbContext.ShoppingCartItems.RemoveRange(cartItems);

            _appDbContext.SaveChanges();
        }

        public decimal GetShoppingCartTotal()
        {
            var total = _appDbContext.ShoppingCartItems.Where(c => c.ShoppingCartId == ShoppingCartId)
                .Select(c => c.Pie.Price * c.Amount).Sum();
            return total;
        }
    }
}
