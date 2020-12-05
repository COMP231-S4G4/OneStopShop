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
        private static int currentStore = 0;

        public ProductsController(ApplicationDbContext context, IDataProtectionProvider provider, IHttpContextAccessor httpContextAccessor, IWebHostEnvironment _environment) : base(context, provider, httpContextAccessor, _environment)
        {
        }

        // GET: Products/Index
        /// <summary>
        /// This action will get triggered when user will click on Products button on Dashboard
        /// This action will pass all the products to the Index view 
        /// User will be able to see the list products
        /// </summary>
        /// <returns>User will get list of products</returns>
        public async Task<IActionResult> Index(int id = 0)
        {
            if (id == 0)
            {
                return NotFound();
            }
            currentStore = id;
            var products = await _context.Products.Where(i => i.StoreId.Equals(id)).Include(a => a.store).ToListAsync();

            return View(products);
        }

        public IActionResult Back()
        {
            return RedirectToAction("Index", new { id = currentStore });
        }

        public IActionResult Dashboard()
        {
            return RedirectToAction("Dashboard", "Stores", new { id = currentStore });
        }

        // GET: Products/Details/5
        /// <summary>
        /// This action gets triggered when user clicks on the details button attached to each product
        /// details of a particular product is searched for bases on product id
        /// </summary>
        /// <returns>returns the details of a particular product</returns>
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
        /// <summary>
        /// This action will get triggered when user/seller will click on Create Product button
        /// </summary>
        /// <returns>Seller will get Create Product form with required fields</returns>
        public IActionResult Create()
        {
            ViewData["StoreId"] = new SelectList(_context.Stores.Where(a => a.StoreId == currentStore), "StoreId", "StoreName");
            return View();
        }

        // Post: Products/Create
        /// <summary>
        /// This action will get triggered when user/seller will click on Submit button on create product form
        /// This action will pass all the details into the database with the information that the seller has provided
        /// Seller will be able to add all the product information
        /// </summary>
        /// <returns>Seller will post a new product with the information that he provided while creating the product</returns> 

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(IFormFile EventBannerFile, int StoreId, [Bind("ProductID,StoreID,ProductName,ProductDescription,ProductPrice,ProductCreatedDate,ProductModifiedDate,ProductImage,ProductSize,ProductColor")] Product product)
        {
            if (ModelState.IsValid)
            {
                if (EventBannerFile != null)
                {
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
                product.ProductCreatedDate = DateTime.Now;
                product.ProductModifiedDate = DateTime.Now;
                product.StoreId = StoreId;
                product.IsAddedToCart = false;
                _context.Add(product);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction("Index", "Products", new { id = StoreId });
        }

        /// <summary>
        /// This action gets triggered when user clicks on the add to cart button
        /// a particular product is added to the cart based on its product ID.
        /// </summary>
        /// <returns>displas the cart page with newly added item and existing itmes</returns>
        public async Task<RedirectToActionResult> AddToCartAsync(int productId)
        {
            var product = _context.Products.Where(a => a.ProductID.Equals(productId)).FirstOrDefault();
            product.IsAddedToCart = true;
            _context.Update(product);
                       
            await _context.SaveChangesAsync();


            return RedirectToAction("Index", "Cart");
        }

        public async Task<RedirectToActionResult> RemoveCartItemAsync(int id)
        {
            var product = _context.Products.Where(a => a.ProductID.Equals(id)).FirstOrDefault();
            product.IsAddedToCart = false;
            _context.Update(product);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index", "Cart");
        }

        // GET: Products/Edit
        /// <summary>
        /// This action will get triggered when user/seller will click on Edit product button
        /// This action will display the edit product page
        /// </summary>
        /// <returns>Seller will get Edit product form with all the information for that particular product and editable fields</returns>
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

        }

        // Post: Products/Edit
        /// <summary>
        /// This action will get triggered when user/seller will click on Update button on Edit product form
        /// This action will fetch all the details from the database with the editable fields
        /// Seller will be able to edit all the product fields
        /// </summary>
        /// <returns>Seller will get an updated product with the information that he provided while editing the product</returns>
        /// 
        [HttpPost]
        public IActionResult Edit(int id, IFormFile EventBannerFile, [Bind("ProductID,StoreId,ProductName,ProductDescription,ProductPrice,ProductModifiedDate,ProductSize,ProductColor,ProductImage")] Product product)
        {
            if (ModelState.IsValid)
            {
                product.ProductModifiedDate = DateTime.Now;

                if (EventBannerFile != null)
                {
                    string wwwPath = this.Environment.WebRootPath;
                    string contentPath = this.Environment.ContentRootPath;
                    string folderName = "Product" + product.ProductID;
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

                _context.Update(product);
                _context.SaveChanges();

                return RedirectToAction("Index", new { id = product.StoreId });
            }
            return View("Details");
        }
        //Get Product/Delete
        /// <summary>
        /// This action will get triggered when user/seller will click on Delete product button
        /// This action will display the delete product prompt
        /// </summary>
        /// <returns>Seller will get delete product prompt which asks if user wants to delete product</returns>
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
        //Post Product/Delete
        /// <summary>
        /// This action will get triggered when user/seller will click on Yes button on Delete product prompt
        /// Seller will be able to delete the particular product
        /// </summary>
        /// <returns>The product will get deleted, Seller will get an updated list of products</returns>
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var product = await _context.Products.FindAsync(id);
            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
            // return RedirectToAction(nameof(Index));
            return RedirectToAction("Index", new { id = product.StoreId });
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

        /// <summary>
        /// This action gets triggered when user enters a string in searchbox and clicks on search button
        /// First all the products belonging to that particular store is fetched and then it is filtered with the searchstring matching product name or product description.
        /// All products with name or description matching the search string is stored as a new list.
        /// </summary>
        /// <returns>The new filtered product list is returned along with current store details</returns>
        public async Task<IActionResult> ProductSearch(string searchString)
        {
            IList<Product> Produnewlist = null;
            ViewData["CurrentFilter"] = searchString;
            
            var prodlist = from s in _context.Products.Where(i => i.StoreId.Equals(currentStore)) select s;
            if (!String.IsNullOrEmpty(searchString))
            {
                Produnewlist = prodlist.Where(s => s.ProductName.Contains(searchString)
                                       || s.ProductDescription.Contains(searchString)).ToList();
            }
            
            var store = _context.Stores.Find(currentStore);
            var tupleData = new Tuple<IList<OneStopShop.Models.Product>, OneStopShop.Models.Store>(Produnewlist, store);
            return View("ProductList", tupleData);
        }

        public async Task<IActionResult> ViewProduct(int? id)
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

        [HttpPost]
        public IActionResult AddToWishlist(int id)
        {

            Wishlist item = new Wishlist()
            {
                ProductId = id,
                UserId = (int)HttpContext.Session.GetInt32("UserId"),
                IsAddedToWishlist = true
            };
            if (!_context.Wishlists.Any(o => o.ProductId==id))
            {
                _context.Wishlists.Add(item);
                _context.SaveChanges();
            }

            //return View ("WishList");
            return RedirectToAction("Index","Wishlist", new { productID = id });

        }

       
    }
}