using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using OneStopShop.Models;
using Stripe;

namespace OneStopShop.Controllers
{
    public class OrdersController : Controller
    {
        private readonly ApplicationDbContext _context;
        private static int currentStore = 0;
        private Cart cart;

        public OrdersController(ApplicationDbContext context, Cart cartService)
        {
            _context = context;
            cart = cartService;

        }

        // GET: Orders
        public async Task<IActionResult> Index(int StoreID)
        {
            currentStore = StoreID;
            return View(await _context.Orders.Where(i => i.StoreId.Equals(StoreID)).ToListAsync());
        }

        // GET: Orders/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var orders = await _context.Orders
                .FirstOrDefaultAsync(m => m.OrderId == id);
            if (orders == null)
            {
                return NotFound();
            }

            return View(orders);
        }

        // GET: Orders/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Orders/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("OrderId,ProductID,StoreId,OrderCreatedDate,OrderModifiedDate")] Orders orders)
        {
            if (ModelState.IsValid)
            {
                _context.Add(orders);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(orders);
        }

        // GET: Orders/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var orders = await _context.Orders.FindAsync(id);
            if (orders == null)
            {
                return NotFound();
            }
            return View(orders);
        }

        // POST: Orders/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("OrderId,ProductID,StoreId,OrderCreatedDate,OrderModifiedDate")] Orders orders)
        {
            if (id != orders.OrderId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(orders);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OrdersExists(orders.OrderId))
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
            return View(orders);
        }

        // GET: Orders/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var orders = await _context.Orders
                .FirstOrDefaultAsync(m => m.OrderId == id);
            if (orders == null)
            {
                return NotFound();
            }

            return View(orders);
        }

        // POST: Orders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var orders = await _context.Orders.FindAsync(id);
            _context.Orders.Remove(orders);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool OrdersExists(int id)
        {
            return _context.Orders.Any(e => e.OrderId == id);
        }

        //Get Checkout
        public IActionResult Checkout()
        {
            Orders order = new Orders();
           
            order.Lines = cart.Lines.ToArray();
            _context.Update(order);
           

            //ViewModel model = new ViewModel();
            //model.product = product;
            //model.order = order;         


            return View(order);
            
        }

        [HttpPost]
        public IActionResult Checkout(Orders order)
        {
            //model.order.Lines = cart.Lines.ToArray();
            //_context.Orders.Add(model.order);
            //return View("Payment");

           
            if (cart.Lines.Count() == 0)
            {
                ModelState.AddModelError("", "Sorry, your cart is empty!");
            }

            if (ModelState.IsValid)
            {
                order.Lines = cart.Lines.ToArray();
                var cost=order.Lines.Sum(e => e.Product.ProductPrice * e.Quantity).ToString("c");
                ViewBag.Message = cost;
                _context.Update(order);
                _context.SaveChanges();
                return View("Payment");

               // return RedirectToAction(nameof(Completed));
            }
            else
            {
                return View(order);
            }

        }

        //Get payment
        public IActionResult Payment()
        {
            return View();
        }

        //post payment

        [HttpPost]
        public IActionResult Payment(string stripeEmail, string stripeToken)
        {
            var cost = cart.Lines.ToArray().Sum(e => e.Product.ProductPrice * e.Quantity);
            var customers = new CustomerService();
            var charges = new ChargeService();
            var customer = customers.Create(new CustomerCreateOptions
            {
                Email = stripeEmail,
                Source = stripeToken
            });
            var charge = charges.Create(new ChargeCreateOptions
            {                 

                Amount =Convert.ToInt64(cost*100),
                Description = "OneStopShopPayment",
                Currency = "CAD",
                Customer = customer.Id,
                ReceiptEmail = stripeEmail

            });

            if (charge.Status == "succeeded")
            {
                string BalanceTransactionId = charge.BalanceTransactionId;
                return View();
            }


            return View();

        }
        public ActionResult OrderConfirmation()
        {
            return View();
        }
    }
}
