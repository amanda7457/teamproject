using System;
using System.Collections.Generic;

namespace Group14_BevoBooks.Models
{
    public class Cart
    {
        public Int32 CartID { get; set; }

        public AppUser CartOwner { get; set; }

        public List<Book> BooksInCart { get; set; }

        public OrderDetail OrderDetail { get; set; }

        public Cart()
        {
            if (BooksInCart == null)
            {
                BooksInCart = new List<Book>();
            }
        }
    }
}
