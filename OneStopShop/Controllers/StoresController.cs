﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using OneStopShop.Models;

namespace OneStopShop.Controllers
{
    public class StoresController : Controller
    {
        private readonly ApplicationDbContext _context;

        public StoresController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Stores/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Stores/Create

        [HttpPost]
        public IActionResult Create([Bind("StoreId,StoreName,SellerFirstname,SellerLasttname,StoreDescription,PhoneNumber,Email")] Store store)
        {
            if (ModelState.IsValid)
            {
                _context.Add(store);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            return View(store);
        }


        // GET:List of Stores
        public  IActionResult Index()
        {
            return View( _context.Stores.ToList());
        }
        public IActionResult Details(int id)
        {
            return RedirectToAction("Index", "Products", new { ID = id });
        }


    }
}




