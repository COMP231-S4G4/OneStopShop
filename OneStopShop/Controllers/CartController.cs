using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OneStopShop.Models;
using OneStopShop.Models.ViewModels;

namespace OneStopShop.Controllers
{
    public class CartController : BaseController
    {
        private Cart cart;
        public CartController(ApplicationDbContext context, IDataProtectionProvider provider, IHttpContextAccessor httpContextAccessor, IWebHostEnvironment _environment, Cart cartService) : base(context, provider, httpContextAccessor, _environment)
        {
            cart = cartService;
        }

        // Cart/Index
        /// <summary>
        /// This action will get triggered when user will click on cart button on Home Page
        /// </summary>
        /// <returns>User will see the cart view</returns>
        public IActionResult Index()
        {
            return View(cart);
        }
        // Cart/GetAddedCartPro
        /// <summary>
        /// This action will get triggered when user will click on cart button on Home Page
        /// This action will pass all the products that the buyer has added to his cart to the view
        /// User will be able to see all the list of items in the cart
        /// </summary>
        /// <returns>User will see the list of items in the cart that the buyer has added to his cart</returns>
        private async Task<List<Product>> GetAddedCartPro()
        {
            var product = _context.Products.Where(a => a.IsAddedToCart.Equals(true)).ToList();

            return product;
        }

        // Cart/AddToCart
        /// <summary>
        /// This action will get triggered when user will click on add to cart button on View Product Page
        /// This action will pass the product and UserId to the cart
        /// User will be able to add a product to his cart
        /// </summary>
        /// <returns>User will be able to add a product to his cart</returns>
        public async Task<RedirectToActionResult> AddToCart(int productId)
        {
            int userID;
            await HttpContext.Session.LoadAsync();

            string userId = HttpContext.Session.GetString("UserId");
            if (userId == null)
            {
                return RedirectToAction("Login", "Users");
            }
            userID = Convert.ToInt32(protector.Unprotect(userId));
            var product = _context.Products
                .FirstOrDefault(p => p.ProductID == productId);
            product.IsAddedToCart = true;

            _context.Update(product);
            _context.SaveChanges();

            var store = _context.Stores
              .FirstOrDefault(p => p.StoreId == productId);

            if (product != null)
            {
                cart.AddItem(product, 1,product.StoreId);
            }
            var a = cart;
            
            return RedirectToAction("Index");
        }

        // Cart/RemoveFromCart
        /// <summary>
        /// This action will get triggered when user will click on remove from cart button on Cart Page
        /// User will be able to removd a product from his cart
        /// </summary>
        /// <returns>User will be able to remove a product from his cart</returns>
        public RedirectToActionResult RemoveFromCart(int id, string returnUrl)
        {
            Product product = _context.Products
                .FirstOrDefault(p => p.ProductID == id);

            if (product != null)
            {
                cart.RemoveLine(product);
            }

            return RedirectToAction("Index", new { returnUrl });
        }
    }
}