using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Group14_BevoBooks.DAL;
using Group14_BevoBooks.Models;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Group14_BevoBooks.Controllers
{
    public class HomeController : Controller

    {
        private readonly AppDbContext _context;

        public HomeController(AppDbContext context)
        {
            _context = context;
        }

        // GET: /<controller>/
        public IActionResult Index()
        {

            if (User.Identity.IsAuthenticated)
            {
                AppUser user = _context.Users.FirstOrDefault(u => u.UserName == User.Identity.Name);
                ViewBag.Name = user.FirstName;
            }
                
            SetActive();
            SetActiveDiscount();

            if (User.IsInRole("Customer"))
            {
                AppUser user = _context.Users.FirstOrDefault(u => u.UserName == User.Identity.Name);
                ViewBag.Name = user.FirstName;

                List<Discount> discounts = GetActiveDiscounts();
                return View(discounts);
            }

            return View();
        }

        public List<Discount> GetActiveDiscounts()
        {
            List<Discount> discountlist = _context.Discounts.ToList();

            List<Discount> activediscounts = new List<Discount>();
            foreach (Discount d in discountlist)
            {
                if (d.Active == true)
                {
                    activediscounts.Add(d);
                }
            }

            return activediscounts;
        }

        public void SetActiveDiscount()
        {
            DateTime today = System.DateTime.Today;

            List<Discount> discountlist = _context.Discounts.ToList();

            foreach (Discount d in discountlist)
            {
                if (d.DiscountEndDate.Date >= today.Date && d.DiscountStartDate.Date <= today.Date)
                {
                    d.Active = true;
                }

                else
                {
                    d.Active = false;
                }
            }

            _context.SaveChanges();
        }

        public void SetActive()
        {
            List<Book> allbooks = _context.Books.ToList();

            foreach (Book b in allbooks)
            {
                if (b.Inventory < 1 || b.Discontinued == true)
                {
                    b.Active = false;
                }
                else
                {
                    b.Active = true;
                }
            }

            _context.SaveChanges();
        }
    }
}
