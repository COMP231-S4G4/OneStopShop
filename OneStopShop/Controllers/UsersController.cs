using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using OneStopShop.Models;

namespace sampleUsser.Controllers
{
    public class UsersController : Controller
    {
        private readonly ApplicationDbContext _context;

        public UsersController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Users
        public async Task<IActionResult> Index()
        {
            return View(await _context.Users.ToListAsync());
        }

        // GET: Users/Details/5
        public async Task<IActionResult> Details(int? UserId)
        {
            if (UserId == null)
            {
                return NotFound();
            }

            var user = await _context.Users
                .FirstOrDefaultAsync(m => m.UserID == UserId);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // GET: Users/Create
        public IActionResult Create()
        {
          
            return View();
        }

        // POST: Users/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("UserID,StoreId,Username,Password,Address,email,PhoneNum,AccountType,BankName,AccountNumber,TransitNumber,InstitutionNumber,RoleId")] Users user)
        {
            if (ModelState.IsValid)
            {
                _context.Add(user);

                await _context.SaveChangesAsync();
                //return RedirectToAction(nameof(Index));
                return RedirectToAction("Login", "Users");
            }
            return View(user);
        }

        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [RequireHttps]
        public async Task<IActionResult> Login(string username, string password)
        {
            if (ModelState.IsValid)
            {
                var user = _context.Users.Where(a => a.Username.Equals(username)).FirstOrDefault();
                if (user == null)
                {
                    ViewData["loginErrorMessege"] = "Account does not exist, please try with try with different Email";
                    return View();
                }
                var userId = user.UserID;
                if (username == user.Username && password == user.Password)
                {
                    HttpContext.Session.SetInt32("UserId", userId);
                    if (user.AccountType == "Seller")
                    {
                        var storeid = _context.JoinedStore.Where(a => a.UserId.Equals(user.UserID) && a.IsOwner.Equals(true)).Select(a => a.StoreId).FirstOrDefault();
                        if (storeid != 0)
                            return RedirectToAction("Dashboard", "Stores", new { id = storeid });
                        else
                            return RedirectToAction("Index", "Stores", new { id = storeid });
                        //}
                    }
                    else
                    {
                        return RedirectToAction("Index", "Home");
                    }
                }
                else
                {
                    ViewData["loginErrorMessege"] = "Account does not exist, please try with try with different Email";
                    return View();
                }
            }
            return View();
        }

        // GET: Users/Edit
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            return View(user);
        }

        // POST: Users/Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("UserID,Username,Password,AccountType,Address,email,PhoneNum,BankName,AccountNumber,TransitNumber,InstitutionNumber,RoleId")] Users user)
        {
            if (id != user.UserID)
            {
                return NotFound();
            }
           
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(user);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserExists(user.UserID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(user);
        }

        // GET: Users/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.Users
                .FirstOrDefaultAsync(m => m.UserID == id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // POST: Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var user = await _context.Users.FindAsync(id);
            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UserExists(int id)
        {
            return _context.Users.Any(e => e.UserID == id);
        }
    }
}