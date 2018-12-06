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
//TODO: change this to work with shipping


namespace Group14_BevoBooks.Controllers
{
    [Authorize(Roles = "Manager")]
    public class ShippingController : Controller
    {
        private readonly AppDbContext _context;

        public ShippingController(AppDbContext context)
        {
            _context = context;
        }

        // GET: CreditCard/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Shipping/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(int id, [Bind("ShippingID, ShippingFirst, ShippingAdditional")] Shipping shipping)
        {
            if (ModelState.IsValid)
            {
                AppUser user = _context.Users.FirstOrDefault(u => u.UserName == User.Identity.Name);
                shipping.ManagerSet = user;

                _context.ShippingPrices.Add(shipping);

                await _context.SaveChangesAsync();
                return RedirectToAction("Index", "Order");
            }
            return View(shipping);
        }

        // GET: CreditCard/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            //if cc does not exist in db create one

            if (id == null)
            {
                return NotFound();
            }

            var shipping = await _context.ShippingPrices.FindAsync(id);
            if (shipping == null)
            {
                return NotFound();
            }
            return View(shipping);
        }

        // POST: CreditCard/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ShippingID, ShippingFirst, ShippingAdditional")] Shipping shipping)
        {
            if (id != shipping.ShippingID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(shipping);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ShippingExists(shipping.ShippingID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Index", "Order");
            }
            return View(shipping);
        }

        private bool ShippingExists(int id)
        {
            return _context.ShippingPrices.Any(c => c.ShippingID == id);
        }
    }
}
