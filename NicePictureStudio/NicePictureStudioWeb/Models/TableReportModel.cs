using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using NicePictureStudio.App_Data;
using System.Globalization;

namespace NicePictureStudio.Models
{
    public class TableReportModel
    {
        public List<EmployeeDetails> listEmployee { get; set; }
        public string MainPhotoGraph { get; set; }
        public string Position { get; set; }

        //Equipment - Output Section
        public DateTime RequiredDate { get; set; }

        //Customer Information
        public string Bride { get; set; }
        public string Groom { get; set; }
        public string SpecialRequest { get; set; }
        public string Suggestion { get; set; }
        public string Remark { get; set; }

        //Location
        public string Location { get; set; }
        public string LocationDetails { get; set; }
        public string Map { get; set; }

        //booking
        public string BookingCode { get; set; }
        public string BookingRequest { get; set; }

    }

    public class EmployeeDetails
    {
        public string Name { get; set; }
        public string Position { get; set; }
    }
}