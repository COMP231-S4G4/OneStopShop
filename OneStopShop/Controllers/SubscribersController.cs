﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
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
        // Post: Subscriber/JoinStore
        /// <summary>
        /// This action gets triggered when user/buyer will click on Subscribe button on store page
        /// This action passes user details and StoreId into the database
        /// Buyer subscribes to the store
        /// </summary>
        /// <returns>Buyer subscribes to a store</returns>
        public async Task<IActionResult> JoinStore(int StoreId)
        {
            int userID;
            await HttpContext.Session.LoadAsync();

            string userId = HttpContext.Session.GetString("UserId");
            if(userId == null)
            {
                return RedirectToAction("Login", "Users");
            }
            userID = Convert.ToInt32(protector.Unprotect(userId));
            
            //int userID = Convert.ToInt32(userId);
           // int userID = Convert.ToInt32(userId);
            var user = await _context.Users
               .FirstOrDefaultAsync(m => m.UserID == userID);

            if (ModelState.IsValid)
            {
                var IsMember = _context.Subscribers.Where(j => j.StoreId.Equals(StoreId)
             && j.UserId.Equals(userID)).FirstOrDefault();
                if (IsMember == null)
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
                else
                {
                    TempData["ErrorSubscribed"] = $"You have already subscribed  this store";
                }
                return RedirectToAction("ProductList", "Products", new { id = StoreId });
            }
            return View();
        }
    }
}