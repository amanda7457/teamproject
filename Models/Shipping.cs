using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Group14_BevoBooks.Models
{
    public class Shipping
    {
        public Int32 ShippingID { get; set; }

        [Display(Name = "Shipping Price - First Book")]
        [DisplayFormat(DataFormatString = "{0:C}")]
        public Decimal ShippingFirst  { get; set; }

        [Display(Name = "Shipping Price - Each Additional Book")]
        [DisplayFormat(DataFormatString = "{0:C}")]
        public Decimal ShippingAdditional { get; set; }

        //navigation

        //manager who set shipping price
        public AppUser ManagerSet { get; set; }

        //orders that use that shipping price
        public List<Order> Orders { get; set; }

    }
}
