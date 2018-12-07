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
using Group14_BevoBooks.Utilities;

//TODO: make creating a book genertae book number utility and add

namespace Group14_BevoBooks.Controllers
{
    [Authorize(Roles = "Manager")]
    public class BookController : Controller
    {
        private readonly AppDbContext _context;

        public BookController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Book
        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            List<Book> allbooklist = await _context.Books.Include(b => b.Genre).ToListAsync();

            if (User.IsInRole("Employee"))
            {
                return View("EmployeeIndex", allbooklist);
            }
            if (User.IsInRole("Manager"))
            {
                return View("ManagerIndex", allbooklist);
            }
            else
            {
                return View(allbooklist);
            }

        }

        [AllowAnonymous]
        // GET: Book/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            Boolean alreadyincart;
            if (id == null)
            {
                return NotFound();
            }

            Book book = await _context.Books.Include(b => b.Genre).Include(b => b.Reviews)
                .FirstOrDefaultAsync(m => m.BookID == id);

            if (book == null)
            {
                return NotFound();
            }

            int ordernumber = AlreadyInCart();
            if (ordernumber != -1)
            {
                Order cartorder = _context.Orders.Include(o => o.OrderDetails).ThenInclude(o => o.Book).FirstOrDefault(o => o.OrderID == ordernumber);

                List<Book> booksincart = new List<Book>();

                foreach (OrderDetail od in cartorder.OrderDetails)
                {
                    Book b = od.Book;
                    booksincart.Add(b);
                }

                if (booksincart.Contains(book))
                {
                    alreadyincart = true;
                }
                else
                {
                    alreadyincart = false;
                }
            }

            else
            {
                alreadyincart = false;
            }

            //different views depending on role
            if (User.IsInRole("Employee"))
            {
                return View("EmployeeDetails", book);
            }
            if (User.IsInRole("Manager"))
            {
                return View("ManagerDetails", book);
            }


            Boolean seereviewdetail = CanSeeReview(id);

            if (seereviewdetail == true)
            {
                if (alreadyincart == true)
                {
                    ViewBag.Message = "WARNING: This book is already in your cart.";
                }

                return View("DetailsReview", book);
            }


            if (alreadyincart == true)
            {
                ViewBag.Message = "WARNING: This book is already in your cart.";
            }
            return View(book);
        }

        [AllowAnonymous]
        // GET: Book/Details/5
        public async Task<IActionResult> EmployeeDetails(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var book = await _context.Books.Include(b => b.Genre)
                .FirstOrDefaultAsync(m => m.BookID == id);
            if (book == null)
            {
                return NotFound();
            }

            return View(book);
        }

        [Authorize(Roles = "Manager")]
        // GET: Book/Details/5
        public async Task<IActionResult> ManagerDetails(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var book = await _context.Books.Include(b => b.Genre)
                .FirstOrDefaultAsync(m => m.BookID == id);
            if (book == null)
            {
                return NotFound();
            }

            return View(book);
        }

        // GET: Book/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Book/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("BookID,UniqueID,Title,SellingPrice,SupplierPrice,Description,Author,Reorder,Active,Inventory,UnqiueID")] Book book)
        {
            if (ModelState.IsValid)
            {
                book.UniqueID = Utilities.GenerateNextBookNumber.GetNextBookNumber(_context);

                _context.Add(book);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(book);
        }

        // GET: Book/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var book = await _context.Books.FindAsync(id);
            if (book == null)
            {
                return NotFound();
            }
            return View(book);
        }

        // POST: Book/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("BookID,UniqueID,Title,SellingPrice,SupplierPrice,Description,Author,Reorder,Active,Inventory,UnqiueID")] Book book)
        {
            if (id != book.BookID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(book);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BookExists(book.BookID))
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
            return View(book);
        }

        // GET: Book/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var book = await _context.Books
                .FirstOrDefaultAsync(m => m.BookID == id);
            if (book == null)
            {
                return NotFound();
            }

            return View(book);
        }

        // POST: Book/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var book = await _context.Books.FindAsync(id);
            _context.Books.Remove(book);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BookExists(int id)
        {
            return _context.Books.Any(e => e.BookID == id);
        }

        public Boolean CanSeeReview(int? bookid)
        {
            AppUser user = _context.Users.Include(u => u.ReviewsWritten).ThenInclude(u => u.Book).
                        FirstOrDefault(u => u.UserName == User.Identity.Name);

            List<Order> orderlist = new List<Order>();

            //all orders for person logged in
            orderlist = _context.Orders.Include(o => o.AppUser).Include(m => m.OrderDetails).ThenInclude(o => o.Book).
                Where(o => o.AppUser == user).ToList();

            List<OrderDetail> orderdetails = new List<OrderDetail>();

            //all ods for each order
            foreach (Order o in orderlist)
            {
                List<OrderDetail> odlist = o.OrderDetails.ToList();

                foreach (OrderDetail ood in odlist)
                {
                    orderdetails.Add(ood);
                }
            }

            List<Book> books = new List<Book>();

            //books a person has ordered
            foreach (OrderDetail od in orderdetails)
            {
                books.Add(od.Book);
            }

            List<Review> reviewswritten = user.ReviewsWritten.ToList();

            List<Book> booksreviewed = new List<Book>();
            foreach (Review r in reviewswritten)
            {
                Book b = r.Book;
                booksreviewed.Add(b);
            }

            Book book = _context.Books.Find(bookid);

            //make a list of books i'be bought and haven't reviewed
            List<Book> result = books.Except(booksreviewed).ToList();

            if (result.Contains(book))
            {
                return true;
            }

            else
            {
                return false;
            }



        }

        public int AlreadyInCart()
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


        public void SetActive()
        {
            List<Book> allbooks = _context.Books.ToList();

            foreach (Book b in allbooks)
            {
                if (b.Inventory >= 1)
                {
                    b.Active = true;
                }
                else
                {
                    b.Active = false;
                }
            }
        }
    }
}
