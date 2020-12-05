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
    /// <summary>
    /// This controller has the actions where Buyer is able to Get and Delete the products in the wishlist.
    /// </summary>
    public class WishlistController : BaseController
    {
        public WishlistController(ApplicationDbContext context, IDataProtectionProvider provider, IHttpContextAccessor httpContextAccessor, IWebHostEnvironment _environment) : base(context, provider, httpContextAccessor, _environment)
        {

        }

        // GET: Wishlist/Index
        /// <summary>
        /// This action will get triggered when user/buyer will click on Add to Wishlist button on Product List Page
        /// This action will pass the product added in the wishlist to the wishlist screen
        /// Buyer will be able to see all the products in the wishlist
        /// </summary>
        /// <returns>Buyer will get all the products that he added in the wishlist</returns>
        public async Task<ActionResult> Index()
        {
            int UserId = (int)HttpContext.Session.GetInt32("UserId");
            var list = await _context.Wishlists.Where(a => a.UserId.Equals(UserId)).Include(a => a.Product).ToListAsync();
            return View(list);
        }

        // POST: WishlistController/Delete
        /// <summary>
        /// This action will get triggered when clicks on the delete button attacked to each product on the wishlist page
        /// Delete action acts as an edit in the case of wishlist ie editing the wishlist.
        /// </summary>
        /// <returns>The deleted product is removed from the wishlist and the wishlist is refreshed</returns>
        public ActionResult Delete(int id)
        {
            var item = _context.Wishlists.Where(i => i.WishlistId == id).FirstOrDefault();
            _context.Wishlists.Remove(item);
            _context.SaveChanges();

            return RedirectToAction("Index");
        }
    }
}
