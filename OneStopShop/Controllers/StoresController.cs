using System;
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
        public async Task<IActionResult> Create([Bind("StoreId,StoreName,SellerFirstname,SellerLasttname,StoreDescription,PhoneNumber,Email")] Store store)
        {
            if (ModelState.IsValid)
            {
                _context.Add(store);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(store);
        }

        // GET:List of Stores
        public async Task<IActionResult> Index()
        {
            return View(await _context.Stores.ToListAsync());
        }

        public async Task<IActionResult> Details(int id)
        {
            var storeDetails = await _context.Stores.Include(a => a.product).ThenInclude(a => a.ProductID)
               .FirstOrDefaultAsync(m => m.StoreId == id);
            return View(storeDetails);
        }

        // GET: Stores/Edit/id
        public IActionResult Edit(int id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var store = _context.Stores.Find(id);
            if (store == null)
            {
                return NotFound();
            }
            return View(store);
        }

        // POST: Stores/Edit/id

        [HttpPost]
        public IActionResult Edit(int id, [Bind("StoreId,StoreName,SellerFirstname,SellerLasttname,StoreDescription,PhoneNumber,Email")] Store store)
        {
            if (id != store.StoreId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                _context.Update(store);
                _context.SaveChanges();

                return RedirectToAction(nameof(Index));
            }
            return View(store);
        }

        // GET: Stores/Delete/5
        public IActionResult Delete(int id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var store = _context.Stores
                .FirstOrDefault(m => m.StoreId == id);
            if (store == null)
            {
                return NotFound();
            }

            return View(store);
        }

        // POST: Stores/Delete/5
        [HttpPost, ActionName("Delete")]
        public IActionResult DeleteConfirmed(int id)
        {
            var store = _context.Stores.Find(id);
            _context.Stores.Remove(store);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        private bool StoreExists(int id)
        {
            return _context.Stores.Any(e => e.StoreId == id);
        }

        public IActionResult Dashboard(int id)
        {
            return View(_context.Stores.FirstOrDefault(s => s.StoreId == id));
        }
    }
}