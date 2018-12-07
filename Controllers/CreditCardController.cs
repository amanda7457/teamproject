using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Group14_BevoBooks.DAL;
using Microsoft.AspNetCore.Mvc;
using Group14_BevoBooks.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Rendering;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Group14_BevoBooks.Controllers
{
    [Authorize]
    public class CreditCardController : Controller
    {
        private readonly AppDbContext _context;

        public CreditCardController(AppDbContext context)
        {
            _context = context;
        }

        // GET: CreditCard/Create
        public IActionResult Create()
        {
            AppUser user = _context.Users.Include(u => u.CreditCards).FirstOrDefault(u => u.UserName == User.Identity.Name);

            List<CreditCard> userccs = user.CreditCards.ToList();

            if (userccs.Count >= 3)
            {
                return View("Error", new string[] { "You cannot store more then 3 Credit Cards - please edit an existing card" });
            }

            ViewBag.CardList = GetCardTypes();
            return View();
        }

        // POST: CreitCard/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(int id, [Bind("CreditCardID, CardNumber")] CreditCard creditcard, int SelectedCardType)
        {
            if (ModelState.IsValid)
            {
                AppUser user = _context.Users.Include(u => u.CreditCards).FirstOrDefault(u => u.UserName == User.Identity.Name);

                creditcard.AppUser = user;

                CardType type;
                if (SelectedCardType == 0)
                {
                    type = CardType.Visa;
                    creditcard.CardType = type;
                }

                if (SelectedCardType == 1)
                {
                    type = CardType.AmericanExpress;
                    creditcard.CardType = type;
                }

                if (SelectedCardType == 2)
                {
                    type = CardType.MasterCard;
                    creditcard.CardType = type;
                }

                if (SelectedCardType == 3)
                {
                    type = CardType.Discover;
                    creditcard.CardType = type;
                }

                _context.Add(creditcard);

                _context.Update(user);
                _context.SaveChanges();

                return RedirectToAction("Cart", "Order");
            }
            return View(creditcard);
        }

        // GET: CreditCard/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            //if cc does not exist in db create one

            if (id == null)
            {
                return NotFound();
            }

            CreditCard creditcard = _context.CreditCards.Include(c => c.AppUser).FirstOrDefault(c => c.CreditCardID == id);
            if (creditcard == null)
            {
                return NotFound();
            }

            ViewBag.CardList = GetCardTypes();
            return View(creditcard);
        }

        // POST: CreditCard/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("CreditCardID, CardNumber")] CreditCard creditcard, int SelectedCardType)
        //public async Task<IActionResult> Edit(int id, string CardNumber)
        {
            if (id != creditcard.CreditCardID)
            {
                return NotFound();
            }

            CardType type;
            if (SelectedCardType == 0)
            {
                type = CardType.Visa;
                creditcard.CardType = type;
            }

            if (SelectedCardType == 1)
            {
                type = CardType.AmericanExpress;
                creditcard.CardType = type;
            }

            if (SelectedCardType == 2)
            {
                type = CardType.MasterCard;
                creditcard.CardType = type;
            }

            if (SelectedCardType == 3)
            {
                type = CardType.Discover;
                creditcard.CardType = type;
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(creditcard);

                    await _context.SaveChangesAsync();

                    return RedirectToAction("Cart", "Order");

                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CreditCardExists(creditcard.CreditCardID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            }
            return View(creditcard);
        }

        public SelectList GetCardTypes()
        {
            var cardtypes = from CardType b in Enum.GetValues(typeof(CardType))
                                select new { ID = (int)b, Name = b.ToString() };

            SelectList sortselect = new SelectList(cardtypes, "ID", "Name");

            return sortselect;
        }

        private bool CreditCardExists(int id)
        {
            return _context.Books.Any(e => e.BookID == id);
        }
    }
}
