using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NicePictureStudio.Models
{
    public class BookingViewsModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string BookingCode { get; set; }
        public System.DateTime AppointmentDate { get; set; }
        public string SpecialOrder { get; set; }
        public string Details { get; set; }

        public string BookingStatus  { get; set; }
        public string PromotionName { get; set; }
        public string ServiceName  { get; set; }
    }
}