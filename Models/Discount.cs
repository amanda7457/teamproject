using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Group14_BevoBooks.Models
{
    //what are the types?
    public enum DiscountType { FreeShipping, PercentOff };

    public class Discount
    {
        public Int32 DiscountID { get; set; }

        [StringLength(20, MinimumLength = 1, ErrorMessage = "Discount code must be between 1 and 20 characters")]
        [Display(Name = "Discount Code")]
        public string PromoCode { get; set; }

        [Display(Name = "Discount Amount")]
        [Range(0, Double.MaxValue, ErrorMessage = "The value must be greater than 0")]
        public Decimal DiscountAmount { get; set; }

        [Display(Name = "Start Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime DiscountStartDate { get; set; }

        [Display(Name = "End Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime DiscountEndDate { get; set; }

        public DiscountType DiscountType { get; set; }

        [Display(Name = "Discount Code Still Active")]
        public Boolean Active { get; set; }

        //navigation
        public List<Order> Orders { get; set; }

        public Discount()
        {
            if (Orders == null)
            {
                Orders = new List<Order>();
            }

        }
    }
}
