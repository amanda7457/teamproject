using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Group14_BevoBooks.DAL;
using Group14_BevoBooks.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace Group14_BevoBooks.Controllers
{
    public class OrderController : Controller
    {
        private readonly UserManager<AppUser> _userManager;

        private AppDbContext _context;

        public OrderController(AppDbContext context, UserManager<AppUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Order
        public async Task<IActionResult> Index()
        {
            List<Order> Orders = new List<Order>();

            if (User.IsInRole("Customer"))
            {
                Orders = _context.Orders.Include(m => m.OrderDetails).Where(o => o.AppUser.UserName == User.Identity.Name).ToList();
            }
            else
            {
                Orders = _context.Orders.Include(o => o.OrderDetails).ToList();
            }
            return View(Orders);
        }

        [Authorize]
        // POST: Order/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public async Task<IActionResult> Create(int bookid, [Bind("OrderID,OrderDate")] Order order)
        {
            TempData["bookid"] = bookid;
            List<Order> orderlist = new List<Order>();
            bool orderplaced;
            Order lastorder;

            if (ModelState.IsValid)
            {
                //assocaite order with user
                AppUser user = _context.Users.FirstOrDefault(u => u.UserName == User.Identity.Name);

                try
                {
                    //find most recent order
                    orderlist = _context.Orders.Include(o => o.OrderDetails).Where(o => o.AppUser.UserName == User.Identity.Name).ToList();
                    lastorder = orderlist.LastOrDefault();
                    orderplaced = lastorder.OrderPlaced;
                }
                catch
                {
                    orderlist = new List<Order>();
                    orderplaced = true;
                    lastorder = null;
                }


                bool isEmpty = !orderlist.Any();
                if (isEmpty == true || orderplaced == true)
                {
                    order.OrderDate = System.DateTime.Today;
                    order.AppUser = user;

                    _context.Add(order);
                    await _context.SaveChangesAsync();

                    return RedirectToAction("AddToCart", new { id = order.OrderID });
                }
                else
                {
                    //_context.Remove(order);
                    //await _context.SaveChangesAsync();

                    return RedirectToAction("AddToCart", new { id = lastorder.OrderID });
                }
            }

            return View(order);
        }

        [Authorize]
        //GET: /Order/AddToCart
        public async Task<IActionResult> AddToCart(int id)
        {
            //var order = await _context.Orders.Include(o => o.OrderDetails).ThenInclude(o => o.Book).FirstOrDefaultAsync(o => o.OrderID == id);

            int bookid = Convert.ToInt32(TempData["bookid"]);
            Book book = _context.Books.Find(bookid);

            ViewBag.BookName = book.Title;

            //find order in db
            OrderDetail od = new OrderDetail();
            od.Order = _context.Orders.Find(id);

            await _context.SaveChangesAsync();

            TempData.Keep();


            return View(od);
        }

        bool AlreadyInOrder;
        [Authorize]
        //POST: /Order/AddToCart
        [HttpPost]
        public async Task<IActionResult> AddToCart(OrderDetail od, int Quantity)
        {
            int bookid = Convert.ToInt32(TempData["bookid"]);
            Book book = _context.Books.Find(bookid);

            Order order = _context.Orders.FirstOrDefault(o => o.OrderID == od.Order.OrderID);
            List<OrderDetail> odlist = order.OrderDetails.ToList();

            //make sure book is not already in order, if it is add to existing od
            foreach (OrderDetail orderdetail in odlist)
            {
                int id = orderdetail.Book.BookID;

                if (id == book.BookID)
                {
                    AlreadyInOrder = true;
                    orderdetail.Quantity = orderdetail.Quantity + Quantity;
                }

                else
                {
                    AlreadyInOrder = false;
                }
            }

            if (AlreadyInOrder == false)
            {
                od.Book = book;
                od.Order = order;

                //quantity equal to selected quantity
                od.Quantity = Quantity;

                //set the product price for this detail equal to the current price
                decimal price = book.SellingPrice;
                od.Price = price;
            }

            //order not placed yet
            od.Order.OrderPlaced = false;


            //Order order = _context.Find(orderid);

            if (ModelState.IsValid)
            {
                //check if book is in stock -- TODO probably need to add some more logic here
                if (book.Inventory == 0)
                {
                    ViewBag.Message = "Sorry - this book is out of stock.";
                    return View(od);
                }

                else //everything is going right
                {
                    _context.OrderDetails.Add(od);

                    await _context.SaveChangesAsync();

                    //od.Order = order;

                    //await _context.SaveChangesAsync();

                    return RedirectToAction("Cart", new { id = od.Order.OrderID });
                }
            }

            return View("Index", "Book");
        }

        public Int32 GetOrderNumber()
        {
            //variables
            AppUser user = _context.Users.FirstOrDefault(u => u.UserName == User.Identity.Name);
            List<Order> orderlist = new List<Order>();
            bool orderplaced;
            Order lastorder;

            try
            {
                //find most recent order
                orderlist = _context.Orders.Include(o => o.OrderDetails).Where(o => o.AppUser.UserName == User.Identity.Name).ToList();
                lastorder = orderlist.LastOrDefault();
                orderplaced = lastorder.OrderPlaced;
            }
            catch
            {
                orderlist = new List<Order>();
                orderplaced = true;
                lastorder = null;
            }


            bool isEmpty = !orderlist.Any();
            if (isEmpty == true || orderplaced == true)
            {
                Int32 ordernumber = -1;
                return ordernumber;
            }
            else
            {
                Int32 ordernumber = lastorder.OrderID;
                return ordernumber;
            }
        }

        // GET: Order/Cart
        [Authorize]
        public ActionResult Cart(int? id)
        {
            if (id == null)
            {
                id = GetOrderNumber();
            }

            if (id == -1)
            {
                return View("Empty", new string[] { "You must have at least one item in your cart" });
            }

            else
            {
                Order order = _context.Orders.Include(o => o.AppUser).Include(o => o.OrderDetails).
                                      ThenInclude(o => o.Book).FirstOrDefault(o => o.OrderID == id);

                //create order details list
                List<OrderDetail> ods = _context.OrderDetails.Include(m => m.Book).Where(o => o.Order.OrderID == order.OrderID).ToList();

                if (User.IsInRole("Manager") == false && order.AppUser.UserName != User.Identity.Name)
                {
                    return View("Error", new string[] { "You are not authorized to view this order!" });
                }

                if (order == null)
                {
                    return View("Error", new string[] { "Order was not found" });
                }

                //make sure prices that are shows are the most updated
                foreach (OrderDetail od in ods)
                {
                    Book book = od.Book;
                    od.Price = book.SellingPrice;
                }

                //find shipping prices in db
                Shipping shipping = _context.ShippingPrices.FirstOrDefault(s => s.ShippingID == 1);

                decimal shippingfirst = shipping.ShippingFirst;
                decimal shippingadditional = shipping.ShippingAdditional;

                if (ods.Count < 1)
                {
                    order.OrderShipping = 0;
                }

                if (ods.Count == 1)
                {
                    order.OrderShipping = shippingfirst;
                }

                if (ods.Count > 1)
                {
                    int additionalbooks = ods.Count - 1;

                    order.OrderShipping = shippingfirst + (shippingadditional * additionalbooks);
                }

                _context.SaveChangesAsync();

                return View(order);
            }

        }

        [Authorize]
        // GET: Order/Checkout
        public IActionResult Checkout(int? id)
        {
            Order order = _context.Orders.Include(o => o.AppUser).Include(o => o.OrderDetails).
                                  ThenInclude(o => o.Book).FirstOrDefault(o => o.OrderID == id);

            ViewBag.BookCount = GetBookCount(id);

            AppUser user = _context.Users.Include(c => c.CreditCards).FirstOrDefault(u => u.UserName == User.Identity.Name);

            String street = user.Address;
            String city = user.City;
            String state = user.State;
            String zip = user.Zipcode;

            String FullAddress = street + " " + city + " " + state + " " + zip;

            ViewBag.FullAddress = FullAddress;

            List<string> UserCCs = GetHiddenCards();

            List<CreditCard> userccs = GetCards();
            List<int> ccids = new List<int>();
            foreach (CreditCard cc in userccs)
            {
                int ccid = cc.CreditCardID;
                ccids.Add(ccid);
            }

            //Viewbags for CC1 CC2 CC3
            var ccCount = UserCCs.Count;

            if (ccCount == 1)
            {
                ViewBag.CC1 = UserCCs[0];
                ViewBag.CC1ID = ccids[0];
            }

            if (ccCount == 2)
            {
                ViewBag.CC1 = UserCCs[0];
                ViewBag.CC1ID = ccids[0];

                ViewBag.CC2 = UserCCs[1];
                ViewBag.CC2ID = ccids[1];
            }

            if (ccCount == 3)
            {
                ViewBag.CC1 = UserCCs[0];
                ViewBag.CC1ID = ccids[0];

                ViewBag.CC2 = UserCCs[1];
                ViewBag.CC2ID = ccids[1];

                ViewBag.CC3 = UserCCs[2];
                ViewBag.CC3ID = ccids[2];
            }

            return View(order);
        }

        public IActionResult Redeem(string promocodeinput, int? OrderID)
        {
            //TODO - why does price change when code is not valid???

            //is entered code in list of codes
            Boolean bolpromovalid = IsPromoValid(promocodeinput);

            Order order = _context.Orders.Include(o => o.Discount).Include(o => o.AppUser).Include(o => o.OrderDetails).
                                  ThenInclude(o => o.Book).FirstOrDefault(o => o.OrderID == OrderID);

            if (bolpromovalid == false)
            {
                ViewBag.PromoMessage = promocodeinput + " is not a valid promotional code - try again";
                return View("Checkout", order);
            }


            if (bolpromovalid == true)
            {
                //find discount in db
                Discount discount = _context.Discounts.FirstOrDefault(d => d.PromoCode == promocodeinput);
                Decimal discountamount = discount.DiscountAmount;

                if(discount.Active == false)
                {
                    ViewBag.PromoMessage = promocodeinput + " code is not currently active - please try again";
                    return View("Checkout", order);
                }

                //if customer has already used this promo code
                Boolean hasbeenused = CustomerUsedCode(discount);
                if (hasbeenused == true)
                {
                    ViewBag.PromoMessage = "You have already used this code - please try again";
                    return View("Checkout", order);
                }


                else
                {
                    order.Discount = discount;

                    if (order.Discount.DiscountType == DiscountType.FreeShipping)
                    {
                        //ordershipping
                        if (order.OrderTotal >= discountamount)
                        {
                            order.OrderShipping = 0;
                        }
                    }

                    if (order.Discount.DiscountType == DiscountType.PercentOff)
                    {
                        //ordertotal
                        decimal discountpercent = discountamount / 100;

                        List<decimal> actualprices = new List<decimal>();

                        List<OrderDetail> orderdetails = order.OrderDetails.ToList();
                        foreach (OrderDetail od in orderdetails)
                        {
                            od.Price = od.Price * discountpercent;
                        }
                    }
                }

                _context.SaveChanges();
                return RedirectToAction("Checkout", new { id = order.OrderID });
            }

            return RedirectToAction("Checkout", new { id = order.OrderID });
        }

        [Authorize]
        // GET: Order/Checkout
        [HttpPost]
        public IActionResult Checkout(int SelectedCreditCard)
        {
            int ordernumber = GetOrderNumber();

            Order order = _context.Orders.Include(o => o.AppUser).Include(o => o.OrderDetails).
                                  ThenInclude(o => o.Book).FirstOrDefault(o => o.OrderID == ordernumber);

            List<CreditCard> userccs = GetCards();

            switch (SelectedCreditCard)
            {
                case 1:
                    order.CreditCard = userccs[0];
                    break;

                case 2:
                    order.CreditCard = userccs[1];
                    break;

                case 3:
                    order.CreditCard = userccs[2];
                    break;

                default:
                    break;
            }
            _context.SaveChanges();

            return RedirectToAction("OrderSummary", new { id = order.OrderID });
        }

        //TODO check on when to take stuff out of inventory....

        [Authorize]
        // GET: Order/OrderSummary
        public IActionResult OrderSummary(int? id)
        {
            Order order = _context.Orders.Include(o => o.AppUser).Include(o => o.CreditCard).Include(o => o.OrderDetails).
                                  ThenInclude(o => o.Book).FirstOrDefault(o => o.OrderID == id);

            ViewBag.BookCount = GetBookCount(id);

            string hiddenString;
            string ccnumber = order.CreditCard.CardNumber;

            hiddenString = ccnumber.Substring(ccnumber.Length - 4).PadLeft(ccnumber.Length, '*');

            ViewBag.CreditCard = hiddenString;

            AppUser user = _context.Users.Include(c => c.CreditCards).FirstOrDefault(u => u.UserName == User.Identity.Name);

            String street = user.Address;
            String city = user.City;
            String state = user.State;
            String zip = user.Zipcode;

            String FullAddress = street + " " + city + " " + state + " " + zip;

            ViewBag.FullAddress = FullAddress;

            return View(order);
        }


        [Authorize]
        // POST: Order/PlaceOrder
        [HttpPost]
        public IActionResult PlaceOrder(int? orderid, CreditCard cc)
        {

            Order order = _context.Orders.Include(o => o.AppUser).Include(o => o.CreditCard).Include(o => o.OrderDetails).
                                  ThenInclude(o => o.Book).FirstOrDefault(o => o.OrderID == orderid);

            //TODO: don't let customer check out with empty cart
            foreach (OrderDetail od in order.OrderDetails)
            {
                if (od.Book.Inventory == 0)
                {
                    order.OrderDetails.Remove(od);
                    ViewBag.Message = "This title is out of stock and has been removed from your cart";
                }
                else
                {
                    od.Book.Inventory = od.Book.Inventory - od.Quantity;
                }
            }

            order.OrderPlaced = true;

            _context.SaveChanges();

            return RedirectToAction("Index", "Reccomendation");
        }

        public IActionResult Confirmation(int id)
        {
            //TODO: add reccomendation queries

            return View("Confirmation");
        }

        [Authorize]
        // GET: Order/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            {
                if (id == null)
                {
                    return View("Error", new string[] { "Specify an order to view!" });
                }

                Order order = _context.Orders.Include(o => o.AppUser).Include(o => o.CreditCard).Include(o => o.OrderDetails).
                                      ThenInclude(o => o.Book).FirstOrDefault(o => o.OrderID == id);

                if (User.IsInRole("Manager") == false && order.AppUser.UserName != User.Identity.Name)
                {
                    return View("Error", new string[] { "You are not authorized to view this order!" });
                }

                if (order == null)
                {
                    return View("Error", new string[] { "Order was not found" });
                }

                string hiddenString;
                string ccnumber = order.CreditCard.CardNumber;

                hiddenString = ccnumber.Substring(ccnumber.Length - 4).PadLeft(ccnumber.Length, '*');

                @ViewBag.CreditCard = hiddenString;

                return View(order);
            }
        }

        // GET: Order/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order = await _context.Orders.FindAsync(id);
            if (order == null)
            {
                return NotFound();
            }
            return View(order);
        }

        [Authorize(Roles = "Managers")]
        // POST: Order/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("OrderID,OrderDate")] Order order)
        {
            if (id != order.OrderID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(order);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OrderExists(order.OrderID))
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
            return View(order);
        }

        [Authorize(Roles = "Managers")]
        // GET: Order/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order = await _context.Orders
                .FirstOrDefaultAsync(m => m.OrderID == id);
            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }

        [Authorize(Roles = "Managers")]
        // POST: Order/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var order = await _context.Orders.FindAsync(id);
            _context.Orders.Remove(order);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public IActionResult RemoveFromOrder(int? id)
        {
            if (id == null)
            {
                return View("Error", new string[] { "You need to specify an order id" });
            }

            Order order = _context.Orders.Include(o => o.OrderDetails).ThenInclude(od => od.Book).FirstOrDefault(o => o.OrderID == id);

            if (order == null || order.OrderDetails.Count == 0)//order is not found
            {
                return RedirectToAction("Details", new { id = id });
            }

            //pass the list of order details to the view
            return View(order.OrderDetails);
        }

        public Int32 GetBookCount(int? id)
        {
            Int32 BookCount = 0;
            List<OrderDetail> odlist = new List<OrderDetail>();

            Order order = _context.Orders.Include(o => o.AppUser).Include(o => o.OrderDetails).
                                  ThenInclude(o => o.Book).FirstOrDefault(o => o.OrderID == id);

            odlist = order.OrderDetails.ToList();

            foreach (OrderDetail od in odlist)
            {
                BookCount = BookCount + od.Quantity;
            }

            return BookCount;
        }

        public List<CreditCard> GetCards()
        {
            AppUser user = _context.Users.Include(u => u.CreditCards).FirstOrDefault(u => u.UserName == User.Identity.Name);

            List<CreditCard> UserCCs = user.CreditCards.ToList();

            return UserCCs;

        }

        public List<string> GetHiddenCards()
        {
            AppUser user = _context.Users.FirstOrDefault(u => u.UserName == User.Identity.Name);

            List<CreditCard> cclist = user.CreditCards.ToList();

            List<string> hiddencclist = new List<string>();

            foreach(CreditCard cc in cclist)
            {
                string hiddenString;
                string ccnumber = cc.CardNumber;

                hiddenString = ccnumber.Substring(ccnumber.Length - 4).PadLeft(ccnumber.Length, '*');
                hiddencclist.Add(hiddenString);
            }


            return hiddencclist;
        }

        public Boolean IsPromoValid(string discountcode)
        {
            //list of all discounts in db
            List<Discount> discounts = _context.Discounts.ToList();

            //list of all promo codes
            List<string> promocodes = new List<string>();
            foreach (Discount d in discounts)
            {
                string promocode = d.PromoCode;

                promocodes.Add(promocode);
            }

            if (promocodes.Contains(discountcode))
            {
                //if promo code is in list
                return true;
            }

            else
            {
                return false;
            }
        }

        public Boolean CustomerUsedCode(Discount discount)
        {
            AppUser user = _context.Users.Include(c => c.CreditCards).FirstOrDefault(u => u.UserName == User.Identity.Name);

            //list of past customer orders
            List<Order> orders = _context.Orders.Include(m => m.OrderDetails).
                                         Where(o => o.AppUser.UserName == User.Identity.Name && o.OrderPlaced == true).ToList();

            List<Discount> discountsused = new List<Discount>();
            foreach (Order o in orders)
            {
                Discount d = o.Discount;
                discountsused.Add(d);
            }

            //if customer has used this code before
            if (discountsused.Contains(discount))
            {
                return true;
            }

            else
            {
                return false;
            }

        }

        private bool OrderExists(int id)
        {
            return _context.Orders.Any(e => e.OrderID == id);
        }
    }
}
