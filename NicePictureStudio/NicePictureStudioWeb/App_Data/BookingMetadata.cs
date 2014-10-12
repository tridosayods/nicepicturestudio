using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace NicePictureStudio.App_Data
{
    [MetadataType(typeof(BookingMetadata))]
    public partial class Booking
    { }
    
    public class BookingMetadata
    {
            [Required]
            [StringLength(11,ErrorMessage="Booking Code is not over 11 digits")]
            public object BookingCode { get; set; }
    }

}