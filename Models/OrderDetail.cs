using System;
using System.ComponentModel.DataAnnotations;

namespace Group14_BevoBooks.Models
{
    public class OrderDetail
    {
        public Int32 OrderDetailID { get; set; }

        [Display(Name = "Quantity")]
        [Range(0, Double.MaxValue, ErrorMessage = "The value must be greater than 0")]
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

        [Display(Name = "Profit Margin")]
        [DisplayFormat(DataFormatString = "{0:C}")]
        public Decimal ProfitMargin { get; set; }

        [Display(Name = "Cost")]
        [DisplayFormat(DataFormatString = "{0:C}")]
        public Decimal Cost { get; set; }

        //navigation
        public Order Order { get; set; }
        public Book Book { get; set; }
    }
}
