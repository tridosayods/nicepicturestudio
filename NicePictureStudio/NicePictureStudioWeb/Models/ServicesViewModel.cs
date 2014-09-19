using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using NicePictureStudio.App_Data;

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

        public void CreateLocationServiceList(LocationService location)
        {
            LocationServiceViewModel _locationService = new LocationServiceViewModel();
            _locationService.Id = location.Id;
            _locationService.Name = location.Name;
            _locationService.IsOverNight = location.IsOverNight;
            _locationService.OverNightPeriod = location.OverNightPeriod;
            _locationService.Price = location.Price;
            _locationService.Cost = location.Cost;
            _locationService.LocationId = location.Location.LocationId;
            _locationService.Description = location.Description;
            ListLocationServices.Add(_locationService);
        }

        public void CreateOutSoruceServiceList(OutsourceService outsource)
        {
            OutsourceServiceViewModel _outsourceService = new OutsourceServiceViewModel();
            _outsourceService.Id = outsource.Id;
            _outsourceService.Name = outsource.Name;
            _outsourceService.OutsourceId = outsource.OutsourceContact.OutsourceContactId; // Only ID is required
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
            _outputService.OutputId = output.Id;
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
        public int OutputId { get; set; }
    }

    public class ServiceFromKeeper
    {
        public int CustomerId { get; set; }
        public List<Service> ServiceCollection { get; set; }
    }
}