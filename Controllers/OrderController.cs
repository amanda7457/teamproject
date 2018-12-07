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
using System.Text;

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
            List<Order> sortedorders = new List<Order>();

            if (User.IsInRole("Customer"))
            {
                Orders = _context.Orders.Include(m => m.OrderDetails).Where(o => o.AppUser.UserName == User.Identity.Name).ToList();
                sortedorders = Orders.OrderByDescending(o => o.OrderDate).ToList();

            }
            else
            {
                Orders = _context.Orders.Include(o => o.OrderDetails).ToList();
                sortedorders = Orders.OrderByDescending(o => o.OrderDate).ToList();
            }
            return View(sortedorders);
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

            //make sure we're not editing existing od

            Order order = _context.Orders.Include(o => o.OrderDetails).ThenInclude(o => o.Book).FirstOrDefault(o => o.OrderID == id);
            List<Book> booksincart = new List<Book>();
            foreach (OrderDetail orderdetail in order.OrderDetails)
            {
                booksincart.Add(orderdetail.Book);
            }

            if (booksincart.Contains(book))
            {
                List<OrderDetail> odlist = _context.OrderDetails.Include(o => o.Order).Include(o => o.Book).
                    Where(o => o.Order == order && o.Book == book).ToList();

                foreach (OrderDetail ord in odlist)
                {
                    order.OrderDetails.Remove(ord);
                }

                OrderDetail od = new OrderDetail();

                od.Order = order;

                TempData.Keep();
                _context.SaveChanges();

                return View(od);
            }

            else
            {
                OrderDetail od = new OrderDetail();

                od.Order = order;

                await _context.SaveChangesAsync();

                TempData.Keep();

                return View(od);
            }
        }

        [Authorize]
        //POST: /Order/AddToCart
        [HttpPost]
        public async Task<IActionResult> AddToCart(OrderDetail od, int Quantity)
        {
            int bookid = Convert.ToInt32(TempData["bookid"]);
            Book book = _context.Books.Find(bookid);

            Order order = _context.Orders.Include(o => o.OrderDetails).FirstOrDefault(o => o.OrderID == od.Order.OrderID);
            //List<OrderDetail> odlist = order.OrderDetails.ToList();

            if (ModelState.IsValid)
            {
                if (book.Inventory < Quantity)
                {
                    ViewBag.Message = "We do not have enough books in stock - please add a lower quantity";
                    return RedirectToAction("AddToCart", new { id = od.Order.OrderID });
                }

                else //everything is going right
                {
                    od.Book = book;
                    od.Order = order;

                    //quantity equal to selected quantity
                    od.Quantity = Quantity;

                    //set the product price for this detail equal to the current price
                    decimal price = book.SellingPrice;
                    od.Price = price;

                    //order not placed yet
                    od.Order.OrderPlaced = false;

                    _context.OrderDetails.Add(od);

                    await _context.SaveChangesAsync();

                    //od.Order = order;

                    //await _context.SaveChangesAsync();

                    return RedirectToAction("Cart", new { id = od.Order.OrderID });
                }
            }

            return View(od);
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


                //if book goes out of stock or is discontinued take out of cart
                StringBuilder outofstock = new StringBuilder();
                List<Book> booksoutofstock = new List<Book>();

                StringBuilder discontinued = new StringBuilder();
                List<Book> discontinuedbooks = new List<Book>();

                foreach (OrderDetail od in ods)
                {
                    int currentinventory = od.Book.Inventory;
                    int quantityincart = od.Quantity;

                    if (quantityincart > currentinventory)
                    {
                        outofstock.Append(od.Book.Title);
                        order.OrderDetails.Remove(od);
                        booksoutofstock.Add(od.Book);
                    }

                    if (od.Book.Discontinued == true)
                    {
                        discontinued.Append(od.Book.Title);
                        order.OrderDetails.Remove(od);
                        discontinuedbooks.Add(od.Book);
                    }
                }

                if (booksoutofstock.Count() >= 1)
                {
                    ViewBag.OutOfStock = outofstock + " out of stock and removed from your cart.";
                }

                if (discontinuedbooks.Count() >= 1)
                {
                    ViewBag.Discontinued = discontinued + " discontinued and removed from your cart.";
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

                if (ods.Sum(od => od.Quantity) < 1)
                {
                    order.OrderShipping = 0;
                }

                if (ods.Sum(od => od.Quantity) == 1)
                {
                    order.OrderShipping = shippingfirst;
                }

                if (ods.Sum(od => od.Quantity) > 1)
                {
                    int additionalbooks = ods.Sum(od => od.Quantity) - 1;

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

            if (User.IsInRole("Manager") == false && order.AppUser.UserName != User.Identity.Name)
            {
                return View("Error", new string[] { "You are not authorized to view this order!" });
            }

            if (order == null)
            {
                return View("Error", new string[] { "Order was not found" });
            }

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

                Decimal discountamount;
                if (discount.DiscountType == DiscountType.FreeShipping)
                {
                    discountamount = discount.DiscountAmountShipping;
                }

                if (discount.DiscountType == DiscountType.PercentOff)
                {
                    discountamount = discount.DiscountAmountShipping;
                }

                else
                {
                    discountamount = -1;
                }

                if (discount.Active == false)
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

            if (User.IsInRole("Manager") == false && order.AppUser.UserName != User.Identity.Name)
            {
                return View("Error", new string[] { "You are not authorized to view this order!" });
            }

            if (order == null)
            {
                return View("Error", new string[] { "Order was not found" });
            }

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

            if (User.IsInRole("Manager") == false && order.AppUser.UserName != User.Identity.Name)
            {
                return View("Error", new string[] { "You are not authorized to view this order!" });
            }

            if (order == null)
            {
                return View("Error", new string[] { "Order was not found" });
            }

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

            string ordernumber = order.OrderID.ToString();
            string orderdate = order.OrderDate.ToString();
            string subtotal = order.OrderSubtotal.ToString();
            string ordertotal = order.OrderTotal.ToString();

            List<Book> ReccomendBooks= new List <Book>();

            ReccomendBooks = GetReccomendedBooks();

            List<string> FinalReccomend = new List<string>();

            foreach (Book book in ReccomendBooks)
            {
                string booktitle;

                booktitle= Convert.ToString(book.Title);

                FinalReccomend.Add(booktitle);

            }

            int CountReccomend; 
            CountReccomend= FinalReccomend.Count();

            if (CountReccomend == 3)
            {
                string bookone;
                string booktwo;
                string bookthree;

                bookone = FinalReccomend[0];
                booktwo = FinalReccomend[1];
                bookthree = FinalReccomend[2];

                String strOrderEmail = GetOrderEmail();
                String strEmailBody = "Congratulations - You have successfully placed an order. Here are your order details:" + "\n" + "Order Number " + ordernumber + "\n" + "Order Date " + orderdate + "\n" + "Order Total $" + ordertotal + "\n" + "Reccomended Book(s) " + "\n" + bookone + "\n" +  booktwo + "\n" + bookthree;
                Utilities.EmailMessaging.SendEmail(strOrderEmail, "Order Placed", strEmailBody);
                return RedirectToAction("Index", "Reccomendation");


            }

            if (CountReccomend==2)
            {
                string bookone;
                string booktwo;

                bookone = FinalReccomend[0];
                booktwo = FinalReccomend[1];

                String strOrderEmail = GetOrderEmail();
                String strEmailBody = "Congratulations - You have successfully placed an order. Here are your order details:" + "\n" + "Order Number " + ordernumber + "\n" + "Order Date " + orderdate + "\n" + "Order Total $" + ordertotal + "\n" + "Reccomended Book(s) " + "\n" + bookone + "\n" +  booktwo;
                Utilities.EmailMessaging.SendEmail(strOrderEmail, "Order Placed", strEmailBody);
                return RedirectToAction("Index", "Reccomendation");
            }

            if (CountReccomend == 1)
            {
                string bookone;
            

                bookone = FinalReccomend[0];


                String strOrderEmail = GetOrderEmail();
                String strEmailBody = "Congratulations - You have successfully placed an order. Here are your order details:" + "\n" + "Order Number " + ordernumber + "\n" + "Order Date " + orderdate + "\n" + "Order Total $" + ordertotal + "\n" + "Reccomended Book(s) " + bookone;
                Utilities.EmailMessaging.SendEmail(strOrderEmail, "Order Placed", strEmailBody);
                return RedirectToAction("Index", "Reccomendation");
            }

            if (CountReccomend == 0)
            {
               
                String strOrderEmail = GetOrderEmail();
                String strEmailBody = "Congratulations - You have successfully placed an order. Here are your order details:" + "\n" + "Order Number " + ordernumber + "\n" + "Order Date " + orderdate + "\n" + "Order Total $" + ordertotal + "\n" + "We have no books to reccomend apologie";
                Utilities.EmailMessaging.SendEmail(strOrderEmail, "Order Placed", strEmailBody);
                return RedirectToAction("Index", "Reccomendation");
            }

            return RedirectToAction("Index", "Reccomendation");

        }

        public String GetOrderEmail()
        {
            IndexViewModel iv = new IndexViewModel();

            //get user info
            String id = User.Identity.Name;
            AppUser user = _context.Users.FirstOrDefault(u => u.UserName == id);

            //populate the view model
            iv.Email = user.Email;
            iv.HasPassword = true;
            iv.UserID = user.Id;
            iv.UserName = user.UserName;

            String WOWEmail;
            WOWEmail = iv.Email;
            return WOWEmail;
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

        public List<Book> GetReccomendedBooks()
        {
            List<Book> reccomendedbooks = new List<Book>();

            //pick one book from last order
            List<Order> pastorders = _context.Orders.Include(m => m.OrderDetails).ThenInclude(m => m.Book).ThenInclude(b => b.Genre).
                                             Where(o => o.AppUser.UserName == User.Identity.Name && o.OrderPlaced == true).ToList();

            //the order that was just placed
            Order justplaced = pastorders.LastOrDefault();

            List<Book> booksinorder = new List<Book>();
            foreach (OrderDetail od in justplaced.OrderDetails.ToList())
            {
                Book b = od.Book;
                booksinorder.Add(b);
            }

            //book to base reccomendation on
            Book book = booksinorder[0];

            //DO NOT include books customer has already ordered
            List<OrderDetail> pastorderdetails = new List<OrderDetail>();
            foreach (Order o in pastorders)
            {
                List<OrderDetail> odlist = o.OrderDetails.ToList();

                foreach (OrderDetail ood in odlist)
                {
                    pastorderdetails.Add(ood);
                }
            }

            List<Book> pastbooks = new List<Book>();
            foreach (OrderDetail od in pastorderdetails)
            {
                pastbooks.Add(od.Book);
            }

            List<Book> allbooks = _context.Books.ToList();

            //books that haven't been bought by the user
            var eligiblebooks = allbooks.Except(pastbooks);

            //1 Author has book of same genre - highest rating

            //list of books by the same author
            List<Book> authorbooks = _context.Books.Include(b => b.Genre).
                                             Where(b => b.Author == book.Author && b.Genre == book.Genre).ToList();

            //add highest rated book by author if they have a book in the same genre
            if (authorbooks != null)
            {
                List<Book> sortedauthorbooks = authorbooks.OrderBy(o => o.decAverageRating).ToList();
                reccomendedbooks.Add(sortedauthorbooks[0]);
            }

            //2 & 3 High rated books of same genre (different authors) (4 or above ratings)
            List<Book> genrebooks = _context.Books.Include(b => b.Genre).Where(b => b.Genre == book.Genre).ToList();

            //add books of same genre with 4 or above ratings
            List<Book> genreratings = new List<Book>();
            foreach (Book b in genrebooks)
            {
                if (b.decAverageRating >= 4)
                {
                    reccomendedbooks.Add(b);
                    genreratings.Add(b);
                }
            }

            //other books in same genre
            foreach (Book b in genrebooks)
            {
                if (b.decAverageRating < 4)
                {
                    reccomendedbooks.Add(b);
                }
            }

            //if no books in genre, fill with highest rated book in general
            List<Book> bookssorted = eligiblebooks.OrderBy(o => o.decAverageRating).ToList();

            //fill remaining spots
            Book highestrated = bookssorted[0];
            reccomendedbooks.Add(highestrated);

            Book highestrated2 = bookssorted[1];
            reccomendedbooks.Add(highestrated2);

            Book highestrated3 = bookssorted[2];
            reccomendedbooks.Add(highestrated3);

            List<Book> eligiblereccomendations = new List<Book>();
            foreach (Book b in reccomendedbooks.ToList())
            {
                if (eligiblebooks.Contains(b))
                {
                    eligiblereccomendations.Add(b);
                }
            }

            List<Book> reccomendations = new List<Book>();

            if (eligiblereccomendations.Count == 1)
            {
                reccomendations.Add(eligiblereccomendations[0]);
            }

            if (eligiblereccomendations.Count == 2)
            {
                reccomendations.Add(eligiblereccomendations[0]);
                reccomendations.Add(eligiblereccomendations[1]);
            }

            if (eligiblereccomendations.Count >= 3)
            {
                reccomendations.Add(eligiblereccomendations[0]);
                reccomendations.Add(eligiblereccomendations[1]);
                reccomendations.Add(eligiblereccomendations[2]);
            }

            return reccomendations;
        }

        private bool OrderExists(int id)
        {
            return _context.Orders.Any(e => e.OrderID == id);
        }
    }
}
