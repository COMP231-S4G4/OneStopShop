using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using OneStopShop.Models;

namespace OneStopShop.Controllers
{
    public class StoresController : BaseController
    {
        private readonly ApplicationDbContext _context;

        public StoresController(ApplicationDbContext context, IDataProtectionProvider provider, IHttpContextAccessor httpContextAccessor, IWebHostEnvironment _environment) : base(context, provider, httpContextAccessor, _environment)
        {
            _context = context;
        }

        // GET: Stores/Create
        /// <summary>
        /// This action will get triggered when user/seller will click on Create New Store after login
        /// </summary>
        /// <returns>Seller will get Create Store form with required fields</returns></returns>
        public IActionResult Create()
        {
            return View();
        }

        // Post: Stores/Create
        /// <summary>
        /// This action will get triggered when user/seller will click on Create button on create new store form
        /// This action will pass all the details into the database with the information that the seller has provided
        /// </summary>
        /// <returns>Seller gets a new store and is redirected to dashboard</returns>

        [HttpPost]
        public async Task<IActionResult> Create([Bind("StoreId,StoreName,SellerFirstname,SellerLasttname,StoreDescription,PhoneNumber,Email")] Store store)
        {
            if (ModelState.IsValid)
            {
                await HttpContext.Session.LoadAsync();

                string userId = HttpContext.Session.GetString("UserId");
               int userID = Convert.ToInt32(protector.Unprotect(userId));
                _context.Add(store);
                var user = await _context.Users
               .FirstOrDefaultAsync(m => m.UserID == userID);
                await _context.SaveChangesAsync();

                Subscribers joined = new Subscribers()
                {
                    UserId = userID,
                    StoreId = store.StoreId,
                    Username = user.Username,
                    email = user.email,                    
                    IsOwner = true,
                };
                await _context.JoinedStore.AddAsync(joined);
                await _context.SaveChangesAsync();

                return RedirectToAction("Dashboard", "Stores", new { ID = joined.StoreId });
            }
            return View(store);
        }

        // GET: Blogs/Index
        /// <summary>
        /// This action will get triggered when user will click on My store button
        /// User will be able to see his store
        /// </summary>
        /// <returns>User will get his store</returns>
        public async Task<IActionResult> Index()
        {
            string userId = HttpContext.Session.GetString("UserId");
            ViewBag.message = Convert.ToInt32(protector.Unprotect(userId));
            return View(await _context.Stores.Include(a => a.JoinedStore).ToListAsync());

        }

        // GET: Stores/Details
        /// <summary>
        /// This action gets triggered when user clicks on the details button on My Store Page
        /// details of the store are displayed
        /// </summary>
        /// <returns>returns the details of a particular store</returns>
        public async Task<IActionResult> Details(int id)
        {
            var productlist = await _context.Products.Where(i => i.StoreId.Equals(id)).ToListAsync();
            var store = _context.Stores.Find(id);
            var tupleData = new Tuple<IList<OneStopShop.Models.Product>, OneStopShop.Models.Store>(productlist, store);
            return View(tupleData);
        }

        // GET: Stores/Edit
        /// <summary>
        /// This action will get triggered when user/seller will click on Edit Store button
        /// This action will display the edit Store page
        /// </summary>
        /// <returns>Seller will get Edit Store form with all the information for that particular Store and editable fields</returns>
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

        // Post: Stores/Edit
        /// <summary>
        /// This action will get triggered when user/seller will click on Update button on Edit store form
        /// This action will fetch all the details from the database with the editable fields
        /// Seller will be able to edit all the store fields
        /// </summary>
        /// <returns>Seller will get an updated store with the information that he provided while editing the store</returns>

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

        //Get Stores/Delete
        /// <summary>
        /// This action will get triggered when user/seller will click on Delete Store button
        /// This action will display the delete store prompt
        /// </summary>
        /// <returns>Seller will get delete store prompt which asks if user wants to delete store</returns>
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

        //Post Stores/Delete
        /// <summary>
        /// This action will get triggered when user/seller will click on Yes button on Delete store prompt
        /// Seller will be able to delete the particular store
        /// </summary>
        /// <returns>The store will get deleted</returns>
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