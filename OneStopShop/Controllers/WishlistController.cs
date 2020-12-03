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
    public class WishlistController : BaseController
    {

        private readonly ApplicationDbContext _context;

        public WishlistController(ApplicationDbContext context, IDataProtectionProvider provider, IHttpContextAccessor httpContextAccessor, IWebHostEnvironment _environment) : base(context, provider, httpContextAccessor, _environment)
        {
            _context = context;
        }

        // GET: Wishlist
        /// <summary>
        /// This action will get triggered when user/buyer will click on Add to Wishlist button on Product List Page
        /// This action will pass the product added in the wishlist to the wishlist screen
        /// Buyer will be able to see all the products in the wishlist
        /// </summary>
        /// <returns>Buyer will get all the products that he added in the wishlist</returns>

        public async Task<ActionResult> Index(int productID)
        {
            int UserId = (int)HttpContext.Session.GetInt32("UserId");
            var list = await _context.Wishlists.Where(a => a.UserId.Equals(UserId)).Include(a => a.Product).ToListAsync();
            return View(list);
        }

        // POST: WishlistController/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id)
        {

            try
            {
                Product product = _context.Products
                .FirstOrDefault(p => p.ProductID == id);

                if (product != null)
                {
                    //wishList.RemoveLine(product);
                }

                return RedirectToAction("WishList");
            }
            catch
            {
                return View("WishList");
            }
        }
    }
}
