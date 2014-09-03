using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using NicePictureStudio.App_Data;

namespace NicePictureStudio.Models
{
    public class ServicesViewModel
    {
        public int Id { get; set; }
        public string BookingName { get; set; }
        public string GroomName { get; set; }
        public string BrideName { get; set; }
        public string SpecialRequest { get; set; }
        public Nullable<decimal> Payment { get; set; }
        public Nullable<decimal> PayAmount { get; set; }
        public int CustomerId { get; set; }
        public Nullable<int> CRMFormId { get; set; }
    }

    public class PromotionViewModel
    {
        private DateTime _expireDate;
        private int _photoGraphDiscount;
        private int _equipmentDiscount;
        private int _locationDiscount;
        private int _outputDiscount;
        private int _outsourceDiscount;

        public PromotionViewModel(DateTime expDate,int photoGraphDiscount, int equipmentDiscount, int locationDiscount,
            int outputDiscount, int outsourceDiscount)
        {
            _expireDate = expDate;
            _photoGraphDiscount = photoGraphDiscount;
            _equipmentDiscount = equipmentDiscount;
            _locationDiscount = locationDiscount;
            _outputDiscount = outputDiscount;
            _outsourceDiscount = outsourceDiscount;
        }
    }

    public class ServiceFormViewModel
    { 
        public int Id { get; set; }
        public string Name { get; set; }
        public int ServiceType { get; set; }
        public int Status { get; set; }
        public System.DateTime EventStart { get; set; }
        public System.DateTime EventEnd { get; set; }
        public Nullable<int> GuestsNumber { get; set; }
    }

    public class ServiceFromKeeper
    {
        public int CustomerId { get; set; }
        public List<Service> ServiceCollection { get; set; }
    }
}