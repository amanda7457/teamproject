using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Group14_BevoBooks.Models
{
    public class DefaultReorder
    {
        public Int32 DefaultReorderID { get; set; }

        [Display(Name = "Default Reorder Quantity")]
        [Range(0, Double.MaxValue, ErrorMessage = "The value must be greater than 0")]
        public Int32 DefaultQuantity  { get; set; }

        //navigation

        //manager who set quantity
        public AppUser ManagerSet { get; set; }

        //orders that use that quantity price
        public List<Order> Orders { get; set; }

    }
}
