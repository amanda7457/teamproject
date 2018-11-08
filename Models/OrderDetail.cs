using System;
using System.ComponentModel.DataAnnotations;

namespace Shalev_Elah_HW6.Models
{
    public class OrderDetail
    {
        public Int32 OrderDetailID { get; set; }

        [Display(Name = "Quantity")]
        [Range(1, 1000, ErrorMessage = "Quantity must be between 1 and 1000")]
        public Int32 Quantity { get; set; }

        //price at time of order
        public Decimal ProductPrice { get; set}

        //quantity * product price at the time of the order
        //TODO make this a calculated property??
        [Display(Name = "Order Total")]
        [DisplayFormat(DataFormatString = "{0:C}")]
        public Decimal ExtendedPrice { get; set; }

        //navigational properties
        public Order Order { get; set; }
        public Product Product { get; set; }
    }
}
