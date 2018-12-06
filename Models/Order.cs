using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Group14_BevoBooks.Models
{
    public class Order
    {
        public Int32 OrderID { get; set; }

        [Display(Name = "Order Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime OrderDate { get; set; }
        
        [Display(Name = "Order Subotal")]
        [DisplayFormat(DataFormatString = "{0:C}")]
        public Decimal OrderSubtotal
        {
			get { return OrderDetails.Sum(od => od.ExtendedPrice); }
        }

        [Display(Name = "Shipping Price")]
        [DisplayFormat(DataFormatString = "{0:C}")]
        public Decimal OrderShipping { get; set; }

        [Display(Name = "Order Total")]
        [DisplayFormat(DataFormatString = "{0:C}")]
        public Decimal OrderTotal
        {
            get { return OrderShipping + OrderDetails.Sum(od => od.ExtendedPrice); }
        }

        [Display(Name = "Order Placed?")]
        public Boolean OrderPlaced { get; set; }

        [Display(Name = "Order Profit Margin")]
        [DisplayFormat(DataFormatString = "{0:C}")]
        public Decimal OrderProfitMargin
        {
            get { return OrderDetails.Sum(od => od.ProfitMargin); }
        }

        //navigation
        public AppUser AppUser { get; set; }
        public List<OrderDetail> OrderDetails { get; set; }
        public Discount Discount { get; set; }
        public CreditCard CreditCard { get; set; }

        public Order()
        {
            if (OrderDetails == null)
            {
                OrderDetails = new List<OrderDetail>();
            }
        }
    }
}
