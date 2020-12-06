using System;
using System.Collections.Generic;
using System.Globalization;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Http;
using System.Linq;
using System.Threading.Tasks;
using OneStopShop.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using OneStopShop.Models;


using Stripe;
using Microsoft.AspNetCore.Hosting;

namespace OneStopShop.Controllers
{
    public class OrdersController: Controller
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
        public IActionResult Index(int id)
        {
            var OrderList = _context.OrderItems.ToList();
            currentStore = id;

            List<Models.OrderItem> StoreOrders = new List<Models.OrderItem>();

            var StoreOrderList = (from item in OrderList
                                  where item.StoreId == id
                                  select item).ToList();
            foreach (var item in StoreOrderList)
            {
                var order = _context.Orders.FirstOrDefault(o => o.OrderId == item.OrderId);
                if (order.PaymentConfirmation == true)
                {
                    StoreOrders.Add(item);
                }
            }

            return View(StoreOrders);
        }

        // GET: Orders/Details/id
        /// <summary>
        /// This action will get triggered when clicks on the details button attached to each order.
        /// first we get the order item from the OrderItems table using OrderItemID and then we use that to get order from Orderstable and products from Products table.
        /// products, orderitem, and orders are then passed to the view for display.
        /// </summary>
        /// <returns>tupledata containing information about products, orderitem, and orders to Details view</returns>
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var orderitem = await _context.OrderItems
               .FirstOrDefaultAsync(m => m.OrderItemId == id);
            var orders = await _context.Orders
                .FirstOrDefaultAsync(m => m.OrderId == orderitem.OrderId);
            var products = await _context.Products
               .FirstOrDefaultAsync(m => m.ProductID == orderitem.ProductId);

            if (orders == null)
            {
                return NotFound();
            }
            var tupleData = new Tuple<OneStopShop.Models.Product, OneStopShop.Models.OrderItem, OneStopShop.Models.Orders>(products, orderitem, orders);
            return View("Details", tupleData);
        }

        private bool OrdersExists(int id)
        {
            return _context.Orders.Any(e => e.OrderId == id);
        }

        public IActionResult Dashboard()
        {
            return RedirectToAction("Dashboard", "Stores", new { id = currentStore });
        }

        //Get Checkout
        //Get Checkout        
        /// <summary>
        /// Display the checkout page with product information saved as an order.
        /// </summary>
        public IActionResult Checkout()
        {
            
                Orders order = new Orders();
                _context.Orders.Add(order);
                _context.SaveChanges();

                order.Lines = cart.Lines.ToArray();

                return View(order);
            
           
            
            return View(order);
        }

        //Post/CheckOut
        /// <summary>
        /// Submit the chekout information. Order will get upadated with checkout info.
        /// </summary>
        /// <param name="order"></param>
       

        [HttpPost]
        public IActionResult Checkout(Orders order)
        {
            string userId = HttpContext.Session.GetString("UserId");
           
            //int userID = Convert.ToInt32(protector.Unprotect(userId.ToString());
            if (cart.Lines.Count() == 0)
            {
                ModelState.AddModelError("", "Sorry, your cart is empty!");
            }

            if (ModelState.IsValid)
            {
                order.Lines = cart.Lines.ToArray();
                order.UserId = (int)HttpContext.Session.GetInt32("UserId");
               
                var cost = order.Lines.Sum(e => e.Product.ProductPrice * e.Quantity).ToString("c");
                ViewBag.Message = cost;
                order.TotalCost = order.Lines.Sum(e => e.Product.ProductPrice * e.Quantity);


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

                return View("Payment", order);
            }
            else
            {
                return View(order);
            }
        }

        //Get payment
       ///<summary>
       ///Payment view displayed
       ///<summary>

        public IActionResult Payment()
        {
            return View();
        }

        

        /// <summary>
        /// Post/Submit the payment information. It uses stripe test account.
        /// </summary>
        /// <param name="stripeEmail"> Email in the payment</param>
        /// <param name="stripeToken"> Token generated for the payment action</param>
        /// <param name="id"> This is the id associated with the order</param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult Payment(string stripeEmail, string stripeToken, int id)
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
                Amount = Convert.ToInt64(cost * 100),
                Description = "OneStopShopPayment",
                Currency = "CAD",
                Customer = customer.Id,
                ReceiptEmail = stripeEmail
            });

            if (charge.Status == "succeeded")
            {
                string BalanceTransactionId = charge.BalanceTransactionId;
                var order = _context.Orders
                .FirstOrDefault(m => m.OrderId == id);
                order.OrderCreatedDate = DateTime.Now;
                order.PaymentConfirmation = true;
                _context.Update(order);
                _context.SaveChanges();

                cart.Clear();

                return View("OrderConfirmation", order);
            }

            return View();
        }
        /// <summary>
        /// Order confirmation view loaded after succesfull payment
        /// </summary>
        /// <returns></returns>
        public ActionResult OrderConfirmation()
        {
            return View();
        }
        public ActionResult PreviousOrder()
        {
            return View("PreviousOrder");
        }


        ///<summary>
        ///Seller can update the status of order that they received
        ///</summary>

        [HttpPost]
        public IActionResult StatusUpdate(int id)
        {
            if (ModelState.IsValid)
            {
                var selectedValue = Request.Form["OrderStatus"].ToString();
                var orderitem = _context.OrderItems.FirstOrDefault(m => m.OrderItemId == id);
               
                ViewBag.message = orderitem.StoreId;
                if (selectedValue!=string.Empty)
                {
                    orderitem.Staus = selectedValue;
                    TempData["msg"] = "Order Updated Successfully";               
                  

                    _context.OrderItems.Update(orderitem);

                    _context.SaveChanges();
                }
                else
                {
                    ViewBag.message = orderitem.StoreId;
                    TempData["msg"] = "Please select a status";
                }
                              

            }
            return View("OrderUpdateConfirmation");
        }
    }
}