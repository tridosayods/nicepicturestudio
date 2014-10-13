using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace NicePictureStudio.App_Data
{
    [MetadataType(typeof(CustomerMetadata))]
    public partial class Customer
    { }

    public class CustomerMetadata
    {
        //[Required]
        //[StringLength(11, ErrorMessage = "Booking Code is not over 11 digits")]
        //public object BookingCode { get; set; }

        [Required]
        public string CustomerName { get; set; }
         [Required]
         [Phone]
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
         [Required]
         [EmailAddress]
        public string Email { get; set; }
        public string PostcalCode { get; set; }
        [Required]
        public Nullable<System.DateTime> AnniversaryDate { get; set; }
        public string ReferencePerson { get; set; }
        [EmailAddress]
        public string ReferenceEmail { get; set; }
        [Phone]
        public string ReferencePhoneNumber { get; set; }
        public string CustomerTitle { get; set; }
        public string CustomerSurname { get; set; }
        public string CoupleTitle { get; set; }
        public string CoupleName { get; set; }
        public string CoupleSurname { get; set; }
        public string CouplePhoneNumber { get; set; }
        public string BuildingBlock { get; set; }
        public string Road { get; set; }
        public string Subdistrict { get; set; }
        public string District { get; set; }
        public string Province { get; set; }
        public string Country { get; set; }
        public string CoupleEmail { get; set; }
    }

}