using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using OneStopShop.Models;

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
            var addedPro = GetAddedCartPro().Result.ToList();

            ViewModel model = new ViewModel();
            model.product = addedPro;
            return View(model);
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
        public RedirectToActionResult AddToCart(int productId)
        {
            var product = _context.Products
                .FirstOrDefault(p => p.ProductID == productId);

            if (product != null)
            {
                cart.AddItem(product, 1);
            }

            return RedirectToAction("Index", new { id = product.ProductID });
        }
    }
}