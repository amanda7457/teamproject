using System;
using System.ComponentModel.DataAnnotations;

namespace Group14_BevoBooks.Models
{
    public class BookOrderDetail
    {
        public Int32 BookOrderDetailID { get; set; }

        [Display(Name = "Quantity")]
        public Int32 Quantity { get; set; }

        [Display(Name = "Price")]
        [DisplayFormat(DataFormatString = "{0:C}")]
        public Decimal Price { get; set; }

        [Display(Name = "Extended price")]
        [DisplayFormat(DataFormatString = "{0:C}")]
        public Decimal ExtendedPrice
        {
            get { return Price * Quantity; }
        }

        //navigation
        public BookOrder Order { get; set; }
        public Book Book { get; set; }
    }
}
