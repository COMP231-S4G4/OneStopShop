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

        
        public CartController(ApplicationDbContext context,Cart cartService)
        {
            _context = context;
            cart = cartService;
        }
        public IActionResult Index()
        {
            return View();
        }

        public RedirectToActionResult AddToCart(int productId)
        {
            Product product = _context.Products
                .FirstOrDefault(p => p.ProductID == productId);

            if (product != null)
            {
                cart.AddItem(product, 1);
            }

            return RedirectToAction("Index");

        }
    }
}