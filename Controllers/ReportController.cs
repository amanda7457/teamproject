using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Group14_BevoBooks.DAL;
using Group14_BevoBooks.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Group14_BevoBooks.Controllers
{
    [Authorize(Roles = "Manager")]
    public class ReportController : Controller
    {
        public enum BookSort { MostRecent, ProfitMarginA, ProfitMarginD, PriceA, PriceD, MostPopular };
        public enum OrderSort { MostRecent, ProfitMarginA, ProfitMarginD, PriceA, PriceD };
        public enum CustomerSort { ProfitMarginA, ProfitMarginD, RevenueA, RevenueD };

        private AppDbContext _context;

        public ReportController(AppDbContext context)
        {
            _context = context;

        }

        public IActionResult Index()
        {
            return View();
        }


        //TODO: Sorting options: most recent first, profit margin (ascending and descending), price (ascending and descending), and most popular (highest quantity sold).
        public IActionResult AllBooksSold(int? SelectedSort)
        {
            List<Order> ordersplaced = _context.Orders.Include(b => b.AppUser).Include(m => m.OrderDetails).ThenInclude(m => m.Book).
                ThenInclude(b => b.Genre).Where(o => o.OrderPlaced == true).ToList();


            List<OrderDetail> allorderdetails = new List<OrderDetail>();

            foreach (Order o in ordersplaced)
            {
                foreach (OrderDetail od in o.OrderDetails)
                {
                    allorderdetails.Add(od);
                }
            }

            if (SelectedSort == 1)
            {
                List<OrderDetail> SortedRecent = allorderdetails.OrderBy(o => o.Order.OrderDate).ToList();

                ViewBag.ReportCount = allorderdetails.Count();
                ViewBag.BookSort = GetBookSort();
                return View(SortedRecent);
            }

            if (SelectedSort == 2)
            {
                List<OrderDetail> SortedProfitA = allorderdetails.OrderBy(o => o.ProfitMargin).ToList();

                ViewBag.ReportCount = allorderdetails.Count();
                ViewBag.BookSort = GetBookSort();
                return View(SortedProfitA);
            }

            if (SelectedSort == 3)
            {
                List<OrderDetail> SortedProfitD = allorderdetails.OrderByDescending(o => o.ProfitMargin).ToList();

                ViewBag.ReportCount = allorderdetails.Count();
                ViewBag.BookSort = GetBookSort();
                return View(SortedProfitD);
            }

            if (SelectedSort == 4)
            {
                List<OrderDetail> SortedPriceA = allorderdetails.OrderBy(o => o.Price).ToList();

                ViewBag.ReportCount = allorderdetails.Count();
                ViewBag.BookSort = GetBookSort();
                return View(SortedPriceA);
            }

            if (SelectedSort == 5)
            {
                List<OrderDetail> SortedPriceD = allorderdetails.OrderByDescending(o => o.Price).ToList();

                ViewBag.ReportCount = allorderdetails.Count();
                ViewBag.BookSort = GetBookSort();
                return View(SortedPriceD);
            }

            if (SelectedSort == 6)
            {
                List<OrderDetail> SortedPopularity = allorderdetails.OrderByDescending(o => o.Book.intPopularity).ToList();

                ViewBag.ReportCount = allorderdetails.Count();
                ViewBag.BookSort = GetBookSort();
                return View(SortedPopularity);
            }



            ViewBag.ReportCount = allorderdetails.Count();
            ViewBag.BookSort = GetBookSort();

            return View(allorderdetails);

        }

        public IActionResult AllOrders()
        {
            List<Order> ordersplaced = _context.Orders.Include(b => b.AppUser).Include(m => m.OrderDetails).ThenInclude(m => m.Book).
                ThenInclude(b => b.Genre).Where(o => o.OrderPlaced == true && User.IsInRole("Customer")).ToList();


            ViewBag.ReportCount = ordersplaced.Count();
            ViewBag.OrderSort = GetOrderSort();

            return View(ordersplaced);
        }

        public IActionResult AllCustomers()
        {

            List<AppUser> allcustomers = _context.Users.Include(u => u.Orders).ThenInclude(o => o.OrderDetails)
                        .ThenInclude(od => od.Book).ToList();

            ViewBag.ReportCount = allcustomers.Count();
            ViewBag.CustomerSort = GetCustomerSort();

            return View(allcustomers);
        }

        public IActionResult Totals()
        {
            //total cost, total profit and total revenue -> get all od and all books???
            List<OrderDetail> allorderdetails = _context.OrderDetails.Include(o => o.Order).Include(o => o.Book).
                    ThenInclude(o => o.BookOrders).Where(o => o.Order.OrderPlaced == true).ToList();

            //total revenue = num of each od extended price
            Decimal totalrevenue = allorderdetails.Sum(bo => bo.ExtendedPrice);

            Decimal totalcost = allorderdetails.Sum(bo => bo.Cost);

            Decimal totalprofit = totalrevenue - totalcost;

            ViewBag.TotalRevenue = totalrevenue.ToString("C");
            ViewBag.TotalCost = totalcost.ToString("C");
            ViewBag.TotalProfit = totalprofit.ToString("C");

            return View();

        }

        //Current inventory – shows the book title, the number of books in inventory, and the average cost for each book title. 
        //At the bottom of this report, you should show the total value of current inventory.
        public IActionResult Inventory()
        {
            List<Book> booksinventory = _context.Books.Include(b => b.OrderDetails).Include(b => b.BookOrders).
                    Where(b => b.InStock == true).ToList();

            Decimal totalvalue = booksinventory.Sum(bo => bo.AverageCost * bo.Inventory);
            ViewBag.TotalValue = totalvalue.ToString("C");

            int totalbooks = booksinventory.Sum(bo => bo.Inventory);
            ViewBag.ReportCount = totalbooks;

            return View(booksinventory);
        }

        public SelectList GetBookSort()
        {
            var booksorts = from BookSort b in Enum.GetValues(typeof(BookSort))
                            select new { ID = (int)b, Name = b.ToString() };

            SelectList sortselect = new SelectList(booksorts, "ID", "Name");

            return sortselect;
        }

        public SelectList GetOrderSort()
        {
            var ordersorts = from OrderSort b in Enum.GetValues(typeof(OrderSort))
                             select new { ID = (int)b, Name = b.ToString() };

            SelectList sortselect = new SelectList(ordersorts, "ID", "Name");

            return sortselect;
        }

        public SelectList GetCustomerSort()
        {
            var customersorts = from CustomerSort b in Enum.GetValues(typeof(CustomerSort))
                                select new { ID = (int)b, Name = b.ToString() };

            SelectList sortselect = new SelectList(customersorts, "ID", "Name");

            return sortselect;
        }
    }
}
