using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Group14_BevoBooks.DAL;
using Group14_BevoBooks.Models;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.c/fwlink/?LinkID=397860

namespace Group14_BevoBooks.Controllers
{

    public enum TitleOrder { Descending, Ascending }
    public enum AuthorOrder { Descending, Ascending }
    public enum PopularityOrder { Descending, Ascending }
    public enum ReleaseOrder { Descending, Ascending }
    public enum RatingOrder { Descending, Ascending }

    public enum SortOrder { TitleAscending, TitleDescending, AuthorAscending, AuthorDescending, PopularityAscending, PopularityDescending, ReleaseAscending, ReleaseDescending, RatingAscending, RatingDescending };

    //Descending is newest to oldest, Ascending is oldest to newest

    public class DetailedSearchController : Controller
    {

        private AppDbContext _db;
        public DetailedSearchController(AppDbContext context)
        {
            _db = context;
        }


        public IActionResult Details(int? id)
        {

            if (id == null) //Repo id not specified
            {
                return View("Error", new String[] { "Book ID not specified - which repo do you want to view?" });
            }

            Book repo = _db.Books.Include(r => r.Genre).FirstOrDefault(r => r.BookID == id);

            if (repo == null) //Repo does not exist in database
            {
                return View("Error", new String[] { "Book not found in database" });
            }

            //if code gets this far, all is well
            return View(repo);

        }

        // GET: /<controller>/
        public ActionResult Index()
        {
            ViewBag.AllGenres = GetAllGenres();
            return View();
        }

        //queries the results -- has to match the detailed search page
        public ActionResult SearchResult(String Title, String Author, String BookID, int SelectedGenre, SortOrder SortSelected )
        {
            SetInStock();

            //our viewbag for all of our books :)
            ViewBag.TotalBooks = _db.Books.Count();
            //List<Book> SelectedBooks = new List<Book>();

            //this gets all the results from the database
            List<Book> BooksToDisplay = new List<Book>();
            var query = from c in _db.Books
                        select c;

            // QUERYING NOW yay 

            if (Title != null && Title != "")
            {
                query = query.Where(c => c.Title.Contains(Title));
            }

            if (Author != null && Author != "")
            {
                query = query.Where(c => c.Author.Contains(Author));
            }

            if (BookID != null && BookID != "")
            {
                //make sure string is a valid number
                Int32 intBookID;
                try
                {
                    intBookID = Convert.ToInt32(BookID);
                }
                catch
                {
                    //re-populate the viewbag
                    ViewBag.AllGenres = GetAllGenres();

                    //send user back to detailed search
                    return View();
                }
                query = query.Where(c => c.UniqueID == intBookID);

            }

            if (SelectedGenre == 0)
            {
                query = query.OrderByDescending(c => c.Title);
            }
            else
            {
                Genre GenreToDisplay = _db.Genres.Find(SelectedGenre);
                query = query.Where(c => c.Genre == GenreToDisplay);
            }

            List<Book> SelectedBooks = query.ToList();
            SelectedBooks = query.Include(r => r.Genre).ToList();
            ViewBag.SelectedBooks = SelectedBooks.Count();
            ViewBag.TotalBooks = _db.Books.Count();

            switch (SortSelected)
            {
                case SortOrder.TitleDescending:
                    SelectedBooks = SelectedBooks.OrderByDescending(c => c.Title).ToList();
                    break;

                case SortOrder.TitleAscending:
                    SelectedBooks = SelectedBooks.OrderBy(c => c.Title).ToList();
                    break;

                case SortOrder.AuthorDescending:
                    SelectedBooks = SelectedBooks.OrderBy(c => c.Author).ToList();
                    break;

                case SortOrder.AuthorAscending:
                    SelectedBooks = SelectedBooks.OrderByDescending(c => c.Author).ToList();
                    break;

                case SortOrder.PopularityDescending:
                    SelectedBooks = SelectedBooks.OrderByDescending(c => c.intPopularity).ToList();
                    break;

                case SortOrder.PopularityAscending:
                    SelectedBooks = SelectedBooks.OrderBy(c => c.intPopularity).ToList();
                    break;

                case SortOrder.ReleaseDescending:
                    SelectedBooks = SelectedBooks.OrderByDescending(c => c.PublishedDate).ToList();
                    break;

                case SortOrder.ReleaseAscending:
                    SelectedBooks = SelectedBooks.OrderBy(c => c.PublishedDate).ToList();
                    break;

                case SortOrder.RatingDescending:
                    SelectedBooks = SelectedBooks.OrderByDescending(c => c.decAverageRating).ToList();
                    break;

                case SortOrder.RatingAscending:
                    SelectedBooks = SelectedBooks.OrderBy(c => c.decAverageRating).ToList();
                    break;

            }


            return View(SelectedBooks);
        
        }


        //THIS IS YO VIEWBAG
        public SelectList GetAllGenres()
        {
            List<Genre> Genres = _db.Genres.ToList();

            //add a record for all genres
            Genre SelectNone = new Genre() { GenreID = 0, GenreName = "All Genres" };
            Genres.Add(SelectNone);

            //convert list to select list
            SelectList AllGenres = new SelectList(Genres.OrderBy(m => m.GenreID), "GenreID", "GenreName");

            //return the select list
            return AllGenres;

        }

        public void SetInStock()
        {
            List<Book> allbooklist = _db.Books.Include(b => b.Genre).ToList();

            foreach (Book b in allbooklist)
            {
                if (b.Inventory >= 1)
                {
                    b.InStock = true;
                }
                else
                {
                    b.InStock = false;
                }

                _db.Update(b);
            }
            _db.SaveChanges();
        }

    }

}