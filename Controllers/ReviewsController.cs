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
    public class ReviewsController : Controller
    {
        private readonly AppDbContext _context;

        public ReviewsController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Reviews
        public async Task<IActionResult> Index()
        {
            List<Review> pendingreviews = _context.Reviews.Include(r => r.Author).Include(r => r.Book).Where(r => r.Approved == null).ToList();

            return View(pendingreviews);
        }

        // GET: Reviews/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var review = await _context.Reviews
                .FirstOrDefaultAsync(m => m.ReviewID == id);
            if (review == null)
            {
                return NotFound();
            }

            return View(review);
        }

        public IActionResult PreCreate(int? bookid)
        {
            TempData["bookid"] = bookid;
            Book book = _context.Books.Find(bookid);

            return View(book);
        }

        // GET: Reviews/Create
        public IActionResult Create(int bookid)
        {
            TempData["bookid"] = bookid;

            return View();
        }

        // POST: Reviews/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> CreateReview([Bind("ReviewID,Rating,ReviewText, Author, Book")] Review review)
        {

            AppUser user = _context.Users.FirstOrDefault(u => u.UserName == User.Identity.Name);

            int bookID = Convert.ToInt32(TempData["bookid"]);
            Book book = _context.Books.Find(bookID);

            if (ModelState.IsValid)
            {
                review.Author = user;
                review.Book = book;
                review.Approved = null;

                AppUser userperson = _context.Users.FirstOrDefault(u => u.UserName == User.Identity.Name);

                _context.Reviews.Add(review);
                _context.SaveChanges();

                if (User.IsInRole("Manager") == true)
                {
                    return RedirectToAction("Index", new { id = review.ReviewID });
                }
                if (User.IsInRole("Customer") == true)
                {
                    return RedirectToAction("Details", new { id = review.ReviewID });
                }
            }
            return View("Create", review);
        }


        // GET: Reviews/Edit/5
        [Authorize(Roles = "Employee, Manager")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var review = await _context.Reviews.FindAsync(id);
            if (review == null)
            {
                return NotFound();
            }
            return View(review);
        }

        // POST: Reviews/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Employee, Manager")]
        public async Task<IActionResult> Edit(int id, [Bind("ReviewID,Rating,ReviewText")] Review review)
        {
            if (id != review.ReviewID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(review);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ReviewExists(review.ReviewID))
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
            return View(review);
        }

        // GET: Reviews/Delete/5
        [Authorize(Roles = "Employee, Manager")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var review = await _context.Reviews
                .FirstOrDefaultAsync(m => m.ReviewID == id);
            if (review == null)
            {
                return NotFound();
            }

            return View(review);
        }




        // POST: Reviews/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Employee, Manager")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var review = await _context.Reviews.FindAsync(id);
            _context.Reviews.Remove(review);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ReviewExists(int id)
        {
            return _context.Reviews.Any(e => e.ReviewID == id);
        }

        [Authorize(Roles = "Employee, Manager")]
        public async Task<IActionResult> ApproveReview(int id)
        {
            AppUser user = _context.Users.FirstOrDefault(u => u.UserName == User.Identity.Name);

            Review review = _context.Reviews.Include(r => r.Approver).Include(b => b.Book).FirstOrDefault(r => r.ReviewID == id);
            if (id != review.ReviewID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    review.Approver = user;
                    review.Approved = true;

                    _context.Update(review);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ReviewExists(review.ReviewID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return View("ApproveView", review);
            }
            return View(review);
        }

        [Authorize(Roles = "Employee, Manager")]
        public async Task<IActionResult> RejectReview(int id)
        {
            AppUser user = _context.Users.FirstOrDefault(u => u.UserName == User.Identity.Name);

            Review review = _context.Reviews.Include(r => r.Approver).Include(b => b.Book).FirstOrDefault(r => r.ReviewID == id);
            if (id != review.ReviewID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    review.Approver = user;
                    review.Approved = false;

                    _context.Update(review);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ReviewExists(review.ReviewID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return View("ApproveView", review);
            }
            return View(review);
        }
    }
}





