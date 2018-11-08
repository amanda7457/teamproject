using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Shalev_Elah_HW6.Models
{
    public class Supplier
    {
        //Primary key for database
        [Display(Name = "Supplier ID")]
        public Int32 SupplierID { get; set; }

        //All other required properties for supplier table
        [Required(ErrorMessage = "Supplier Name is required.")]
        [Display(Name = "Supplier Name")]
        public String SupplierName { get; set; }

        [Required(ErrorMessage = "Supplier Email is required.")]
        [Display(Name = "Supplier Email Address")]
        [EmailAddress(ErrorMessage = "Please enter a valid Supplier Email")]
        public String SupplierEmail { get; set; }

        [Required(ErrorMessage = "Supplier Phone Number is required.")]
        [DataType(DataType.PhoneNumber)]
        [Display(Name = "Supplier Phone Number")]
        [DisplayFormat(DataFormatString = "{0:###-###-####}", ApplyFormatInEditMode = true)]
        public String SupplierPhone { get; set; }

        [Required(ErrorMessage = "Established Date is required.")]
        [Display(Name = "Supplier Established")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}", ApplyFormatInEditMode = true)]
        public String SupplierEstablished { get; set; }

        [Required(ErrorMessage = "Preffered Supplier status is required.")]
        [Display(Name = "Is this a preffered supplier?")]
        public Boolean PreferredSupplier { get; set; }

        //Optional property
        [Display(Name = "Notes:")]
        public String SupplierNotes { get; set; }

        //navigation
        public List<SupplierDetail> SupplierDetails { get; set; }

    }
}