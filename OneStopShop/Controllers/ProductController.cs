using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using OneStopShop.Models;

namespace OneStopShop.Controllers
{
    public class ProductsController : BaseController
    {
        //private readonly ApplicationDbContext _context;
        //private readonly object protector;
        //private static int currentStore = 0;

        //public ProductsController(ApplicationDbContext context, IWebHostEnvironment hostEnvironment)
        //{
        //    _context = context;
        //    webHostEnvironment = hostEnvironment;
        //}

        private static int currentStore = 0;

        public ProductsController(ApplicationDbContext context, IDataProtectionProvider provider, IHttpContextAccessor httpContextAccessor, IWebHostEnvironment _environment) : base(context, provider, httpContextAccessor, _environment)
        {
        }

        // GET: Products
        public async Task<IActionResult> Index(int id)
        {
            currentStore = id;
            var products = await _context.Products.Where(i => i.StoreId.Equals(id)).Include(a => a.store).ToListAsync();

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
        public IActionResult Create()
        {
            ViewData["StoreId"] = new SelectList(_context.Stores.Where(a => a.StoreId == currentStore), "StoreId", "StoreName");
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
                    //var fileName = Path.GetFileName(EventBannerFile.FileName);
                    //var fileExtension = Path.GetExtension(fileName);
                    //var newFileName = String.Concat(Convert.ToString(Guid.NewGuid()), fileExtension);

                    string wwwPath = this.Environment.WebRootPath;
                    string contentPath = this.Environment.ContentRootPath;
                    string folderName = "Product" + StoreId;
                    string path = Path.Combine(this.Environment.WebRootPath, "Upload/Events/" + folderName);

                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }
                    string fileName = Path.GetFileNameWithoutExtension(EventBannerFile.FileName);
                    string extension = Path.GetExtension(EventBannerFile.FileName);
                    string fileNameBanner = fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;

                    string folderPath = "Upload/Events/" + folderName;
                    product.ProductImage = folderPath + '/' + fileNameBanner;

                    var filepath = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", folderPath)).Root + $@"{fileNameBanner}";
                    using (FileStream fs = System.IO.File.Create(filepath))
                    {
                        EventBannerFile.CopyTo(fs);
                        fs.Flush();
                    }
                }
                product.StoreId = StoreId;

                _context.Add(product);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction("Index", "Products", new { id = StoreId });
        }

        // GET: Products/Delete/5
        public async Task<IActionResult> Delete(int? id, int StoreId)
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
            //return RedirectToAction("Index", "Products", new { id = StoreId });
        }

        private bool ProductExists(int id)
        {
            return _context.Products.Any(e => e.ProductID == id);
        }

        public RedirectToActionResult AddToCart(int productId)
        {
            var product = _context.Products.Where(a => a.ProductID.Equals(productId)).FirstOrDefault();
            product.IsAddedToCart = true;
            _context.Update(product);
            _context.SaveChangesAsync();

            return RedirectToAction("Index", "Cart");
        }

        public RedirectToActionResult RemoveCartItem(int id)
        {
            var product = _context.Products.Where(a => a.ProductID.Equals(id)).FirstOrDefault();
            product.IsAddedToCart = false;
            _context.Update(product);
            _context.SaveChangesAsync();

            return RedirectToAction("Index", "Cart");
        }

        //      private string UploadedFile(ProductImageViewModel model)
        //{
        //          string uniqueFileName = null;

        //          if(model.ProductImage != null)
        //	{
        //              string uploadsFolder = Path.Combine(webHostEnvironment.WebRootPath, "images");
        //              uniqueFileName = Guid.NewGuid().ToString() + "_" + model.ProductImage.FileName;
        //              string filePath = Path.Combine(uploadsFolder, uniqueFileName);
        //              using (var fileStream = new FileStream(filePath, FileMode.Create))
        //		{
        //                  model.ProductImage.CopyTo(fileStream);
        //		}
        //	}
        //          return uniqueFileName;
        //}
    }
}