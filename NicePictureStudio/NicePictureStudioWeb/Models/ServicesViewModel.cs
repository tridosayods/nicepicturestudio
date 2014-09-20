using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using NicePictureStudio.App_Data;
using System.Globalization;

namespace NicePictureStudio.Models
{
    public class ServicesViewModel
    {
        private ServiceFormFactory _serviceFormPreWedding = new ServiceFormFactory();
        private ServiceFormFactory _serviceFormEngagement = new ServiceFormFactory();
        private ServiceFormFactory _serviceFormWedding = new ServiceFormFactory();

        public int Id { get; set; }
        public string BookingName { get; set; }
        public string GroomName { get; set; }
        public string BrideName { get; set; }
        public string SpecialRequest { get; set; }
        public Nullable<decimal> Payment { get; set; }
        public Nullable<decimal> PayAmount { get; set; }
        public int CustomerId { get; set; }
        public Nullable<int> CRMFormId { get; set; }

        public CustomerViewModel Customer { get; set; }
        public PromotionViewModel Promotion { get; set; }

        public ServiceFormFactory ServiceFormPreWedding
        {
            get { return _serviceFormPreWedding; }
            set { _serviceFormPreWedding = value; }
        }
        public ServiceFormFactory ServiceFormEngagement
        {
            get { return _serviceFormEngagement; }
            set { _serviceFormEngagement = value; }
        }
        public ServiceFormFactory ServiceFormWedding
        {
            get { return _serviceFormWedding; }
            set { _serviceFormWedding = value; }
        }

        public void CreateService(Service service)
        {
            Id = service.Id;
            BookingName = service.BookingName;
            GroomName = service.GroomName;
            BrideName = service.BrideName;
            SpecialRequest = service.SpecialRequest;
            Payment = service.Payment;
            PayAmount = service.PayAmount;
        }

        public void CreateCustomer(Customer customer)
        {
            Customer = new CustomerViewModel();
            Customer.CustomerId= customer.CustomerId;
            Customer.CustomerName = customer.CustomerName;
            Customer.Address = customer.Address;
            Customer.AnniversaryDate = customer.AnniversaryDate;
            Customer.City = customer.City;
            Customer.Email = customer.Email;
            Customer.PhoneNumber = customer.PhoneNumber;
            Customer.PostcalCode = customer.PostcalCode;
            Customer.ReferenceEmail = customer.ReferenceEmail;
            Customer.ReferencePerson = customer.ReferencePerson;
            Customer.ReferencePhoneNumber = customer.ReferencePhoneNumber;
        }

    }

    public class ServiceFormFactory
    {
        public ServiceFormViewModel ServiceForm { get; set; }
        public PhotoGraphServiceViewModel PhotoGraphService { get; set; }
        public List<EquipmentServiceViewModel> ListEquipmentServices = new List<EquipmentServiceViewModel>();
        public List<OutsourceServiceViewModel> ListOutsourceServices = new List<OutsourceServiceViewModel>();
        public List<OutputServiceViewModel> ListOutputServices = new List<OutputServiceViewModel>();
        public List<LocationServiceViewModel> ListLocationServices = new List<LocationServiceViewModel>();

        public void CreateServiceForm(ServiceForm serviceForm, int statusId, int typeId)
        {
            ServiceForm = new ServiceFormViewModel();
            ServiceForm.Id = serviceForm.Id;
            ServiceForm.Name = serviceForm.Name;
            ServiceForm.ServiceType = typeId;
            ServiceForm.Status = statusId;
            ServiceForm.EventStart = serviceForm.EventStart;
            ServiceForm.EventEnd = serviceForm.EventEnd;
        }

        public void CreatePhotoGraphService(PhotographService photoGraph, List<string> photoGraphList, List<string> cameraManList)
        {
            PhotoGraphService = new PhotoGraphServiceViewModel();
            PhotoGraphService.Id = photoGraph.Id;
            PhotoGraphService.Name = photoGraph.Name;
            PhotoGraphService.PhotographerNumber = photoGraph.PhotographerNumber;
            PhotoGraphService.Price = photoGraph.Price;
            PhotoGraphService.CameraManNumber = photoGraph.CameraManNumber;
            PhotoGraphService.Cost = photoGraph.Cost;
            PhotoGraphService.Description = photoGraph.Description;
            PhotoGraphService.PhotoGraphIdList = new List<string>(photoGraphList);
            PhotoGraphService.CameraMandIdList = new List<string>(cameraManList);
        }

        public void CreateEquipmentServiceList(EquipmentService equipment, int equipmentId)
        {
            EquipmentServiceViewModel _equipmentService = new EquipmentServiceViewModel();
            _equipmentService.Id = equipment.Id;
            _equipmentService.Name = equipment.Name;
            _equipmentService.Price = equipment.Price;
            _equipmentService.Cost = equipment.Cost;
            _equipmentService.Description = equipment.Description;
            _equipmentService.EquipmentId = equipmentId;
            ListEquipmentServices.Add(_equipmentService);
        }

        public void CreateLocationServiceList(LocationService location, int locationId)
        {
            LocationServiceViewModel _locationService = new LocationServiceViewModel();
            _locationService.Id = location.Id;
            _locationService.Name = location.Name;
            _locationService.IsOverNight = location.IsOverNight;
            _locationService.OverNightPeriod = location.OverNightPeriod;
            _locationService.Price = location.Price;
            _locationService.Cost = location.Cost;
            _locationService.LocationId = locationId;
            _locationService.Description = location.Description;
            ListLocationServices.Add(_locationService);
        }

        public void CreateOutSoruceServiceList(OutsourceService outsource, int oursourceId)
        {
            OutsourceServiceViewModel _outsourceService = new OutsourceServiceViewModel();
            _outsourceService.Id = outsource.Id;
            _outsourceService.Name = outsource.Name;
            _outsourceService.OutsourceId = oursourceId; // Only ID is required
            _outsourceService.PortFolioURL = outsource.PortFolioURL;
            _outsourceService.Price = outsource.Price;
            _outsourceService.Cost = outsource.Cost;
            _outsourceService.Description = outsource.Description;
            ListOutsourceServices.Add(_outsourceService);
        }

        public void CreateOutputServiceList(OutputService output)
        {
            OutputServiceViewModel _outputService = new OutputServiceViewModel();
            _outputService.Id = output.Id;
            _outputService.Name = output.Name;
            _outputService.OutputURL = output.OutputURL;
            _outputService.Price = output.Price;
            _outputService.Cost = output.Cost;
            _outputService.Description = output.Description;
            ListOutputServices.Add(_outputService);
        }
    }

    public class CustomerViewModel
    {
        public int CustomerId { get; set; }
        public string CustomerName { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string Email { get; set; }
        public string PostcalCode { get; set; }
        public Nullable<System.DateTime> AnniversaryDate { get; set; }
        public string ReferencePerson { get; set; }
        public string ReferenceEmail { get; set; }
        public string ReferencePhoneNumber { get; set; }
    }

    public class PromotionViewModel
    {
        private DateTime _expireDate;
        private DateTime _createDate;
        private string _name;
        private int _photoGraphDiscount;
        private int _equipmentDiscount;
        private int _locationDiscount;
        private int _outputDiscount;
        private int _outsourceDiscount;
        
        //public PromotionViewModel(DateTime expDate,int photoGraphDiscount, int equipmentDiscount, int locationDiscount,
        //    int outputDiscount, int outsourceDiscount)
        //{
        //    _expireDate = expDate;
        //    _photoGraphDiscount = photoGraphDiscount;
        //    _equipmentDiscount = equipmentDiscount;
        //    _locationDiscount = locationDiscount;
        //    _outputDiscount = outputDiscount;
        //    _outsourceDiscount = outsourceDiscount;
        //}

        public PromotionViewModel(Promotion promotion)
        {
            if (promotion !=null)
            {
                _expireDate = promotion.ExpireDate;
                _photoGraphDiscount = promotion.PhotoGraphDiscount;
                _equipmentDiscount = promotion.EquipmentDiscount;
                _locationDiscount = promotion.LocationDiscount;
                _outputDiscount = promotion.OutputDiscount;
                _outsourceDiscount = promotion.OutsourceDiscount;
                _name = promotion.Name;
                _createDate = promotion.CreateDate;
            }
           
        }

        public string PromotionName() {return _name;}
        public DateTime PromotionEndDate() { return _expireDate; }
        public DateTime PromotionStartDate() { return _createDate; }
        public int PhotoGraphDiscount 
        {
            get { return _photoGraphDiscount; }
            set { _photoGraphDiscount = value; }
        }
        public int EquipmentDiscount 
        {
            get { return _equipmentDiscount; }
            set { _equipmentDiscount = value; }
        }
        public int LocationDiscount 
        {
            get { return _locationDiscount; }
            set { _locationDiscount = value; }
        }
        public int OutsourceDiscount 
        {
            get { return _outsourceDiscount; }
            set { _outsourceDiscount = value; }
        }
        public int OutputDiscount 
        {
            get { return _outputDiscount; }
            set { _outputDiscount = value; }
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

    public class PhotoGraphServiceViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int PhotographerNumber { get; set; }
        public int CameraManNumber { get; set; }
        public string Description { get; set; }
        public Nullable<decimal> Cost { get; set; }
        public Nullable<decimal> Price { get; set; }
        public List<string> PhotoGraphIdList { get; set; }
        public List<string> CameraMandIdList { get; set; }
        public bool IsSelected { get; set; }

        public void generatePhotoGraphService(PhotographService _photoGraphService)
        { 
            
        }
    }

    public class PhotoGraph
    { 
        public string Id { get; set; }
        public string Name { get; set; }
        public bool IsSelect { get; set; }

        public PhotoGraph() {}

        public PhotoGraph(Employee photograph, List<string> selectedId)
        {
            Id = photograph.Id;
            Name = photograph.Name;
            IsSelect = selectedId.Contains(photograph.Id);
        }
    }

    public class CameraMan
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public bool IsSelect { get; set; }
        public CameraMan() { }
        public CameraMan(Employee cameraman, List<string> selectedId)
        {
            Id = cameraman.Id;
            Name = cameraman.Name;
            IsSelect = selectedId.Contains(cameraman.Id);
        }

    }

    public class EquipmentServiceViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public Nullable<decimal> Price { get; set; }
        public Nullable<decimal> Cost { get; set; }
        public string Description { get; set; }
        public int EquipmentId { get; set; }
    }

    public class LocationServiceViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public Nullable<decimal> Price { get; set; }
        public Nullable<decimal> Cost { get; set; }
        public bool IsOverNight { get; set; }
        public Nullable<int> OverNightPeriod { get; set; }
        public int LocationId { get; set; }
        public string Description { get; set; }
    }

    public class OutsourceServiceViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string PortFolioURL { get; set; }
        public Nullable<decimal> Price { get; set; }
        public Nullable<decimal> Cost { get; set; }
        public string Description { get; set; }
        public int OutsourceId { get; set; }
    }

    public class OutputServiceViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string OutputURL { get; set; }
        public Nullable<decimal> Price { get; set; }
        public Nullable<decimal> Cost { get; set; }
        public string Description { get; set; }
    }

    public class ServiceFromKeeper
    {
        public int CustomerId { get; set; }
        public List<Service> ServiceCollection { get; set; }
    }

    public class PromotionCalculator
    {
        private readonly string PromotionDefaultName = "No Promotion";
        private readonly string PromotionDiscountDefaultName ="No Discount Available";
        private bool hasPromotion;
        private PromotionViewModel currentPromotion;

        public string EstimatePrice { get; set; }
        public string TotalPrice { get; set; }
        public string TotalPriceBeforeTax { get; set; }
        public string PromotionDiscount { get; set; }
        public string PromotionName { get; set; }
        public string ServiceTax { get; set; }

        public PromotionCalculator()
        {
            init();
        }

        public PromotionCalculator(PromotionViewModel promotion) 
        {
            if (promotion != null)
            {
                hasPromotion = true;
                currentPromotion = promotion;
            }
            else 
            {
               init();
            }

        }

        private void init()
        {
            hasPromotion = false;
            PromotionName = PromotionDefaultName;
            PromotionDiscount = PromotionDiscountDefaultName;
            EstimatePrice = "0";
            TotalPrice = "0";
            TotalPriceBeforeTax = "0";
            ServiceTax = "0";
        }


        public void CalculateCurrentPrice(decimal PhotographPrice, decimal EquipmentPrice, decimal LocationPrice, decimal OutsourcePrice, decimal OutputPrice)
        {
            decimal _photoGraphPrice = PhotographPrice;
            decimal _equipmentPrice = EquipmentPrice;
            decimal _locationPrice = LocationPrice;
            decimal _outsourcePrice = OutsourcePrice;
            decimal _outputPrice = OutputPrice;
            decimal _totalPriceBeforeTax = 0;


            if (hasPromotion)
            {
                EstimatePrice = (PhotographPrice + EquipmentPrice + LocationPrice + OutsourcePrice + OutputPrice).ToString("C2",CultureInfo.CurrentCulture);
                PromotionName = currentPromotion.PromotionName();
                PromotionDiscount = ((currentPromotion.PhotoGraphDiscount + currentPromotion.EquipmentDiscount +
                                                     currentPromotion.LocationDiscount + currentPromotion.OutsourceDiscount + currentPromotion.OutputDiscount) / (decimal)500).ToString("P");
                _totalPriceBeforeTax = ((PhotographPrice * currentPromotion.PhotoGraphDiscount)
                    + (EquipmentPrice * currentPromotion.EquipmentDiscount)
                    + (LocationPrice * currentPromotion.LocationDiscount)
                    + (OutsourcePrice * currentPromotion.OutsourceDiscount)
                    + (OutputPrice * currentPromotion.OutputDiscount)
                    );

                TotalPriceBeforeTax = _totalPriceBeforeTax.ToString("C2", CultureInfo.CurrentCulture);
                TotalPrice = (_totalPriceBeforeTax * (decimal)10 / (decimal)100).ToString("C2", CultureInfo.CurrentCulture);
            }
            else
            {
                EstimatePrice = (PhotographPrice + EquipmentPrice + LocationPrice + OutsourcePrice + OutputPrice).ToString("C2", CultureInfo.CurrentCulture);
                PromotionName = PromotionDefaultName;
                PromotionDiscount = PromotionDiscountDefaultName;
                _totalPriceBeforeTax = ((PhotographPrice)
                    + (EquipmentPrice)
                    + (LocationPrice)
                    + (OutsourcePrice)
                    + (OutputPrice)
                    );
                TotalPriceBeforeTax = _totalPriceBeforeTax.ToString("C2", CultureInfo.CurrentCulture);
                TotalPrice = (_totalPriceBeforeTax * (decimal)10 / (decimal)100).ToString("C2", CultureInfo.CurrentCulture);
            }
        }
    }
}