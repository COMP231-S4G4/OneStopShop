using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OneStopShop.Models;

namespace OneStopShop.Controllers
{
    public class SubscriberController : BaseController
    {
        private static int currentStore = 0;

        public SubscriberController(ApplicationDbContext context, IDataProtectionProvider provider, IHttpContextAccessor httpContextAccessor, IWebHostEnvironment _environment) : base(context, provider, httpContextAccessor, _environment)
        {
        }

        // GET: SubscriberController
        public async Task<ActionResult> Index(int StoreId)
        {
            currentStore = StoreId;
            var subscribers = await _context.Subscribers.Where(a => a.Store.StoreId.Equals(StoreId)).ToListAsync();
            return View(subscribers);
        }

        // GET: SubscriberController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: SubscriberController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: SubscriberController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: SubscriberController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: SubscriberController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: SubscriberController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: SubscriberController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
