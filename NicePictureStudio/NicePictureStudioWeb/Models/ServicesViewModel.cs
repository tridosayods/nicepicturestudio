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
        public PromotionCalculator Calculator { get; set; }

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
            Customer.Email = customer.Email;
            Customer.PhoneNumber = customer.PhoneNumber;
            Customer.PostcalCode = customer.PostcalCode;
            Customer.ReferenceEmail = customer.ReferenceEmail;
            Customer.ReferencePerson = customer.ReferencePerson;
            Customer.ReferencePhoneNumber = customer.ReferencePhoneNumber;
            Customer.CustomerTitle = customer.CustomerTitle;
            Customer.CustomerSurname = customer.CustomerSurname;

            Customer.CustomerSurname  = customer.CustomerSurname;
            Customer.CoupleTitle = customer.CoupleName;
            Customer.CoupleName = customer.CoupleName;
            Customer.CoupleSurname =customer.CoupleSurname;
             Customer.CouplePhoneNumber = customer.CouplePhoneNumber;
             Customer.BuildingBlock = customer.BuildingBlock;
             Customer.Road = customer.Road;
             Customer.Subdistrict = customer.Subdistrict;
             Customer.District = customer.District;
             Customer.Province = customer.Province;
             Customer.Country = customer.Country;
             Customer.CoupleEmail = customer.CoupleEmail;
             Customer.ReferenceTitle = customer.ReferenceTitle;
             Customer.ReferenceSurname = customer.ReferenceSurname;
             Customer.CustomerNickname = customer.CustomerNickname;
             Customer.CoupleNickname = customer.CoupleNickname;
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

        public void CreateServiceForm(ServiceForm serviceForm, int statusId, int typeId, int? locationId)
        {
            ServiceForm = new ServiceFormViewModel();
            ServiceForm.Id = serviceForm.Id;
            ServiceForm.Name = serviceForm.Name;
            ServiceForm.ServiceType = typeId;
            ServiceForm.Status = statusId;
            ServiceForm.EventStart = serviceForm.EventStart;
            ServiceForm.EventEnd = serviceForm.EventEnd;
            ServiceForm.GuestsNumber = serviceForm.GuestsNumber;
            ServiceForm.IsLocationSelected = (bool)serviceForm.IsLocationSelected;
            ServiceForm.IsOvernightSelected = (bool)serviceForm.IsLocationSelected;
            ServiceForm.LocationId = locationId == null ? 0 : (int)locationId;
        }

        //For Edit purpose => using Clone wording
        public void CloneServiceForm(ServiceForm serviceForm)
        {
            ServiceForm = new ServiceFormViewModel();
            ServiceForm.Id = serviceForm.Id;
            ServiceForm.Name = serviceForm.Name;
            ServiceForm.ServiceType = serviceForm.ServiceType.Id;
            ServiceForm.Status = serviceForm.ServiceStatu.Id;
            ServiceForm.EventStart = serviceForm.EventStart;
            ServiceForm.EventEnd = serviceForm.EventEnd;
            ServiceForm.GuestsNumber = serviceForm.GuestsNumber;
            ServiceForm.ServiceCost = Convert.ToDecimal(serviceForm.ServiceCost);
            ServiceForm.ServicePrice = Convert.ToDecimal(serviceForm.ServicePrice);
            ServiceForm.ServiceNetPrice = Convert.ToDecimal(serviceForm.ServiceNetPrice);
            ServiceForm.LocationId = serviceForm.Locations.Count <= 0 ? 0 : serviceForm.Locations.FirstOrDefault().LocationId;
        }

        public void UpdateServiceForm(ServiceForm serviceForm)
        {

            ServiceForm.Name = serviceForm.Name; 
            ServiceForm.EventStart = serviceForm.EventStart;
            ServiceForm.EventEnd = serviceForm.EventEnd;
            ServiceForm.GuestsNumber = serviceForm.GuestsNumber;
        }

        public void CreatePhotoGraphService(PhotographService photoGraph, List<string> photoGraphList, List<string> cameraManList, int photoGraphServiceId, int cntPHoutsource, int cntCMoutsource)
        {
            decimal? _price = 0;
            decimal? _cost = 0;
            decimal? totalPrice = photoGraph.Price;
            decimal? totalCost = photoGraph.Cost;
            PhotoGraphService = new PhotoGraphServiceViewModel();
            PhotoGraphService.Id = photoGraph.Id;
            PhotoGraphService.Name = photoGraph.Name;
            PhotoGraphService.PhotographerNumber = photoGraph.PhotographerNumber;
            if (photoGraph.PhotographerNumber != photoGraphList.Count)
            { _price += ((photoGraph.PhotographerNumber - photoGraphList.Count) * 4500); }
            if (photoGraph.CameraManNumber != cameraManList.Count)
            { _price += ((photoGraph.CameraManNumber - cameraManList.Count) * 6000); }
            PhotoGraphService.Price = totalPrice - _price;
            PhotoGraphService.CameraManNumber = photoGraph.CameraManNumber;
            if (photoGraph.PhotographerNumber != photoGraphList.Count)
            { _cost += ((photoGraph.PhotographerNumber - photoGraphList.Count) * 3000); }
            if (photoGraph.CameraManNumber != cameraManList.Count)
            { _cost += ((photoGraph.CameraManNumber - cameraManList.Count) * 4000); }
            PhotoGraphService.Cost = totalCost - _cost;
            PhotoGraphService.Description = photoGraph.Description;
            PhotoGraphService.PhotoGraphServiceId = photoGraphServiceId;
            PhotoGraphService.PhotoGraphIdList = new List<string>(photoGraphList);
            PhotoGraphService.CameraMandIdList = new List<string>(cameraManList);
            PhotoGraphService.CameraManOrcCnt = cntCMoutsource;
            PhotoGraphService.PhotoGraphOrcCnt = cntPHoutsource;
        }

       
        public void CreateEquipmentServiceList(EquipmentService equipment, int equipmentId, int equipmentServiceId)
        {
            EquipmentServiceViewModel _equipmentService = new EquipmentServiceViewModel();
            _equipmentService.Id = equipmentServiceId;
            _equipmentService.Name = equipment.Name;
            _equipmentService.Price = equipment.Price;
            _equipmentService.Cost = equipment.Cost;
            _equipmentService.Description = equipment.Description;
            _equipmentService.EquipmentId = equipmentId;
            _equipmentService.EquipmentServiceId = equipmentServiceId;
            if (!ListEquipmentServices.Exists(element => element.Id == _equipmentService.Id))
            {ListEquipmentServices.Add(_equipmentService);}
        }

        public void CreateLocationServiceList(LocationService location, int locationId, int locationServiceId)
        {
            LocationServiceViewModel _locationService = new LocationServiceViewModel();
            _locationService.Id = locationServiceId; //location.id
            _locationService.Name = location.Name;
            _locationService.IsOverNight = location.IsOverNight;
            _locationService.OverNightPeriod = location.OverNightPeriod;
            _locationService.Price = location.Price;
            _locationService.Cost = location.Cost;
            _locationService.LocationId = locationId;
            _locationService.Description = location.Description;
            _locationService.LocationServiceId = locationServiceId;
            if (!ListLocationServices.Exists(element =>element.Id == _locationService.Id))
            { ListLocationServices.Add(_locationService); }
        }

        public void CreateOutSoruceServiceList(OutsourceService outsource, int oursourceId, int outsourceServiceId, bool IsSelectFromPhotograph = false, bool IsSelectFromCameraman = false)
        {
            OutsourceServiceViewModel _outsourceService = new OutsourceServiceViewModel();
            _outsourceService.Id = outsourceServiceId; //outsource.id
            _outsourceService.Name = outsource.Name;
            _outsourceService.OutsourceId = oursourceId; // Only ID is required
            _outsourceService.PortFolioURL = outsource.PortFolioURL;
            _outsourceService.Price = outsource.Price;
            _outsourceService.Cost = outsource.Cost;
            _outsourceService.Description = outsource.Description;
            _outsourceService.OutsourceServiceId = outsourceServiceId;
            _outsourceService.IsSelectedFromPhotograph = IsSelectFromPhotograph;
            _outsourceService.IsSelectedFromCameraman = IsSelectFromCameraman;
            if (!ListOutsourceServices.Exists(element => element.Id == _outsourceService.Id))
            { ListOutsourceServices.Add(_outsourceService); }
        }

        public void CreateOutputServiceList(OutputService output,int outputServiceId, int outputQuantity,DateTime HandOnDate)
        {
            OutputServiceViewModel _outputService = new OutputServiceViewModel();
            _outputService.Id = outputServiceId; //output.id
            _outputService.Name = output.Name;
            _outputService.OutputURL = output.OutputURL;
            _outputService.Price = output.Price*outputQuantity;
            _outputService.Cost = output.Cost*outputQuantity;
            _outputService.Description = output.Description;
            _outputService.OutputServiceId = outputServiceId;
            _outputService.OutputQuantity = outputQuantity;
            _outputService.HandOnDate = HandOnDate;
            if (!ListOutputServices.Exists(element=>element.Id == _outputService.Id))
            { ListOutputServices.Add(_outputService);}
           
        }
    }

    public class CustomerViewModel
    {
        public int CustomerId { get; set; }
        public string CustomerName { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        public string Email { get; set; }
        public string PostcalCode { get; set; }
        public Nullable<System.DateTime> AnniversaryDate { get; set; }
        public string ReferencePerson { get; set; }
        public string ReferenceEmail { get; set; }
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
        public string ReferenceTitle { get; set; }
        public string ReferenceSurname { get; set; }
        public string CustomerNickname { get; set; }
        public string CoupleNickname { get; set; }
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
                PromotionId = promotion.Id;
            }
           
        }

        public int PromotionId;
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
        public decimal ServiceCost { get; set; }
        public decimal ServicePrice { get; set; }
        public decimal ServiceNetPrice { get; set; }
        public bool IsLocationSelected { get; set; }
        public bool IsOvernightSelected { get; set; }
        public int LocationId { get; set; }
    }

    public class PhotoGraphServiceViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int PhotographerNumber { get; set; }
        public int CameraManNumber { get; set; }
        public int PhotoGraphServiceId { get; set; }
        public string Description { get; set; }
        public Nullable<decimal> Cost { get; set; }
        public Nullable<decimal> Price { get; set; }
        public List<string> PhotoGraphIdList { get; set; }
        public List<string> CameraMandIdList { get; set; }
        public bool IsSelected { get; set; }
        public int PhotoGraphOrcCnt { get; set; }
        public int CameraManOrcCnt { get; set; }

        public void generatePhotoGraphService(PhotographService _photoGraphService)
        { 
            
        }
    }

    public class PhotoGraph
    { 
        public string Id { get; set; }
        public string Name { get; set; }
        public string Specialability { get; set; }
        public bool IsSelect { get; set; }

        public PhotoGraph() {}

        public PhotoGraph(Employee photograph, List<string> selectedId)
        {
            Id = photograph.Id;
            Name = photograph.Name;
            Specialability = photograph.Specialability;
            IsSelect = selectedId.Contains(photograph.Id);
        }
    }

    public class CameraMan
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public bool IsSelect { get; set; }
        public string Specialability { get; set; }

        public CameraMan() { }
        public CameraMan(Employee cameraman, List<string> selectedId)
        {
            Id = cameraman.Id;
            Name = cameraman.Name;
            Specialability = cameraman.Specialability;
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
        public int EquipmentServiceId { get; set; }
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
        public int LocationServiceId { get; set; }
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
        public int OutsourceServiceId { get; set; }
        public bool IsSelectedFromPhotograph { get; set; }
        public bool IsSelectedFromCameraman { get; set; }
    }

    public class OutputServiceViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string OutputURL { get; set; }
        public Nullable<decimal> Price { get; set; }
        public Nullable<decimal> Cost { get; set; }
        public string Description { get; set; }
        public int OutputServiceId { get; set; }
        public int OutputQuantity { get; set; }
        public DateTime HandOnDate { get; set; }
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

        public string PhotoGraphPriceText { get; set; }
        public string EquipmentPriceText { get; set; }
        public string LocationPriceText { get; set; }
        public string OutsourcePriceText { get; set; }
        public string OutputPriceText { get; set; }

        public decimal PhotoNetPrice { get; set; }
        public decimal EquipmentNetPrice { get; set; }
        public decimal LocationNetPrice { get; set; }
        public decimal OutsourceNetPrice { get; set; }
        public decimal OutputNetPrice { get; set; }

        public string EstimatePrice { get; set; }
        public string TotalPrice { get; set; }
        public string TotalPriceBeforeTax { get; set; }
        public string PromotionDiscount { get; set; }
        public string PromotionName { get; set; }
        public string ServiceTax { get; set; }

        public decimal NetPriceWithoutTax { get; set; }
        public decimal PriceWithoutTax { get; set; }

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
            EstimatePrice = "0.00";
            TotalPrice = "0.00";
            TotalPriceBeforeTax = "0.00";
            ServiceTax = "0.00";
        }


        public void CalculateCurrentPrice(decimal PhotographPrice, decimal EquipmentPrice, decimal LocationPrice, decimal OutsourcePrice, decimal OutputPrice)
        {
            decimal _photoGraphPrice = PhotographPrice;
            decimal _equipmentPrice = EquipmentPrice;
            decimal _locationPrice = LocationPrice;
            decimal _outsourcePrice = OutsourcePrice;
            decimal _outputPrice = OutputPrice;
            decimal _totalPriceBeforeTax = 0;
            decimal _totalVat = 0;

            PhotoNetPrice = PhotographPrice;
            EquipmentNetPrice = EquipmentPrice;
            LocationNetPrice = LocationPrice;
            OutsourceNetPrice = OutsourcePrice;
            OutputNetPrice = OutputPrice;

            PhotoGraphPriceText = PhotographPrice.ToString("0,0.000");
            EquipmentPriceText = EquipmentPrice.ToString("0,0.000");
            LocationPriceText = LocationPrice.ToString("0,0.000");
            OutsourcePriceText = OutsourceNetPrice.ToString("0,0.000");
            OutputPriceText = OutputPrice.ToString("0,0.000");
            
            if (hasPromotion)
            {

                EstimatePrice = (PhotographPrice + EquipmentPrice + LocationPrice + OutsourcePrice + OutputPrice).ToString("0,0.000", CultureInfo.CurrentCulture);
                PromotionName = currentPromotion.PromotionName();
                PromotionDiscount = ((currentPromotion.PhotoGraphDiscount + currentPromotion.EquipmentDiscount +
                                                   currentPromotion.LocationDiscount + currentPromotion.OutsourceDiscount + currentPromotion.OutputDiscount) / (decimal)500).ToString("P");
                _totalPriceBeforeTax = ((PhotographPrice -(PhotographPrice * currentPromotion.PhotoGraphDiscount / (decimal)100))
                    + (EquipmentPrice - (EquipmentPrice * currentPromotion.EquipmentDiscount / (decimal)100))
                    + (LocationPrice -(LocationPrice * currentPromotion.LocationDiscount / (decimal)100))
                    + (OutsourcePrice - (OutsourcePrice * currentPromotion.OutsourceDiscount / (decimal)100))
                    + (OutputPrice - (OutputPrice * currentPromotion.OutputDiscount / (decimal)100))
                    );
                PriceWithoutTax = _totalPriceBeforeTax;
                TotalPriceBeforeTax = _totalPriceBeforeTax.ToString("0,0.000", CultureInfo.CurrentCulture);
                _totalVat = (_totalPriceBeforeTax * (decimal)10 / (decimal)100);
                NetPriceWithoutTax = _totalPriceBeforeTax + _totalVat;
                TotalPrice = (_totalPriceBeforeTax + _totalVat).ToString("0,0.000", CultureInfo.CurrentCulture);
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
                PriceWithoutTax = _totalPriceBeforeTax;
                TotalPriceBeforeTax = _totalPriceBeforeTax.ToString("C2", CultureInfo.CurrentCulture);
                _totalVat = _totalPriceBeforeTax - (_totalPriceBeforeTax * (decimal)10 / (decimal)100);
                NetPriceWithoutTax = _totalPriceBeforeTax + _totalVat;
                TotalPrice = (_totalPriceBeforeTax + _totalVat).ToString("C2", CultureInfo.CurrentCulture);
            }
        }

        public void CalculateTotalPrice(decimal? estimatePrice, decimal? netPrice)
        {
            decimal _totalPriceBeforeTax = 0;
            decimal _totalVat = 0;
            
            if (hasPromotion)
            {
                EstimatePrice = estimatePrice.Value.ToString("C2",CultureInfo.CurrentCulture);
                PromotionName = currentPromotion.PromotionName();
                PromotionDiscount = ((currentPromotion.PhotoGraphDiscount + currentPromotion.EquipmentDiscount +
                                                   currentPromotion.LocationDiscount + currentPromotion.OutsourceDiscount + currentPromotion.OutputDiscount) / (decimal)500).ToString("P");
                _totalPriceBeforeTax = ((decimal)netPrice - ((decimal)netPrice * currentPromotion.OutputDiscount / (decimal)100));
                PriceWithoutTax = _totalPriceBeforeTax;
                TotalPriceBeforeTax = _totalPriceBeforeTax.ToString("C2", CultureInfo.CurrentCulture);
                _totalVat = (_totalPriceBeforeTax * (decimal)10 / (decimal)100);
                NetPriceWithoutTax = _totalPriceBeforeTax + _totalVat;
                TotalPrice = (_totalPriceBeforeTax + _totalVat).ToString("C2", CultureInfo.CurrentCulture);
            }
            else
            {
                EstimatePrice = estimatePrice.Value.ToString("C2", CultureInfo.CurrentCulture);
                PromotionName = PromotionDefaultName;
                PromotionDiscount = PromotionDiscountDefaultName;
                _totalPriceBeforeTax = (decimal)netPrice;
                PriceWithoutTax = _totalPriceBeforeTax;
                TotalPriceBeforeTax = _totalPriceBeforeTax.ToString("C2", CultureInfo.CurrentCulture);
                _totalVat = (_totalPriceBeforeTax * (decimal)10 / (decimal)100);
                NetPriceWithoutTax = _totalPriceBeforeTax + _totalVat;
                TotalPrice = (_totalPriceBeforeTax + _totalVat).ToString("C2", CultureInfo.CurrentCulture);
            }
        }
    }

    public class ServiceGridViewModel
    {
        public int Id { get; set; }
        public string BookingName { get; set; }
        public string GroomName { get; set; }
        public string BrideName { get; set; }
        public string SpecialRequest { get; set; }
        public Nullable<decimal> Payment { get; set; }
        public Nullable<decimal> PayAmount { get; set; }
        
        public string CustomerName { get; set; }
        public string ServiceStatus { get; set; }
    }

    public class ServiceFormsGridViewModel
    {
        public int Id { get; set; }
        public string BookingNumber { get; set; }
        public string CustomerName { get; set; }
        public string Status { get; set; }
        public DateTime EventStart { get; set; }
        public DateTime EventEnd { get; set; }
        public string Place { get; set; }

    }
}