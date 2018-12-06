using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Group14_BevoBooks.DAL;
using Group14_BevoBooks.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Group14_BevoBooks.Controllers
{
    [Authorize(Roles = "Manager")]
    public class ReportController : Controller
    {
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
        public IActionResult AllBooksSold()
        {
            List<Order> ordersplaced = _context.Orders.Include(b => b.AppUser).Include(m => m.OrderDetails).ThenInclude(m => m.Book).
                ThenInclude(b => b.Genre).Where(o => o.OrderPlaced == true).ToList();


            List<OrderDetail> allorderdetails = new List<OrderDetail>();

            foreach(Order o in ordersplaced)
            {
                foreach(OrderDetail od in o.OrderDetails)
                {
                    allorderdetails.Add(od);
                }
            }

            ViewBag.ReportCount = allorderdetails.Count();

            return View(allorderdetails);

        }

        public IActionResult AllOrders()
        {
            List<Order> ordersplaced = _context.Orders.Include(b => b.AppUser).Include(m => m.OrderDetails).ThenInclude(m => m.Book).
                ThenInclude(b => b.Genre).Where(o => o.OrderPlaced == true && User.IsInRole("Customer")).ToList();


            ViewBag.ReportCount = ordersplaced.Count();

            return View(ordersplaced);
        }

        public IActionResult AllCustomers()
        {

            List<AppUser> allcustomers = _context.Users.Include(u => u.Orders).ThenInclude(o => o.OrderDetails)
                        .ThenInclude(od => od.Book).ToList();

            ViewBag.ReportCount = allcustomers.Count();

            return View(allcustomers);
        }

        public IActionResult Totals()
        {
            //total cost, total profit and total revenue -> get all od and all books???
            List<OrderDetail> allorderdetails = _context.OrderDetails.Include(o => o.Order).Include(o => o.Book).ToList();

            //total revenue = num of each od extended price
            Decimal totalrevenue = allorderdetails.Sum(bo => bo.ExtendedPrice);

            Decimal totalcost = allorderdetails.Sum(bo => bo.Cost);

            Decimal totalprofit = totalrevenue - totalcost;

            ViewBag.TotalRevenue = totalrevenue.ToString("C");
            ViewBag.TotalCost = totalcost.ToString("C");
            ViewBag.TotalProfit = totalprofit.ToString("C");

            return View();

        }

    }
}
