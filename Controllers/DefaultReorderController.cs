using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Group14_BevoBooks.DAL;
using Group14_BevoBooks.Models;

namespace Group14_BevoBooks.Controllers
{
    public class DefaultReorderController : Controller
    {
        private readonly AppDbContext _context;

        public DefaultReorderController(AppDbContext context)
        {
            _context = context;
        }

        // GET: DefaultReorder
        public async Task<IActionResult> Index()
        {
            return View("Index", "BookOrder");
        }

        // GET: DefaultReorder/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var defaultReorder = await _context.ReorderQuantity
                .FirstOrDefaultAsync(m => m.DefaultReorderID == id);
            if (defaultReorder == null)
            {
                return NotFound();
            }

            return View(defaultReorder);
        }

        // GET: DefaultReorder/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: DefaultReorder/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("DefaultReorderID,DefaultQuantity")] DefaultReorder defaultReorder)
        {
            if (ModelState.IsValid)
            {
                defaultReorder.ManagerSet = _context.Users.FirstOrDefault(u => u.UserName == User.Identity.Name);

                _context.Add(defaultReorder);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(defaultReorder);
        }

        // GET: DefaultReorder/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var defaultReorder = await _context.ReorderQuantity.FindAsync(id);
            if (defaultReorder == null)
            {
                return NotFound();
            }
            return View(defaultReorder);
        }

        // POST: DefaultReorder/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("DefaultReorderID,DefaultQuantity")] DefaultReorder defaultReorder)
        {
            if (id != defaultReorder.DefaultReorderID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(defaultReorder);

                    SetDefaultQuantity();

                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DefaultReorderExists(defaultReorder.DefaultReorderID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }

                return RedirectToAction("AutoReorder", "BookOrder");
            }

            return View(defaultReorder);
        }

        // GET: DefaultReorder/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var defaultReorder = await _context.ReorderQuantity
                .FirstOrDefaultAsync(m => m.DefaultReorderID == id);
            if (defaultReorder == null)
            {
                return NotFound();
            }

            return View(defaultReorder);
        }

        // POST: DefaultReorder/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var defaultReorder = await _context.ReorderQuantity.FindAsync(id);
            _context.ReorderQuantity.Remove(defaultReorder);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DefaultReorderExists(int id)
        {
            return _context.ReorderQuantity.Any(e => e.DefaultReorderID == id);
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
    }
}
