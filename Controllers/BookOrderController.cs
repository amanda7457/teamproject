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

namespace Group14_BevoBooks.Controllers
{
    [Authorize(Roles = "Manager")]
    public class BookOrderController : Controller
    {
        private readonly AppDbContext _context;

        public BookOrderController(AppDbContext context)
        {
            _context = context;
        }

        // GET: BookOrder
        public async Task<IActionResult> Index()
        {
            return View();
        }

        public void DeleteAllSupplierOrders()
        {
            List<BookOrder> allbookorders = _context.BookOrders.ToList();

            foreach(BookOrder bo in allbookorders)
            {
                _context.Remove(bo);
            }
            _context.SaveChanges();
        }

        public async Task<IActionResult> BookOrderList()
        {
            return View( await _context.BookOrders.Include(b => b.Book).Include(b => b.AppUser).
                        Where(b => b.Status == Status.Ordered).ToListAsync());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ChangeDefault(int DefaultValue)
        {
            DefaultReorder reorder = _context.ReorderQuantity.FirstOrDefault(u => u.DefaultReorderID == 1);
            reorder.DefaultQuantity = DefaultValue;

            //TempData["quantity"] = DefaultValue;

            return RedirectToAction("AutoRorder");
        }

        //POST : BookOrder/Create
        public IActionResult CreateAuto()
        {
            AppUser user = _context.Users.FirstOrDefault(u => u.UserName == User.Identity.Name);
            DefaultReorder reorder = _context.ReorderQuantity.FirstOrDefault(u => u.DefaultReorderID == 1);

            List<Book> ReorderBooks = GetReorderBooks();

            //list of orders
            List<BookOrder> bookorders = new List<BookOrder>();

            //list of book orders in database
            List<BookOrder> boalreadyindb = _context.BookOrders.ToList();

            //list of books we already have book orders for
            List<Book> titlesbookorder = new List<Book>();

            foreach (BookOrder bookorder in boalreadyindb)
            {
                titlesbookorder.Add(bookorder.Book);
            }

            //list of order details
            foreach (Book b in ReorderBooks)
            {
                bool alreadyExist = titlesbookorder.Contains(b);

                //if book doesnt have order
                if (alreadyExist == false)
                {
                    //create orders
                    BookOrder bo = new BookOrder();
                    bo.AppUser = user;
                    bo.ReorderQuantity = reorder;

                    bo.Book = b;
                    bo.Quantity = bo.ReorderQuantity.DefaultQuantity;
                    bo.Price = b.SupplierPrice;
                    bo.InReorderList = true;

                    bookorders.Add(bo);
                    _context.BookOrders.Add(bo);
                }
            }

            _context.SaveChanges();

            return RedirectToAction("AutoReorder");
        }

        public void ReorderListSet()
        {
            //get books to reorder
            List<Book> reorders = GetReorderBooks();

            List<BookOrder> autobookorders = _context.BookOrders.Include(o => o.Book).ToList();
            foreach (BookOrder bo in autobookorders)
            {
                Book book = bo.Book;
                bool inboth = reorders.Contains(book);

                //if book doesnt have order
                if (inboth == true)
                {
                    //if book has an order and is at the reorder point
                    bo.InReorderList = true;
                }

                if (bo.Status == Status.Ordered)
                {
                    int qafterorder = bo.Quantity + bo.Book.Inventory;

                    if (qafterorder > bo.Book.Reorder)
                    {
                        bo.InReorderList = false;
                    }
                }

                _context.SaveChanges();
            }

        }

        public void SetDefaultQuantity()
        {
            DefaultReorder reorder = _context.ReorderQuantity.FirstOrDefault(u => u.DefaultReorderID == 1);
            List<BookOrder> autobookorders = _context.BookOrders.Include(o => o.Book).Where(b => b.InReorderList == true).ToList();

            foreach (BookOrder bo in autobookorders)
            {
                if (bo.ReorderQuantity == reorder)
                {
                    bo.Quantity = bo.ReorderQuantity.DefaultQuantity;
                    bo.Quantity = reorder.DefaultQuantity;
                    _context.Update(bo);
                }
            }

            _context.SaveChanges();
        }

        // GET: AutoOrder
        public ActionResult AutoReorder()
        {
            ReorderListSet();

            DefaultReorder reorder = _context.ReorderQuantity.FirstOrDefault(u => u.DefaultReorderID == 1);

            List<BookOrder> autobookorders = _context.BookOrders.Include(o => o.Book).Where(b => b.InReorderList == true).ToList();

            foreach (BookOrder bo in autobookorders)
            {
                if (bo.Status == Status.Delivered)
                {
                    bo.InReorderList = false;
                    _context.Update(bo);
                }
            }

            _context.SaveChanges();

            List<BookOrder> updatedautoorders = _context.BookOrders.Include(o => o.Book).Where(b => b.InReorderList == true).ToList();

            ViewBag.ReorderID = reorder.DefaultReorderID;

            return View(updatedautoorders);
        }

        // POST: BookOrder/PlaceAutoOrder
        //[HttpPost]
        public IActionResult PlaceAutoOrder()
        {
            AppUser user = _context.Users.FirstOrDefault(u => u.UserName == User.Identity.Name);
            List<BookOrder> autobookorders = _context.BookOrders.Include(b => b.Book).Where(b => b.InReorderList == true).ToList();

            foreach (BookOrder bo in autobookorders)
            {
                int qafterorder = bo.Quantity + bo.Book.Inventory;

                if(qafterorder > bo.Book.Reorder)
                {
                    bo.InReorderList = false;
                }
              
                //method to set nnew supplier price
                SetSupplierPrice(bo.Book.BookID, bo.Price);

                bo.AppUser = user;
                bo.Status = Status.Ordered;

                _context.Update(bo);
            }

            _context.SaveChanges();

            return View("Confirm");
        }

        public void SetSupplierPrice(int bookid, decimal boprice)
		{
            Book book = _context.Books.Find(bookid);

            book.SupplierPrice = boprice;

            _context.SaveChanges();
		}

        // GET: BookOrder/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var bookOrder = await _context.BookOrders
                .FirstOrDefaultAsync(m => m.BookOrderID == id);
            if (bookOrder == null)
            {
                return NotFound();
            }

            return View(bookOrder);
        }

        // GET: BookOrder/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: BookOrder/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("BookOrderID,Status")] BookOrder bookOrder, int bookid)
        {
            AppUser user = _context.Users.FirstOrDefault(u => u.UserName == User.Identity.Name);
            Book book = _context.Books.Find(bookid);

            if (ModelState.IsValid)
            {
                bookOrder.AppUser = user;
                bookOrder.Book = book;

                _context.BookOrders.Add(bookOrder);
                await _context.SaveChangesAsync();

                return RedirectToAction("AddToOrder", new { id = bookOrder.BookOrderID });
            }

            return RedirectToAction(nameof(Index));
        }


        [Authorize]
        //GET: /Order/AddToOrder
        public async Task<IActionResult> AddToOrder(int id)
        {
            //find order in db
            BookOrder bookorder = _context.BookOrders.Include(b => b.Book).Include(b => b.AppUser).FirstOrDefault(b => b.BookOrderID == id);

            await _context.SaveChangesAsync();

            //TODO: change view to be bookorder
            return View(bookorder);
        }


        //POST: /BookOrder/AddToOrder
        [HttpPost]
        public async Task<IActionResult> AddToOrder(BookOrder bo, int Quantity)
        {
            BookOrder bookorder = _context.BookOrders.Include(b => b.Book).Include(b => b.AppUser).FirstOrDefault(b => b.BookOrderID == bo.BookOrderID);
            Book book = bookorder.Book;

            if (ModelState.IsValid)
            {
                //quantity equal to selected quantity
                bookorder.Quantity = Quantity;

                //set the product price for this detail equal to the current price
                //TODO: make this price supplier price default but allow maager to change
                bookorder.Price = book.SupplierPrice;

                //order not placed yet
                bookorder.Status = Status.Pending;
                bookorder.InReorderList = false;

                await _context.SaveChangesAsync();

                return RedirectToAction("SupplierCart");

            }

            return View("Index", "Book");
        }

        // GET: Order/SupplierCart
        public ActionResult SupplierCart(int? id)
        {

            //list of pending book orders that arent in reorder list (manual pending orders bc only auto are in reorder list)

            List<BookOrder> bookorders = _context.BookOrders.Include(o => o.AppUser).Include(o => o.Book).
                                                 Where(o => o.InReorderList == false && o.Status == Status.Pending).ToList();

            if (User.IsInRole("Manager") == false)
            {
                return View("Error", new string[] { "You are not authorized to view this page." });
            }

            //make sure prices that are shows are the most updated
            //foreach (BookOrder bo in bookorders)
            //{
                //Book book = bo.Book;
                //bo.Price = book.SupplierPrice;
            //}

            _context.SaveChangesAsync();

            return View(bookorders);
        }

        // POST: BookOrder/PlaceAutoOrder
        //[HttpPost]
        public IActionResult PlaceManualOrder()
        {
            AppUser user = _context.Users.FirstOrDefault(u => u.UserName == User.Identity.Name);
            List<BookOrder> manualbookorders = _context.BookOrders.Include(b => b.Book).
                                                       Where(b => b.InReorderList == false && b.Status == Status.Pending).ToList();

            foreach (BookOrder bo in manualbookorders)
            {
                bo.AppUser = user;
                bo.Status = Status.Ordered;

                _context.Update(bo);
            }

            _context.SaveChanges();

            return View("Confirm");
        }



        //Get BookOrder/BookArrival
        public IActionResult BookArrival(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            BookOrder bookorder = _context.BookOrders.Include(b => b.Book).Include(b => b.AppUser)
                                          .FirstOrDefault(b => b.BookOrderID == id);

            if (bookorder == null)
            {
                return NotFound();
            }

            return View(bookorder);
        }

        public IActionResult BookCheckIn(int? id, int NewQuantity)
        {
            BookOrder bookorder = _context.BookOrders.Include(b => b.Book).Include(b => b.AppUser)
                  .FirstOrDefault(b => b.BookOrderID == id);

            int ActualQuantity;
            //don't let supplier give ou too many copies
            if (NewQuantity > bookorder.Quantity)
            {
                ActualQuantity = bookorder.Quantity;
            }
            else
            {
                ActualQuantity = NewQuantity;
            }

            bookorder.Quantity = bookorder.Quantity - ActualQuantity;

            bookorder.Book.Inventory = bookorder.Book.Inventory + ActualQuantity;

            //if number new copies equals number ordered take it off the order list
            if (bookorder.Quantity == 0)
            {
                bookorder.Status = Status.Delivered;
            }

            _context.SaveChanges();

            return RedirectToAction("BookOrderList");
        }

        // GET: BookOrder/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var bookOrder = await _context.BookOrders.FindAsync(id);
            if (bookOrder == null)
            {
                return NotFound();
            }
            return View(bookOrder);
        }

        // POST: BookOrder/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("BookOrderID,Status,Quantity,Price")] BookOrder bookOrder)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(bookOrder);
                    _context.SaveChanges();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BookOrderExists(bookOrder.BookOrderID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }

                _context.SaveChanges();
                return RedirectToAction("BookOrderList");
            }

            return View(bookOrder);
        }

        // GET: BookOrder/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var bookOrder = await _context.BookOrders.Include(b => b.Book)
                .FirstOrDefaultAsync(m => m.BookOrderID == id);
            if (bookOrder == null)
            {
                return NotFound();
            }

            return View(bookOrder);
        }

        // POST: BookOrder/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var bookOrder = await _context.BookOrders.FindAsync(id);
            _context.BookOrders.Remove(bookOrder);
            await _context.SaveChangesAsync();
            return RedirectToAction("SupplierCart");
        }

        private bool BookOrderExists(int id)
        {
            return _context.BookOrders.Any(e => e.BookOrderID == id);
        }

        public List<Book> GetAllBooks()

        {
            List<Book> Books = _context.Books.ToList();

            return Books;
        }

        public List<Book> GetReorderBooks()
        {
            List<Book> AllBooks = GetAllBooks();

            List<Book> ReorderBooks = new List<Book>();

            //list of book beloew reorder point
            foreach (Book b in AllBooks)
            {
                if (b.Inventory <= b.Reorder)
                {
                    ReorderBooks.Add(b);
                }
            }

            return ReorderBooks;
        }

        public Int32 GetBookOrderNumber()
        {
            //variables
            List<BookOrder> bookorderlist = new List<BookOrder>();
            BookOrder lastbookorder;

            try
            {
                //find most recent order
                bookorderlist = _context.BookOrders.Where(o => o.Status == Status.Pending).ToList();
                lastbookorder = bookorderlist.LastOrDefault();
            }
            catch
            {
                bookorderlist = new List<BookOrder>();
                lastbookorder = null;
            }


            bool isEmpty = !bookorderlist.Any();
            if (isEmpty == true)
            {
                //if list is empty return this value
                Int32 ordernumber = -1;
                return ordernumber;
            }
            else
            {
                Int32 ordernumber = lastbookorder.BookOrderID;
                return ordernumber;
            }
        }

    }
}
