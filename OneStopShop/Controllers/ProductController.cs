using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using OneStopShop.Models;
using OneStopShop.Models.ViewModels;

namespace OneStopShop.Controllers
{
    public class ProductsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment webHostEnvironment;
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
            return View(await _context.Products.Where(i => i.StoreId.Equals(id)).ToListAsync());
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

            if (model.ProductImage != null)
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
        // GET: Product/Edit
        public IActionResult Edit(int id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = _context.Products.Find(id);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);

            //return RedirectToAction("Edit", new { id = product.StoreId });
        }

        // POST: Product/Edit

        [HttpPost]
        public IActionResult Edit(int id, [Bind("ProductID,StoreId,ProductName,ProductDescription,ProductPrice,ProductModifiedDate,ProductSize,ProductColor,ProductImage")] Product product)
        {
            if (ModelState.IsValid)
            {
                product.ProductModifiedDate = DateTime.Now;
                _context.Update(product);
                _context.SaveChanges();

                return RedirectToAction("Index", new { id = product.StoreId });
            }
            return View("Details");
        }
        public async Task<IActionResult> ProductList(int id)
        {
            if(id == 0)
            {
                id = currentStore;
            }
            currentStore = id;
            var productlist = await _context.Products.Where(i => i.StoreId.Equals(id)).ToListAsync();
            var store = _context.Stores.Find(id);            
            var tupleData = new Tuple<IList<OneStopShop.Models.Product>, OneStopShop.Models.Store>(productlist, store);
            return View("ProductList", tupleData);
        }


        public async Task<IActionResult> ProductSearch(string searchString)
        {
            IList<Product> Produnewlist = null;
            ViewData["CurrentFilter"] = searchString;
            
            var prodlist = from s in _context.Products.Where(i => i.StoreId.Equals(currentStore))
                           select s;
            if (!String.IsNullOrEmpty(searchString))
            {
                Produnewlist = prodlist.Where(s => s.ProductName.Contains(searchString)
                                       || s.ProductDescription.Contains(searchString)).ToList();
            }
            
            var store = _context.Stores.Find(currentStore);
            var tupleData = new Tuple<IList<OneStopShop.Models.Product>, OneStopShop.Models.Store>(Produnewlist, store);
            return View("ProductList", tupleData);
        }

        public ActionResult OrderConfirmation()
        {
            return View("OrderConfirmation");
        }
    }
}
