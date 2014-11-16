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
        public int? ServiceId { get; set; }
        public int? OutsourceId { get; set; }
        public List<EmployeeDetails> listEmployee { get; set; }
        public string MainPhotoGraph { get; set; }
        public string Position { get; set; }
        public string PhotoGraphPhoneNumber { get; set; }
        public string ServiceType { get; set; }
        public string GuestNumber { get; set; }

        //Equipment - Output Section
        public DateTime RequiredDate { get; set; }

        //Customer Information
        public string Bride { get; set; }
        public string Groom { get; set; }
        public string SpecialRequest { get; set; }
        public string Suggestion { get; set; }
        public string Remark { get; set; }
        public string GroomPhone { get; set; }
        public string BridePhone { get; set; }
        public string GroomMail { get; set; }
        public string BrideMail { get; set; }
        public string Address { get; set; }

        //Location
        public string Location { get; set; }
        public string LocationDetails { get; set; }
        public string Map { get; set; }
        public string LocatioNumber { get; set; }
        public DateTime EventStart { get; set; }
        public DateTime EventEnd { get; set; }

        //booking
        public string BookingCode { get; set; }
        public string BookingRequest { get; set; }

    }

    public class EmployeeDetails
    {
        public string Name { get; set; }
        public string Position { get; set; }
        public string NickName { get; set; }
        public string PhoneNumber { get; set; }
        public string Specialibity { get; set; }
        public string Email { get; set; }
    }

    public class OutsourceInformation
    {
        public int? Id { get; set; }
        public string Name { get; set; }
        public string OutsourceTypeName { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }
        public int NumericNumber { get; set; }
        public TimeSpan OpenTime { get; set; }
        public TimeSpan CloseTime { get; set; }
        public string Detail { get; set; }
    }

    public class OutputInformation
    {
        public int? Id { get; set; }
        public string OutputName { get; set; }
        public string OutputURL { get; set; }
        public string Description { get; set; }
        public string OutputType { get; set; }
        public string OutputSize { get; set; }
        public int Quantity { get; set; }
    }

   public class PhotographInfo
   {
        public int Numphotographer {get;set;}
        public int NumCameraman {get;set;}
        public int? GuestsNumber {get;set;}
        public string Description { get; set; }
   }

   public class ServiceFormWithAllRelatedServicesInfo
   {
       public string Name { get; set; }
       public string ServiceTypeName { get; set; }
       public DateTime EventStart { get; set; }
       public DateTime EventEnd { get; set; }
       public string Status { get; set; }

       public string Details { get; set; }
       public string ExtraInfo1 { get; set; }
       public string ExtraInfo2 { get; set; }
       public int scheduleId { get; set; }
   }

}