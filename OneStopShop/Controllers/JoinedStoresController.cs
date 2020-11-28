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
    public class JoinedStoresController : BaseController
    {
        public JoinedStoresController(ApplicationDbContext context, IDataProtectionProvider provider, IHttpContextAccessor httpContextAccessor, IWebHostEnvironment _environment) : base(context, provider, httpContextAccessor, _environment)
        {
        }
        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> JoinStore(int StoreId)
        {
            await HttpContext.Session.LoadAsync();
            int userID = (int)HttpContext.Session.GetInt32("UserId");
            if (ModelState.IsValid)
            {
                JoinedStore joined = new JoinedStore();
                joined.StoreId = StoreId;
                joined.UserId = userID;
                joined.IsOwner = false;
                _context.Add(joined);
                await _context.SaveChangesAsync();
                return RedirectToAction("ProductList", "Product", new { id = StoreId });
            }
            return View();
        }
    }
}
