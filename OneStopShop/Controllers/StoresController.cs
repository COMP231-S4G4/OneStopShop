using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
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
            //var storeDetails = await _context.Stores.Where(a => a.StoreId.Equals(id)).Include(a => a.product)
            //   .FirstOrDefaultAsync();
            //return View(storeDetails);

            var productlist = await _context.Products.Where(i => i.StoreId.Equals(id)).ToListAsync();
            var store = _context.Stores.Find(id);
            var tupleData = new Tuple<IList<OneStopShop.Models.Product>, OneStopShop.Models.Store>(productlist, store);
            return View(tupleData);
        }

        public async Task<IActionResult> Search(int id,string searchTerm)
        {
            IList<Product> Produnewlist = null;

            var prodlist = from s in _context.Products.Where(i => i.StoreId.Equals(id)) select s;
            if (!String.IsNullOrEmpty(searchTerm))
            {
                Produnewlist = prodlist.Where(i => i.ProductName.Contains(searchTerm)
                                       || i.ProductDescription.Contains(searchTerm)).ToList();
            }

            var store = _context.Stores.Find(id);
            var tupleData = new Tuple<IList<OneStopShop.Models.Product>, OneStopShop.Models.Store>(Produnewlist, store);
            return View("Details", tupleData);
        }

        public IActionResult Productlist(int id)
        {            
            return RedirectToAction("ProductList", "Products", new { ID = id });
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

        public IActionResult Orders(int id)
        {   

            return RedirectToAction("Index", "Orders" ,new { Id = id } );
        }
    }
}