using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace NicePictureStudio.App_Data
{
    [MetadataType(typeof(BookingMetadata))]
    public partial class Booking
    { }
    
    public class BookingMetadata
    {
            [Required]
            [StringLength(5,ErrorMessage="Booking Code is not over 5 digits")]
            public object BookingCode { get; set; }
    }
}