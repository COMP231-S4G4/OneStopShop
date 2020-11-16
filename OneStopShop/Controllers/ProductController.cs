using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using OneStopShop.Models;

namespace OneStopShop.Controllers
{
    public class ProductsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly object protector;
        private static int currentStore = 0;

        public ProductsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Products
        public async Task<IActionResult> Index(int id)
        {
            currentStore = id;
            var products = await _context.Products.Where(i => i.StoreId.Equals(id)).ToListAsync();

            return View(products);
        }

        public async Task<IActionResult> Back()
        {
            return View(await _context.Products.Where(i => i.StoreId.Equals(currentStore)).ToListAsync());
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
        public IActionResult Create(int id)
        {
            ViewData["StoreName"] = new SelectList(_context.Stores.Where(a => a.StoreId == id), "StoreId", "StoreName");
            return View();
        }

        // POST: Products/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(IFormFile EventBannerFile, int StoreId, [Bind("ProductID,StoreID,ProductName,ProductDescription,ProductPrice,ProductCreatedDate,ProductModifiedDate,ProductImage,ProductSize,ProductColor")] Product product)
        {
            if (ModelState.IsValid)
            {
                if (EventBannerFile != null)
                {
                    var fileName = Path.GetFileName(EventBannerFile.FileName);
                    var fileExtension = Path.GetExtension(fileName);
                    var newFileName = String.Concat(Convert.ToString(Guid.NewGuid()), fileExtension);

                    product.StoreId = StoreId;
                    product.ProductImage = fileName;
                    _context.Add(product);
                    await _context.SaveChangesAsync();
                    return RedirectToAction("Details", "Stores", new { id = StoreId });
                }
            }
            return View(product);
        }
    }
}