using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Group14_BevoBooks.Models
{
    public class Genre
    {
        public Int32 GenreID { get; set; }

        [Display(Name = "Genre Name")]
        public String GenreName { get; set; }

        //navigation
        public List<Book> Books { get; set; }

        public Genre()
        {
            if (Books == null)
            {
                Books = new List<Book>();
            }
        }
    }
}
