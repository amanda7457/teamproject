using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Shalev_Elah_HW6.Models
{
    public class Product
    {
        //Primary key for database
        [Display(Name = "Product ID")]
        public Int32 ProductID { get; set; }

        //number should start at 5001 - in controller
        public Int32 SKU { get; set; }

        [Display(Name = "Product Name")]
        public String ProductName { get; set; }

        [Display(Name = "Product Price")]
        [DisplayFormat(DataFormatString = "{0:C}")]
        public Decimal Price { get; set; }

        [Display(Name = "Product Description")]
        public String Description { get; set; }

        //navigation
        public List<OrderDetail> OrderDetails { get; set; }
    }
}
