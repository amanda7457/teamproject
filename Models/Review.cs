using System;
using System.ComponentModel.DataAnnotations;

namespace Group14_BevoBooks.Models
{
    public class Review
    {
        public Int32 ReviewID { get; set; }

        [Display(Name = "Rating")]
        [Range(1,5, ErrorMessage ="Rating must be between 1-5")]
        public Int32 Rating { get; set; }

        [Display(Name = "Review")]
        [StringLength(100, MinimumLength = 1, ErrorMessage = "Reviews must be between 1 and 100 characters")]
        public String ReviewText { get; set; }

        public Boolean Approved { get; set; }

        //navigation
        public AppUser Author { get; set; }

        public AppUser Approver { get; set; }

        public Book Book { get; set; }
    }
}
