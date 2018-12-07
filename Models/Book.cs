using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Group14_BevoBooks.Models
{
    public class Book
    {
        public Int32 BookID { get; set; }

        [Required(ErrorMessage = "Unique ID is required.")]
        [Display(Name = "Unique ID")]
        public Int32 UniqueID { get; set; }

        [Required(ErrorMessage = "Title is required.")]
        [Display(Name = "Book Title")]
        public String Title { get; set; }

        [Required(ErrorMessage = "Selling price is required.")]
        [Display(Name = "Book Price")]
        [DisplayFormat(DataFormatString = "{0:C}")]
        public Decimal SellingPrice { get; set; }

        [Required(ErrorMessage = "Book cost is required.")]
        [Display(Name = "Supplier Price")]
        [DisplayFormat(DataFormatString = "{0:C}")]
        public Decimal SupplierPrice { get; set; }

        [Display(Name = "Description")]
        public String Description { get; set; }

        [Required(ErrorMessage = "Author is required.")]
        [Display(Name = "Author")]
        public String Author { get; set; }

        [Range(0, Double.MaxValue, ErrorMessage = "The value must be greater than 0")]
        public Int32 Reorder { get; set; }

        public Boolean Active { get; set; }

        [Range(0, Double.MaxValue, ErrorMessage = "The value must be greater than 0")]
        public Int32 Inventory { get; set; }

        [Display(Name = "Date Published")]
        [DataType(DataType.Date)]
        public DateTime PublishedDate { get; set; }

        public Boolean Discontinued { get; set; }

        [Display(Name = "Average Rating")]
        public Decimal decAverageRating
        {
            get 
            {
                Decimal averagerating;
                if (Reviews.Count ==0)
                {
                    averagerating = 0;
                }

                else
                {
                    Decimal reviewsum = Reviews.Sum(rd => rd.Rating);
                    int reviewCount = Reviews.Count();

                    averagerating = reviewsum / reviewCount;
                }

                return averagerating;
            }
        }

        public Boolean InStock { get; set; }

        [Display(Name = "Popularity")]
        public Int32 intPopularity 
        {
            get { return OrderDetails.Sum(od => od.Quantity); }
        }

        //get properties for reporting
        [Display(Name = "Weighted Average Cost")]
        [DisplayFormat(DataFormatString = "{0:C}")]
        public Decimal AverageCost
        {
            get
            {
                int booksboughtcount = BookOrders.Sum(bo => bo.Quantity);

                Decimal averagecost;
                if (booksboughtcount >= 1)
                {
                    Decimal costsum = BookOrders.Sum(bo => bo.Price);

                    averagecost = costsum / booksboughtcount;
                }

                else
                {
                    averagecost = SupplierPrice;
                }

                return averagecost;
            }
        }

        [Display(Name = "Weighted Average Price")]
        [DisplayFormat(DataFormatString = "{0:C}")]
        public Decimal AveragePrice
        {
            get
            {
                List<OrderDetail> odplaced = new List<OrderDetail>();
                //list of ods of orders that have been placed already
                foreach (OrderDetail od in OrderDetails)
                {
                    if (od.Order.OrderPlaced == true)
                    {
                        odplaced.Add(od);
                    }
                }

                int bookboughtcount = odplaced.Count();

                Decimal averageprice;
                if (odplaced == null)
                {
                    averageprice = SellingPrice;
                }

                else
                {
                    Decimal pricesum = OrderDetails.Sum(o => o.Price);

                    averageprice = pricesum / bookboughtcount;
                }

                return averageprice;
            }
        }

        //navigation

        public List<OrderDetail> OrderDetails { get; set; }
        public List<Review> Reviews { get; set; }

        public Genre Genre { get; set; }

        public List<BookOrder> BookOrders { get; set; }
        public int UnqiueID { get; internal set; }

        public Book()
        {
            if (Reviews == null)
            {
                Reviews = new List<Review>();
            }

            if (BookOrders == null)
            {
                BookOrders = new List<BookOrder>();
            }

            if (OrderDetails == null)
            {
                OrderDetails = new List<OrderDetail>();
            }
        }
    }
}
