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
    public class BlogsController : BaseController
    {
        // private readonly ApplicationDbContext _context;
        private static int currentStore = 0;

        //public BlogsController(ApplicationDbContext context)
        //{
        //    _context = context;
        //}

        public BlogsController(ApplicationDbContext context, IDataProtectionProvider provider, IHttpContextAccessor httpContextAccessor, IWebHostEnvironment _environment) : base(context, provider, httpContextAccessor, _environment)
        {
        }

        // GET: Blogs
        public async Task<IActionResult> Index(int StoreId)
        {
            currentStore = StoreId;
            var blogs = await _context.Blogs.Where(a => a.StoreId.Equals(StoreId)).ToListAsync();
            return View(blogs);
        }
        // GET: Blogs/Details
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
        public IActionResult Create()
        {
            ViewData["StoreId"] = new SelectList(_context.Stores.Where(a => a.StoreId == currentStore), "StoreId", "StoreName");

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(int StoreId, IFormFile BlogFile, [Bind("BlogId,StoreId,BlogImage,BlogTitle,BlogCreatedDate,BlogModifiedDate,BlogDescription")] Blogs blogs)
        {
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
            ViewData["StoreId"] = new SelectList(_context.Stores.Where(a => a.StoreId == currentStore), "StoreId", "StoreName");

            if (blogs == null)
            {
                return NotFound();
            }
            return View(blogs);
        }

        // POST: Blogs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
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