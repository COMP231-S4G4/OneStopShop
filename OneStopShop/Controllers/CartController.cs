using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using OneStopShop.Models;
using OneStopShop.Models.ViewModels;

namespace OneStopShop.Controllers
{
    public class CartController : Controller
    {
        private readonly ApplicationDbContext _context;
        private Cart cart;

        public CartController(ApplicationDbContext context, Cart cartService)
        {
            _context = context;
            cart = cartService;
        }

        public IActionResult Index()
        {
            return View(cart);
        }

        private async Task<List<Product>> GetAddedCartPro()
        {
            var product = _context.Products.Where(a => a.IsAddedToCart.Equals(true)).ToList();

            return product;
        }

        //private async Task<List<Product>> GetProducts()
        //{
        //    var products = _context.Products.Where(a => a.ProductID.Equals(id)).FirstOrDefault();
        //}

        //[HttpPost]
        //public async Task<IActionResult> totalquantity(int quntity, int productid)
        //{
        //    var product = _context.Products
        //      .FirstOrDefault(p => p.ProductID == productid);
        //    var price = product.ProductPrice;
        //    var total = price * quntity;
        //    product.ProductPrice = total;
        //    _context.Update(product);
        //    await _context.SaveChangesAsync();

        //    return RedirectToAction("Index", new { id = product.ProductID });
        //}

        public RedirectToActionResult AddToCart(int productId)
        {
            var product = _context.Products
                .FirstOrDefault(p => p.ProductID == productId);
            product.IsAddedToCart = true;

            _context.Update(product);
            _context.SaveChanges();

            var store = _context.Stores
              .FirstOrDefault(p => p.StoreId == productId);

            if (product != null)
            {
                cart.AddItem(product, 1);
            }

            return RedirectToAction("Index");
        }

        public RedirectToActionResult RemoveFromCart(int productId, string returnUrl)
        {
            Product product = _context.Products
                .FirstOrDefault(p => p.ProductID == productId);

            if (product != null)
            {
                cart.RemoveLine(product);
            }

            return RedirectToAction("Index", new { returnUrl });
        }
    }
}