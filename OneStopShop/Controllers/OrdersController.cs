using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
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

        // GET: List of orders for a store
        public  IActionResult Index(int id)
        {
            var OrderList = _context.OrderItems.ToList();

            List<Models.OrderItem> StoreOrders = new List<Models.OrderItem>();

            var StoreOrderList = (from item in OrderList
                               where item.StoreId == id
                               select item).ToList();
            foreach (var item in StoreOrderList)
            {
                var order = _context.Orders.FirstOrDefault(o => o.OrderId == item.OrderId);
                if(order.PaymentConfirmation== true)
                {
                    StoreOrders.Add(item);
                }
            }


            return View(StoreOrders);
        }


        // GET: Orders/Details/id
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var orderitem = await _context.OrderItems
               .FirstOrDefaultAsync(m => m.OrderId == id);
            var products= await _context.Products
               .FirstOrDefaultAsync(m => m.ProductID == orderitem.ProductId);
            var orders = await _context.Orders
                .FirstOrDefaultAsync(m => m.OrderId == orderitem.OrderId);         
            

            if (orders == null)
            {
                return NotFound();
            }

            return View(orders);
        }   

       
       

        private bool OrdersExists(int id)
        {
            return _context.Orders.Any(e => e.OrderId == id);
        }

        //Get Checkout
        public IActionResult Checkout()
        {
           Orders order = new Orders();
            _context.Orders.Add(order);
            _context.SaveChanges();

            order.Lines = cart.Lines.ToArray();

            return View(order);
            
        }

        //Post/CheckOut

        [HttpPost]
        public IActionResult Checkout(Orders order)
        {
            if (cart.Lines.Count() == 0)
            {
                ModelState.AddModelError("", "Sorry, your cart is empty!");
            }

            if (ModelState.IsValid)
            {

                order.Lines = cart.Lines.ToArray();

                var cost = order.Lines.Sum(e => e.Product.ProductPrice * e.Quantity).ToString("c");
                ViewBag.Message = cost;
                ViewBag.OrderId = order.OrderId;
                _context.Update(order);
                _context.SaveChanges();

                foreach (var line in order.Lines)
                {
                    Models.OrderItem item = new Models.OrderItem()
                    {
                        OrderId = order.OrderId,
                        ProductId = line.Product.ProductID,
                        StoreId = line.Product.StoreId,
                        Quantity = line.Quantity,
                        Cost = line.Product.ProductPrice * line.Quantity
                    };
                    _context.OrderItems.Add(item);
                    _context.SaveChanges();
                }              

                return View("Payment",order);
               
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
        public IActionResult Payment(string stripeEmail, string stripeToken,Orders order)
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
                order.OrderCreatedDate = DateTime.Now;
                order.PaymentConfirmation = true;
                _context.Update(order);
                _context.SaveChanges();

                return View("OrderConfirmation",order);
            }


            return View();

        }
        public ActionResult OrderConfirmation()
        {
            return View();
        }
    }
}
