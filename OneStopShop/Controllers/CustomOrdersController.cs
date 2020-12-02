﻿using System;
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
    public class CustomOrdersController : BaseController
    {
        //private readonly ApplicationDbContext _context;
        private static int currentStore = 0;

        //public CustomOrdersController(ApplicationDbContext context)
        //{
        //    _context = context;
        //}
        public CustomOrdersController(ApplicationDbContext context, IDataProtectionProvider provider, IHttpContextAccessor httpContextAccessor, IWebHostEnvironment _environment) : base(context, provider, httpContextAccessor, _environment)
        {
        }

        public IActionResult Create(int id)
        {
            currentStore = id;
            ViewData["StoreId"] = new SelectList(_context.Stores.Where(a => a.StoreId == currentStore), "StoreId", "StoreName");

            return View();
        }

        // POST: CustomOrders/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
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
            //return View(customOrders);
            return RedirectToAction("ProductList", "Products", new { id = StoreId });
        }

        // GET: CustomOrders/Details
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