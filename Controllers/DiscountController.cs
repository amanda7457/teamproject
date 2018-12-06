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
    public class DiscountController : Controller
    {
        private readonly AppDbContext _context;

        public DiscountController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Discount
        public async Task<IActionResult> Index()
        {
            return View(await _context.Discounts.ToListAsync());
        }

        // GET: Discount/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var discount = await _context.Discounts
                .FirstOrDefaultAsync(m => m.DiscountID == id);
            if (discount == null)
            {
                return NotFound();
            }

            return View(discount);
        }

        // GET: Discount/Create
        public IActionResult Create()
        {
            ViewBag.DiscountTypes = GetDiscountTypes();
            return View();
        }

        // POST: Discount/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("DiscountID,PromoCode,DiscountAmount,DiscountStartDate,DiscountEndDate")] Discount discount, DiscountType SelectedDiscountType)
        {
            //AppUser user = _context.Users.FirstOrDefault(u => u.UserName == User.Identity.Name);

            //list of all discounts in db
            List<Discount> discounts = _context.Discounts.ToList();

            //list of all promo codes
            List<string> promocodes = new List<string>();
            foreach (Discount d in discounts)
            {
                string promocode = d.PromoCode;

                promocodes.Add(promocode);
            }

            discount.DiscountType = SelectedDiscountType;

            if (promocodes.Contains(discount.PromoCode))
            {
                ViewBag.Message = "This code has already been used - please try again.";
                ViewBag.DiscountTypes = GetDiscountTypes();

                return View(discount);
            }

            if (ModelState.IsValid)
            {
                _context.Add(discount);
                await _context.SaveChangesAsync();

                if (discount.DiscountType == DiscountType.FreeShipping)
                {
                    ViewBag.DiscountTypes = GetDiscountTypes();
                    return RedirectToAction("CreateFreeShipping", new { id = discount.DiscountID });
                }

                if (discount.DiscountType == DiscountType.PercentOff)
                {
                    ViewBag.DiscountTypes = GetDiscountTypes();
                    return RedirectToAction("CreatePercent", new { id = discount.DiscountID });
                }

                return RedirectToAction(nameof(Index));
            }

            ViewBag.DiscountTypes = GetDiscountTypes();
            return View(discount);
        }

        public IActionResult CreateFreeShipping(int? id)
        {
            Discount discount = _context.Discounts.Find(id);

            return View(discount);
        }


        public IActionResult CreatePercent(int? id)
        {
            Discount discount = _context.Discounts.Find(id);

            return View(discount);
        }

        public IActionResult CreateDiscount(int? DiscountID, Decimal DiscountAmount)
        {
            Discount discount = _context.Discounts.Find(DiscountID);

            discount.DiscountAmount = DiscountAmount;

            _context.SaveChanges();

            return View("Confirm");
        }

        // GET: Discount/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var discount = await _context.Discounts.FindAsync(id);
            if (discount == null)
            {
                return NotFound();
            }
            return View(discount);
        }

        // POST: Discount/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("DiscountID,PromoCode,DiscountAmount,DiscountStartDate,DiscountEndDate,DiscountType")] Discount discount)
        {
            if (id != discount.DiscountID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(discount);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DiscountExists(discount.DiscountID))
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
            return View(discount);
        }

        // GET: Discount/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var discount = await _context.Discounts
                .FirstOrDefaultAsync(m => m.DiscountID == id);
            if (discount == null)
            {
                return NotFound();
            }

            return View(discount);
        }

        // POST: Discount/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var discount = await _context.Discounts.FindAsync(id);
            _context.Discounts.Remove(discount);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DiscountExists(int id)
        {
            return _context.Discounts.Any(e => e.DiscountID == id);
        }

        public SelectList GetDiscountTypes()
        {
            List<DiscountType> discounts = new List<DiscountType>();

            discounts.Add(DiscountType.FreeShipping);
            discounts.Add(DiscountType.PercentOff);

            //convert list to select list
            SelectList discounttypes = new SelectList(discounts);

            //return the select list
            return discounttypes;
        }

        public void SetActiveDiscount()
        {
            DateTime today = System.DateTime.Today;

            List<Discount> discountlist = _context.Discounts.ToList();

            foreach (Discount d in discountlist)
            {
                if (d.DiscountEndDate >= today)
                {
                    d.Active = false;
                }

                else
                {
                    d.Active = true;
                }
            }

            _context.SaveChanges();
        }
    }
}
