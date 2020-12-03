using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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

        // GET: WishlistController
        public ActionResult Index()
        {
            return View("WishList");
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
