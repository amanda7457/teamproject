using System;
using System.Collections.Generic;

namespace Group14_BevoBooks.Models
{
    public class OrderHistory
    {
        public Int32 OrderHistoryID { get; set; }

        public AppUser OrderOwner { get; set; }

        public List<Book> PastOrders { get; set; }

        public OrderDetail OrderDetail { get; set; }

        public CreditCard CreditCard { get; set; }

        //need order date

        public OrderHistory()
        {
            if (PastOrders == null)
            {
                PastOrders = new List<Book>();
            }
        }
    }
}
