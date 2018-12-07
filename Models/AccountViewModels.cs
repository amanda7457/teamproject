using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System;

namespace Group14_BevoBooks.Models
{
   
    public class LoginViewModel
    {
        [Required]
        [Display(Name = "Email")]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Display(Name = "Remember me?")]
        public bool RememberMe { get; set; }
    }

    public class RegisterViewModel
    {

        //TODO:  Add any fields that you need for creating a new user

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
    public class ChangePasswordViewModel
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

    public class ChangePhoneNumber
    {
        [Required(ErrorMessage = "Phone number is required")]
        [RegularExpression("^[0-9]{10}$", ErrorMessage = "Phone number bust be 10 digits long")]
        [DataType(DataType.PhoneNumber)]
        [Display(Name = "Phone Number")]
        public String NewPhoneNumber { get; set; }

    }

    public class ChangeFirstName
    {
        [Required]
        [Display(Name = "New Name")]
        public string NewFirstName { get; set; }

    }

    public class ChangeLastName
    {
        [Required]
        [Display(Name = "New Last Name")]
        public string NewLastName { get; set; }

    }

    public class ChangeAddress
    {
        [Required]
        [Display(Name = "New Address")]
        public string NewAddress { get; set; }

    }

    public class ChangeEmail
    {
        [Required]
        [EmailAddress]
        [Display(Name = "New Email")]
        public string NewEmail { get; set; }

    }
    public class IndexViewModel
    {
        public bool HasPassword { get; set; }
        public String UserName { get; set; }
        public String Email { get; set; }
        public String UserID { get; set; }
        public string NewPhoneNumber { get; set; }
        public String NewLastName { get; set; }
        public String NewFirstName { get; set; }
        public String NewEmail { get; set; }
        public String NewAddress { get; set; }
    }
}