using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Group14_BevoBooks.Models
{
    public class CreditCard
    {
        public Int32 CreditCardID { get; set; }

        [Required]
        [RegularExpression("^[0-9]{16}$", ErrorMessage = "Card number must be 16 digits")]
        public string CardNumber { get; set; }

        //navigation
        public AppUser AppUser { get; set; }

        public List<Order> Orders { get; set; }

        public CreditCard()
        {
            if (Orders == null)
            {
                Orders = new List<Order>();
            }
        }
    }
}
