using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System;
using Group14_BevoBooks.Models;

namespace Group14_BevoBooks.Models
{
   
    public class AllBooks
    {
        [Display(Name = "Book")]
        public Book Book { get; set; }

        [Display(Name = "Quantity Sold")]
        public Int32 QuantitySold { get; set; }

        [Display(Name = "Order Detail")]
        public OrderDetail OrderDetail { get; set; }

        [Display(Name = "Customer")]
        public AppUser User { get; set; }

        //set value in controller
        [Display(Name = "Weighted Average Cost")]
        [DisplayFormat(DataFormatString = "{0:C}")]
        public Decimal AverageCost { get; set; }

        [Display(Name = "Profit Margin")]
        [DisplayFormat(DataFormatString = "{0:C}")]
        public Decimal ProfitMargin
        {
            get { return OrderDetail.Price - AverageCost; }
        }
    }

    public class AllOrders
    {
        [Required(ErrorMessage = "First name is required.")]
        [Display(Name = "First Name")]
        public String FirstName { get; set; }

        //Additional fields go here
        [Required(ErrorMessage = "Last name is required.")]
        [Display(Name = "Last Name")]
        public String LastName { get; set; }

        //NOTE: Here is the property for email
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        //NOTE: Here is the property for phone number
        [Required(ErrorMessage = "Phone number is required")]
        [Phone]
        [Display(Name = "Phone Number")]
        public string PhoneNumber { get; set; }

        //NOTE: Here is the property for Address
        [Required(ErrorMessage = "Address is required")]
        [Display(Name = "Address")]
        public string Address { get; set; }

        //NOTE: Here is the property for city
        [Required(ErrorMessage = "City is required")]
        [Display(Name = "City")]
        public string City { get; set; }

        [Required(ErrorMessage = "State is required")]
        [Display(Name = "State")]
        public string State { get; set; }

        [Required(ErrorMessage = "Zipcode is required")]
        [Display(Name = "Zipcode")]
        public string Zipcode { get; set; }




        //NOTE: Here is the logic for putting in a password
        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }
        //NOTE: do we need this for sign in 
        //[DataType(DataType.Password)]
       // [Display(Name = "Confirm password")]
       // [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
       // public string ConfirmPassword { get; set; }
    }
    public class AllCustomers
    {
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Current password")]
        public string OldPassword { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "New password")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm new password")]
        [Compare("NewPassword", ErrorMessage = "The new password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
    }   

    public class Totals
    {
        public bool HasPassword { get; set; }
        public String UserName { get; set; }
        public String Email { get; set; }
        public String UserID { get; set; }
    }

    public class CurrentInventory
    {
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Current password")]
        public string OldPassword { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "New password")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm new password")]
        [Compare("NewPassword", ErrorMessage = "The new password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
    }

    public class Reviews
    {
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Current password")]
        public string OldPassword { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "New password")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm new password")]
        [Compare("NewPassword", ErrorMessage = "The new password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
    }

}