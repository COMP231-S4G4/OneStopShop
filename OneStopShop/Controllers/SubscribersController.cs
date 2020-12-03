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
    public class SubscribersController : BaseController
    {
        public SubscribersController(ApplicationDbContext context, IDataProtectionProvider provider, IHttpContextAccessor httpContextAccessor, IWebHostEnvironment _environment) : base(context, provider, httpContextAccessor, _environment)
        {
        }
        public async Task<IActionResult> Index(int StoreId)
        {
            var subs = await _context.Subscribers.Where(i => i.StoreId.Equals(StoreId)).ToListAsync();
            var tupleData = new Tuple<IList<OneStopShop.Models.Subscribers>, int>(subs, StoreId);
            return View(tupleData);
        }

        public async Task<IActionResult> JoinStore(int StoreId)
        {
            await HttpContext.Session.LoadAsync();
            int userID = (int)HttpContext.Session.GetInt32("UserId");
            var user = await _context.Users
               .FirstOrDefaultAsync(m => m.UserID == userID);
            if (ModelState.IsValid)
            {
                Subscribers joined = new Subscribers();
                joined.StoreId = StoreId;
                joined.UserId = userID;
                joined.Username = user.Username;
                joined.email = user.email;
                joined.IsOwner = false;
                _context.Add(joined);
                await _context.SaveChangesAsync();
                return RedirectToAction("ProductList", "Products", new { id = StoreId });
            }
            return View();
        }
    }
}