using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Group14_BevoBooks.Models;


namespace Group14_BevoBooks.Models
{
    //not sure if this is the best approach or not
    public enum Status { Pending, Ordered, Delivered };

    public class BookOrder
    {
        public Int32 BookOrderID { get; set; }

        [Display(Name = "Supplier Order Status")]
        public Status Status { get; set; }

        [Display(Name = "Quantity")]
        public Int32 Quantity { get; set; }

        [Display(Name = "Price")]
        [DisplayFormat(DataFormatString = "{0:C}")]
        public Decimal Price { get; set; }

        [Display(Name = "Total")]
        [DisplayFormat(DataFormatString = "{0:C}")]
        public Decimal ExtendedPrice
        {
            get { return Price * Quantity; }
        }

        public Boolean InReorderList { get; set; }

        //navigation
        public AppUser AppUser { get; set; }
        public DefaultReorder ReorderQuantity { get; set; }
        public Book Book { get; set; }

        //public List<BookOrderDetail> BookOrderDetails { get; set; }

    }
}
