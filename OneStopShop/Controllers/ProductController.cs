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
    public class ProductsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private static string currentStore = "nil";

        public ProductsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Products
        public async Task<IActionResult> Index(string id)
        {
            currentStore = id;
            return View(await _context.Products.Where(i => i.StoreID.Equals(id)).ToListAsync());
        }

        // GET: Products/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products
                .FirstOrDefaultAsync(m => m.ProductID == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // GET: Products/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Products/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ProductID,StoreID,ProductName,ProductDescription,ProductPrice,ProductCreatedDate,ProductModifiedDate,ProductImage,ProductSize,ProductColor")] Product product)
        {
            if (ModelState.IsValid)
            {
                product.StoreID = currentStore;
                _context.Add(product);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index","Products", new {id = currentStore});
            }
            return View(product);
        }
    }
}
