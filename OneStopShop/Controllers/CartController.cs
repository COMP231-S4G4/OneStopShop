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
        //private readonly ApplicationDbContext _context;
        private Cart cart;

        public CartController(ApplicationDbContext context, IDataProtectionProvider provider, IHttpContextAccessor httpContextAccessor, IWebHostEnvironment _environment, Cart cartService) : base(context, provider, httpContextAccessor, _environment)
        {
            cart = cartService;
        }
        //public CartController(ApplicationDbContext context, )
        //{
        //    _context = context;
        //    cart = cartService;
        //}

        public IActionResult Index()
        {
            return View(cart);
        }

        private async Task<List<Product>> GetAddedCartPro()
        {
            var product = _context.Products.Where(a => a.IsAddedToCart.Equals(true)).ToList();

            return product;
        }

       
        //Add products to cart
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

        //Remove Products from Cart
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