//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace NicePictureStudio.App_Data
{
    using System;
    using System.Collections.Generic;
    
    public partial class Booking
    {
        public Booking()
        {
            this.BookingSpecialRequests = new HashSet<BookingSpecialRequest>();
        }
    
        public int Id { get; set; }
        public string Name { get; set; }
        public string BookingCode { get; set; }
        public System.DateTime AppointmentDate { get; set; }
        public string Details { get; set; }
        public string Title { get; set; }
        public string Surname { get; set; }
    
        public virtual BookingStatu BookingStatu { get; set; }
        public virtual Promotion Promotion { get; set; }
        public virtual Service Service { get; set; }
        public virtual ICollection<BookingSpecialRequest> BookingSpecialRequests { get; set; }
    }
}
