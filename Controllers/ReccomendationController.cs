using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Group14_BevoBooks.DAL;
using Group14_BevoBooks.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Group14_BevoBooks.Controllers
{
    public class ReccomendationController : Controller
    {
        private AppDbContext _context;

        public ReccomendationController(AppDbContext context)
        {
            _context = context;

        }

        public IActionResult Index()
        {
            List<Book> reccomedations = ReccomendedBooks();

            return View(reccomedations);

        }

        public List<Book> ReccomendedBooks()
        {
            List<Book> reccomendedbooks = new List<Book>();

            //pick one book from last order
            List<Order> pastorders = _context.Orders.Include(m => m.OrderDetails).ThenInclude(m => m.Book).ThenInclude(b => b.Genre).
                                             Where(o => o.AppUser.UserName == User.Identity.Name && o.OrderPlaced == true).ToList();

            //the order that was just placed
            Order justplaced = pastorders.LastOrDefault();

            List<Book> booksinorder = new List<Book>();
            foreach (OrderDetail od in justplaced.OrderDetails.ToList())
            {
                Book b = od.Book;
                booksinorder.Add(b);
            }

            //book to base reccomendation on
            Book book = booksinorder[0];

            //DO NOT include books customer has already ordered
            List<OrderDetail> pastorderdetails = new List<OrderDetail>();
            foreach (Order o in pastorders)
            {
                List<OrderDetail> odlist = o.OrderDetails.ToList();

                foreach (OrderDetail ood in odlist)
                {
                    pastorderdetails.Add(ood);
                }
            }

            List<Book> pastbooks = new List<Book>();
            foreach (OrderDetail od in pastorderdetails)
            {
                pastbooks.Add(od.Book);
            }

            List<Book> allbooks = _context.Books.ToList();

            //books that haven't been bought by the user
            var eligiblebooks = allbooks.Except(pastbooks);

            //1 Author has book of same genre - highest rating

            //list of books by the same author
            List<Book> authorbooks = _context.Books.Include(b => b.Genre).
                                             Where(b => b.Author == book.Author && b.Genre == book.Genre).ToList();

            //add highest rated book by author if they have a book in the same genre
            if (authorbooks != null)
            {
                List<Book> sortedauthorbooks = authorbooks.OrderBy(o => o.decAverageRating).ToList();
                reccomendedbooks.Add(sortedauthorbooks[0]);
            }

            //2 & 3 High rated books of same genre (different authors) (4 or above ratings)
            List<Book> genrebooks = _context.Books.Include(b => b.Genre).Where(b => b.Genre == book.Genre).ToList();

            //add books of same genre with 4 or above ratings
            List<Book> genreratings = new List<Book>();
            foreach (Book b in genrebooks)
            {
                if (b.decAverageRating >= 4)
                {
                    reccomendedbooks.Add(b);
                    genreratings.Add(b);
                }
            }

            //other books in same genre
            foreach (Book b in genrebooks)
            {
                if (b.decAverageRating < 4)
                {
                    reccomendedbooks.Add(b);
                }
            }

            //if no books in genre, fill with highest rated book in general
            List<Book> bookssorted = eligiblebooks.OrderBy(o => o.decAverageRating).ToList();

            //fill remaining spots
            Book highestrated = bookssorted[0];
            reccomendedbooks.Add(highestrated);

            Book highestrated2 = bookssorted[1];
            reccomendedbooks.Add(highestrated2);

            Book highestrated3 = bookssorted[2];
            reccomendedbooks.Add(highestrated3);

            List<Book> eligiblereccomendations = new List<Book>();
            foreach (Book b in reccomendedbooks.ToList())
            {
                if (eligiblebooks.Contains(b))
                {
                    eligiblereccomendations.Add(b);
                }
            }

            List<Book> reccomendations = new List<Book>();

            if (eligiblereccomendations.Count == 1)
            {
                reccomendations.Add(eligiblereccomendations[0]);
            }

            if (eligiblereccomendations.Count == 2)
            {
                reccomendations.Add(eligiblereccomendations[0]);
                reccomendations.Add(eligiblereccomendations[1]);
            }

            if (eligiblereccomendations.Count >= 3)
            {
                reccomendations.Add(eligiblereccomendations[0]);
                reccomendations.Add(eligiblereccomendations[1]);
                reccomendations.Add(eligiblereccomendations[2]);
            }

            return reccomendations;
        }
    }
}
