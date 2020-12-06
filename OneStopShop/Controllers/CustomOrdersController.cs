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
    /// This controller has the actions where Buyer is able to Create a custom order and Seller is able to see the list, and details of the custom order he received.
    /// </summary>
    public class CustomOrdersController : BaseController
    {
        private static int currentStore = 0;
        public CustomOrdersController(ApplicationDbContext context, IDataProtectionProvider provider, IHttpContextAccessor httpContextAccessor, IWebHostEnvironment _environment) : base(context, provider, httpContextAccessor, _environment)
        {
        }

        // GET: CustomOrder/Create
        /// <summary>
        /// This action will get triggered when user/buyer will click on Custom Order button on Product List Page
        /// This action passes the current Store Name, storeId to the form
        /// The buyer will get the custom order form with the required fields
        /// </summary>
        /// <returns>The buyer will get the custom order form with the required fields</returns>
        public IActionResult Create(int id)
        {
            currentStore = id;
            ViewData["StoreId"] = new SelectList(_context.Stores.Where(a => a.StoreId == currentStore), "StoreId", "StoreName");

            return View();
        }

        // Post: CustomOrder/Create
        /// <summary>
        /// This action will get triggered when user/buyer will click on Submit button on Custom Order form
        /// This action will pass all the details into the database with the information that the buyer has provided
        /// Buyer will be able to add all the custom order information
        /// </summary>
        /// <returns> Buyer will be able to add all the custom order information that is sent to the seller of the store</returns>

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(int StoreId, [Bind("CustomOrderID,Status,Username,AddressLine1,AddressLine2,email,PhoneNum,OrderCreatedDate,Description,ProductType,Chest,Neck,Shoulder,Sleeve,Waist,Hip,InseamLength,FullLength,AnkleLength")] CustomOrders customOrders)
        {
            if (ModelState.IsValid)
            {
                _context.Add(customOrders);
                customOrders.status = true;
                customOrders.StoreId = StoreId;
                customOrders.OrderCreatedDate = DateTime.Now;
                await _context.SaveChangesAsync();
                return RedirectToAction("ProductList", "Products", new { id = StoreId });
            }
            return RedirectToAction("ProductList", "Products", new { id = StoreId });
        }

        public async Task<IActionResult> Index(int id)
        {
            var customOrders = await _context.CustomOrders.Where(i => i.StoreId.Equals(id)).ToListAsync();

            return View(customOrders);
        }

        // GET: CustomOrder/Details
        /// <summary>
        /// This action will get triggered when user/Seller will click on Details Tab on Custom Order Page
        /// Seller will be able to see the custom order form filled by buyer
        /// </summary>
        /// <returns>Seller will get Custom Order details with all the information</returns>
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var customOrders = await _context.CustomOrders
                .FirstOrDefaultAsync(m => m.CustomOrderID == id);
            if (customOrders == null)
            {
                return NotFound();
            }

            return View(customOrders);
        }
    }
}