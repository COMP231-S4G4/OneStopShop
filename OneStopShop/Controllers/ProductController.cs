using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using OneStopShop.Models;
using OneStopShop.Models.ViewModels;

namespace OneStopShop.Controllers
{
    public class ProductsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly object protector;
        private static int currentStore = 0;

        public ProductsController(ApplicationDbContext context, IWebHostEnvironment hostEnvironment)
        {
            _context = context;
            webHostEnvironment = hostEnvironment;
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
        public async Task<IActionResult> Create(ProductImageViewModel model)
        {
            if (ModelState.IsValid)
            {
                string uniqueFileName = UploadedFile(model);

                Product product = new Product
                {
                    ProductName = model.ProductName,
                    ProductDescription = model.ProductDescription,
                    ProductPrice = model.ProductPrice,
                    ProductCreatedDate = model.ProductCreatedDate,
                    ProductModifiedDate = model.ProductModifiedDate,
                    ProductImage = uniqueFileName,
                    ProductSize = model.ProductSize,
                    ProductColor = model.ProductColor,
                };
                product.ProductCreatedDate = DateTime.Now;
                product.ProductModifiedDate = DateTime.Now;
                product.StoreId = currentStore;
                _context.Add(product);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index", "Products", new { id = currentStore });
            }
            return View();
        }
        // GET: Products/Delete/5
        public async Task<IActionResult> Delete(int? id)
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

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var product = await _context.Products.FindAsync(id);
            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProductExists(int id)
        {
            return _context.Products.Any(e => e.ProductID == id);
        }

        private string UploadedFile(ProductImageViewModel model)
		{
            string uniqueFileName = null;

            if(model.ProductImage != null)
			{
                string uploadsFolder = Path.Combine(webHostEnvironment.WebRootPath, "images");
                uniqueFileName = Guid.NewGuid().ToString() + "_" + model.ProductImage.FileName;
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
				{
                    model.ProductImage.CopyTo(fileStream);
				}
			}
            return uniqueFileName;
		}
    }
}