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
    /// <summary>
    /// This controller has the actions where Seller is able to Create,Edit,Delete the blogs and Buyer is able to see the details of all the blogs.
    /// </summary>
    public class BlogsController : BaseController
    {
        private static int currentStore = 0;
        public BlogsController(ApplicationDbContext context, IDataProtectionProvider provider, IHttpContextAccessor httpContextAccessor, IWebHostEnvironment _environment) : base(context, provider, httpContextAccessor, _environment)
        {
        }

        // GET: Blogs
        /// <summary>
        /// This action will get triggered when user/seller will click on Blogs button on Product List Page
        /// This action will pass all the Blogs to the view that the seller has provided
        /// Buyer will be able to see all the list of blogs
        /// </summary>
        /// <returns>Buyer will get list of blogs with the information that seller has provided while creating the blog</returns>

        public async Task<IActionResult> Index(int StoreId = 0)
        {
            if (StoreId == 0)
            {
                return NotFound();
            }
            currentStore = StoreId;
            var blogs = await _context.Blogs.Where(a => a.StoreId.Equals(StoreId)).ToListAsync();
            return View(blogs);
        }
        /// <summary>
        /// This action will get triggered when user/seller will click on Back to list button on Create Blog page
        /// This action passes the current storeId to Index action
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> Back()
        {
            var blogs = await _context.Blogs.Where(a => a.StoreId.Equals(currentStore)).ToListAsync();
            return RedirectToAction("Index", new { StoreId = currentStore });
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var blogs = await _context.Blogs
                .FirstOrDefaultAsync(m => m.BlogId == id);
            if (blogs == null)
            {
                return NotFound();
            }

            return View(blogs);
        }

        // GET: Blogs/Create
        /// <summary>
        /// This action will get triggered when user/seller will click on Create blog button
        /// </summary>
        /// <returns>Seller will get Create blog form with required fields</returns>
        public IActionResult Create()
        {
            //This viewbag passes current storeId to the create view
            ViewData["StoreId"] = new SelectList(_context.Stores.Where(a => a.StoreId == currentStore), "StoreId", "StoreName");
            ViewData["Store"] = new SelectList(_context.Stores.Where(a => a.StoreId == currentStore), "StoreId");

            return View();
        }

        // Post: Blogs/Create
        /// <summary>
        /// This action will get triggered when user/seller will click on Submit button on create blog form
        /// This action will pass all the details into the database with the information that the seller has provided
        /// Seller will be able to add all the blog information
        /// </summary>
        /// <returns>Seller will get a new blog with the information that he provided while creating the blog</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(int StoreId, IFormFile BlogFile, [Bind("BlogId,StoreId,BlogImage,BlogTitle,BlogCreatedDate,BlogModifiedDate,BlogDescription")] Blogs blogs)
        {
            ViewData["StoreId"] = new SelectList(_context.Stores.Where(a => a.StoreId == currentStore), "StoreId", "StoreName");

            if (ModelState.IsValid)
            {
                if (BlogFile != null)
                {
                    string wwwPath = this.Environment.WebRootPath;
                    string contentPath = this.Environment.ContentRootPath;
                    string folderName = "Product" + blogs.BlogId;
                    string path = Path.Combine(this.Environment.WebRootPath, "Upload/Events/" + folderName);

                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }
                    string fileName = Path.GetFileNameWithoutExtension(BlogFile.FileName);
                    string extension = Path.GetExtension(BlogFile.FileName);
                    string fileNameBanner = fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;

                    string folderPath = "Upload/Events/" + folderName;
                    blogs.BlogImage = folderPath + '/' + fileNameBanner;

                    var filepath = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", folderPath)).Root + $@"{fileNameBanner}";
                    using (FileStream fs = System.IO.File.Create(filepath))
                    {
                        BlogFile.CopyTo(fs);
                        fs.Flush();
                    }
                }
                blogs.BlogCreatedDate = DateTime.Now;
                blogs.BlogModifiedDate = DateTime.Now;
                blogs.StoreId = StoreId;
                _context.Add(blogs);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction("Index", new { StoreId = StoreId });
        }

        // GET: Blogs/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var blogs = await _context.Blogs.FindAsync(id);

            //This viewbag passes current storeId to the edit view
            ViewData["StoreId"] = new SelectList(_context.Stores.Where(a => a.StoreId == currentStore), "StoreId", "StoreName");

            if (blogs == null)
            {
                return NotFound();
            }
            return View(blogs);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, IFormFile BlogFile, [Bind("BlogId,StoreId,BlogImage,BlogTitle,BlogCreatedDate,BlogModifiedDate,BlogDescription")] Blogs blogs)
        {
            if (id != blogs.BlogId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    if (BlogFile != null)
                    {
                        string wwwPath = this.Environment.WebRootPath;
                        string contentPath = this.Environment.ContentRootPath;
                        string folderName = "Product" + blogs.BlogId;
                        string path = Path.Combine(this.Environment.WebRootPath, "Upload/Events/" + folderName);

                        if (!Directory.Exists(path))
                        {
                            Directory.CreateDirectory(path);
                        }
                        string fileName = Path.GetFileNameWithoutExtension(BlogFile.FileName);
                        string extension = Path.GetExtension(BlogFile.FileName);
                        string fileNameBanner = fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;

                        string folderPath = "Upload/Events/" + folderName;
                        blogs.BlogImage = folderPath + '/' + fileNameBanner;

                        var filepath = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", folderPath)).Root + $@"{fileNameBanner}";
                        using (FileStream fs = System.IO.File.Create(filepath))
                        {
                            BlogFile.CopyTo(fs);
                            fs.Flush();
                        }
                    }
                    blogs.BlogCreatedDate = blogs.BlogCreatedDate;
                    blogs.BlogModifiedDate = DateTime.Now;
                    _context.Update(blogs);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BlogsExists(blogs.BlogId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Index", new { StoreId = blogs.StoreId });
            }
            return View(blogs);
        }

        // GET: Blogs/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var blogs = await _context.Blogs
                .FirstOrDefaultAsync(m => m.BlogId == id);
            if (blogs == null)
            {
                return NotFound();
            }

            return View(blogs);
        }

        // POST: Blogs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var blogs = await _context.Blogs.FindAsync(id);
            _context.Blogs.Remove(blogs);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index", new { StoreId = blogs.StoreId });
        }

        private bool BlogsExists(int id)
        {
            return _context.Blogs.Any(e => e.BlogId == id);
        }
    }
}