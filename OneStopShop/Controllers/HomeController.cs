﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.DataProtection;
using OneStopShop.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using System.Web;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Ajax.Utilities;

namespace OneStopShop.Controllers
{
    public class HomeController : BaseController
    {
        private readonly ILogger _logger;

        public HomeController(ApplicationDbContext context, IDataProtectionProvider provider, ILogger<HomeController> logger,
            IHttpContextAccessor httpContextAccessor, IWebHostEnvironment _environment) : base(context, provider, httpContextAccessor, _environment)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            var clubs = GetClubsAsync().Result.ToList();

            var events = GetEventsAsync().Result.ToList();
            ViewModel model = new ViewModel();
            model.store = clubs;
            model.product = events;
            return View(model);
        }

        private async Task<List<Store>> GetClubsAsync()
        {
            //var Clubs = await .Where(m => m.IsInitialApprove == true).ToListAsync();
            var stores = await _context.Stores.Where(a => a.Email != null).ToListAsync();
            (from p in stores orderby Guid.NewGuid() select p).Take(5).ToList();
            return stores;
        }

        private async Task<List<Product>> GetEventsAsync()
        {
            var products = _context.Products.ToList();
            return products;
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}