using BethanysPieShop.Models;
using BethanysPieShop.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BethanysPieShop.ViewComponents
{
    //[ViewComponent]
    //public class ShoppingCartSummary
    //public class ShoppingCartSummaryViewComponent
    public class ShoppingCartSummary : ViewComponent
    {
        // If a ViewComponent has a View, then it should be located as follows:
        // Views/Shared/Components/ShoppingCartSummary/default.cshtml
        // Is invoked as @await Component.InvokeAsync("ShoppingCartSummary")

        private readonly ShoppingCart _shoppingCart;

        public ShoppingCartSummary(ShoppingCart shoppingCart)
        {
            _shoppingCart = shoppingCart;
        }

        // ViewComponent must have a public method Invoke() that is called automatically when ViewComponent is needed
        public IViewComponentResult Invoke()
        {
            var items = _shoppingCart.GetShoppingCartItems();
            // mock data
            //var items = new List<ShoppingCartItem>() { new ShoppingCartItem(), new ShoppingCartItem() };
            _shoppingCart.ShoppingCartItems = items;

            var shoppingCartViewModel = new ShoppingCartViewModel
            {
                ShoppingCart = _shoppingCart,
                ShoppingCartTotal = _shoppingCart.GetShoppingCartTotal()
            };
            return View(shoppingCartViewModel);
        }
    }
}
