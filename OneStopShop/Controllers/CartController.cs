﻿using System;
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

       
        //Add products to cart
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