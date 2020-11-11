using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using OneStopShop.Models;

namespace OneStopShop.Controllers
{
    public class ProductController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ProductController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Product/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: products/Create

        [HttpPost]

        public async Task<IActionResult> Create([Bind("ProductId,ProductName,ProductDescription,ProductPrice,ProductCreatedDate,ProductModifiedDate,ProductQuantity,ProductImage,ProductSize,ProductColor")] Product product)
        {
            if (ModelState.IsValid)
            {
                _context.Add(product);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(product);
        }


        // GET:List of products
        public async Task<IActionResult> Index()
        {
            return View(await _context.Products.ToListAsync());
        }



    }
}