using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace Group14_BevoBooks.Models
{
    public class AppUser : IdentityUser
    {
        //Identity creates several of the important ones for you
        //Full documentation of the IdentityUser class can be found at
        //https://docs.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.identity.entityframeworkcore.identityuser?view=aspnetcore-1.1&viewFallbackFrom=aspnetcore-2.1

        //Here is an example of first name
        [Required(ErrorMessage = "First name is required.")]
        [Display(Name = "First Name")]
        public String FirstName { get; set; }

        [Required(ErrorMessage = "Last name is required.")]
        [Display(Name = "Last Name")]
        public String LastName { get; set; }

        [Required(ErrorMessage = "Address is required.")]
        [Display(Name = "Address")]
        public String Address { get; set; }

        [Required(ErrorMessage = "City is required.")]
        [Display(Name = "City")]
        public String City { get; set; }

        [Required(ErrorMessage = "State is required.")]
        [Display(Name = "State")]
        public String State { get; set; }

        [Required(ErrorMessage = "Zipcode is required.")]
        [Display(Name = "Zipcode")]
        public String Zipcode { get; set; }

        [Display(Name = "Active User")]
        public Boolean ActiveUser { get; set; }

        //navigation
        public List<Order> Orders { get; set; }
        public List<CreditCard> CreditCards { get; set; }

		[InverseProperty("Author")]
        public List<Review> ReviewsWritten { get; set; }

		[InverseProperty("Approver")]
        public List<Review> ReviewsApproved { get; set; }

        public AppUser()
        {
            if (Orders == null)
            {
                Orders = new List<Order>();
            }

            if (CreditCards == null)
            {
                CreditCards = new List<CreditCard>();
            }

            if (ReviewsWritten == null)
            {
               ReviewsWritten = new List<Review>();
            }

            if (ReviewsApproved == null)
            {
                ReviewsApproved = new List<Review>();
            }
        }
    }
}
