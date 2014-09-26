using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using NicePictureStudio.App_Data;
using NicePictureStudio.Models;
using System.Data.Entity.Validation;
using System.Diagnostics;

namespace NicePictureStudio
{
    public class ServicesController : Controller
    {
        private NicePictureStudioDBEntities db = new NicePictureStudioDBEntities();

        private ServicesViewModel _services;
        private ServicesViewModel _servicesEdit;
        private PromotionCalculator _promotionCalculator;
        private PromotionCalculator _promotionCalculatorEdit;
        private PromotionViewModel _promotion;

        private readonly int PREWEDDING_SERVICE = 1;
        private readonly int ENGAGEMENT_SERVICE = 2;
        private readonly int WEDDING_SERVICE = 3;

        private static readonly string CameraManType = "CameraMan";
        private static readonly string PhotographType = "PhotoGraph";
 
        //Command
        public static readonly string PreWedding = "PreWedding";
        public readonly string Engagement = "Engagement";
        public readonly string Wedding = "Wedding";
        public readonly string HTMLTagForReplace = "FormSection";
        public readonly string HTMLTagForDatePickerStart = "DatepickerStart";
        public readonly string HTMLTagForDatePickerEnd = "DatepickerEnd";
        
        //Modal
        public readonly string HTMLModalPhotoGraph = "modalPhotoGraph";
        public readonly string HTMLModalEquipment = "modalEquipment";
        public readonly string HTMLModalLocation = "modalLocation";
        public readonly string HTMLModalOutSource = "modalOutsource";
        public readonly string HTMLModalOutput = "modalOutput";

        public readonly string HTMLModalPhotoGraphArrow = "modalPhotoGraphArrow";
        public readonly string HTMLModalEquipmentArrow = "modalEquipmentArrow";
        public readonly string HTMLModalLocationArrow = "modalLocationArrow";
        public readonly string HTMLModalOutSourceArrow = "modalOutsourceArrow";
        public readonly string HTMLModalOutputArrow = "modalOutput";
        
        //Button
        public readonly string HTMLTagButtonPhotoGraph = "btnPhotoGraph";
        public readonly string HTMLTagButtonEquipment = "btnEquipment";
        public readonly string HTMLTagButtonLocation = "btnLocation";
        public readonly string HTMLTagButtonOutSource = "btnOutsource";
        public readonly string HTMLTagButtonOutput = "btnOutput";
        //Container
        public readonly string HTMLContainerButtonPhotoGraph = "ctnPhotoGraph";
        public readonly string HTMLContainerButtonEquipment = "ctnEquipment";
        public readonly string HTMLContainerButtonLocation = "ctnLocation";
        public readonly string HTMLContainerButtonOutSource = "ctnOutsource";
        public readonly string HTMLContainerButtonOutput = "ctnOutput";

        //DropDownList naming for each division
        //Add Photograph Service Page
        public readonly string HTMLDWLPhotoGraphEngagementServices = "dwlPhotoGraphEngagementService";
        public readonly string HTMLDWLPhotoGraphPreWeddingServices = "dwlPhotoGraphPreWeddingService";
        public readonly string HTMLDWLPhotoGraphWeddingServices = "dwlPhotoGraphWeddingService";

        //Add Equipment Service Page
        public readonly string HTMLDWLEquipmentEngagementServices = "dwlEquipmentEngagementService";
        public readonly string HTMLDWLEquipmentPreWeddingServices = "dwlEquipmentPreWeddingService";
        public readonly string HTMLDWLEquipmentWeddingServices = "dwlEquipmentWeddingService";

        public readonly string HTMLTableEquipmentEngagementServices = "tblEquipmentEngagementService";
        public readonly string HTMLTableEquipmentPreWeddingServices = "tblEquipmentPreWeddingService";
        public readonly string HTMLTableEquipmentWeddingServices = "tblEquipmentWeddingService";

        //Add Location Service Page
        public readonly string HTMLDWLLocationEngagementServices = "dwlLocationEngagementService";
        public readonly string HTMLDWLLocationPreWeddingServices = "dwlLocationPreWeddingService";
        public readonly string HTMLDWLLocationWeddingServices = "dwlLocationWeddingService";

        //Add Outsource Service Page
        public readonly string HTMLDWLOutsourceEngagementServices = "dwlOutsourceEngagementService";
        public readonly string HTMLDWLOutsourcePreWeddingServices = "dwlOutsourcePreWeddingService";
        public readonly string HTMLDWLOutsourceWeddingServices = "dwlOutsourceWeddingService";

        //Add Output Service Page
        public readonly string HTMLDWLOutputEngagementServices = "dwlOutputEngagementService";
        public readonly string HTMLDWLOutputPreWeddingServices = "dwlOutputPreWeddingService";
        public readonly string HTMLDWLOutputWeddingServices = "dwlOutputWeddingService";
        /******************************************************************************************************/

        //Create Service Form by division
        //Add Photograph Service Page
        public readonly string HTMLPhotoServiceModalPreWedding = "divPhotoGraphPreWedding";
        public readonly string HTMLPhotoServiceModalEngagement = "divPhotoGraphEngagement";
        public readonly string HTMLPhotoServiceModalWedding = "divPhotoGraphWedding";

        //Add Equipment Service Page
        public readonly string HTMLEquipmentModalPreWedding = "divEquipmentPreWedding";
        public readonly string HTMLEquipmentModalEngagement = "divEquipmentEngagement";
        public readonly string HTMLEquipmentModalWedding = "divEquipmentWedding";

        //Add Location Service Page
        public readonly string HTMLLocationModalPreWedding = "divLocationPreWedding";
        public readonly string HTMLLocationModalEngagement = "divLocationEngagement";
        public readonly string HTMLLocationModalWedding = "divLocationWedding";

        //Add Outsource Service Page
        public readonly string HTMLOutsourceModalPreWedding = "divOutsourcePreWedding";
        public readonly string HTMLOutsourceModalEngagement = "divOutsourceEngagement";
        public readonly string HTMLOutsourceModalWedding = "divOutsourceWedding";

        //Add Outsource Service Page
        public readonly string HTMLOutputModalPreWedding = "divOutputPreWedding";
        public readonly string HTMLOutputModalEngagement = "divOutputEngagement";
        public readonly string HTMLOutputModalWedding = "divOutputWedding";
        /******************************************************************************************************/

        //CreateCollapse form by division
        //Add Photograph Service Page
        public readonly string HTMLCollapsePhotoPreWedding = "collPhotoGraphPreWedding";
        public readonly string HTMLCollapsePhotoEngagement = "collPhotoGraphEngagement";
        public readonly string HTMLCollapsePhotoWedding = "collPhotoGraphWedding";

        public readonly string HTMLCollapseCameraPreWedding = "collCameraPreWedding";
        public readonly string HTMLCollapseCameraEngagement = "collCameraEngagement";
        public readonly string HTMLCollapseCameraWedding = "collCameraWedding";
        /******************************************************************************************************/

        //Create Table form by division
        //Add Equipment Service Page
        public readonly string HTMLTableEquipmentPreWedding = "tblEquipmentPreWedding";
        public readonly string HTMLTableEquipmentEngagement = "tblEquipmentEngagement";
        public readonly string HTMLTableEquipmentWedding = "tblEquipmentWedding";

        //Add Location Service Page
        public readonly string HTMLTableLocationPreWedding = "tblLocationPreWedding";
        public readonly string HTMLTableLocationEngagement = "tblLocationEngagement";
        public readonly string HTMLTableLocationWedding = "tblLocationWedding";

        //Add Location Service Page
        public readonly string HTMLTableOutsourcePreWedding = "tblOutsourcePreWedding";
        public readonly string HTMLTableOutsourceEngagement = "tblOutsourceEngagement";
        public readonly string HTMLTableOutsourceWedding = "tblOutsourceWedding";

        //Add Location Service Page
        public readonly string HTMLTableOutputPreWedding = "tblOutputPreWedding";
        public readonly string HTMLTableOutputEngagement = "tblOutputEngagement";
        public readonly string HTMLTableOutputWedding = "tblOutputWedding";

        public readonly string HTMLButtonEnabledForm = "btnEnabled";
        public readonly string HTMLButtonDisabledForm = "btnDisabled";
        /******************************************************************************************************/

        // GET: Services
        public async Task<ActionResult> Index()
        {
            var services = db.Services.Include(s => s.Customer);
            return View(await services.ToListAsync());
        }

        // GET: Services/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Service service = await db.Services.FindAsync(id);
            if (service == null)
            {
                return HttpNotFound();
            }
            return View(service);
        }

        // GET: Services/Create
        public ActionResult Create()
        {
            _services = new ServicesViewModel();
            TempData["Services"] = _services;
            ViewBag.CustomerId = new SelectList(db.Customers, "CustomerId", "CustomerName");
            ViewBag.BookingId = new SelectList(db.Bookings, "Id", "Name");
            //Binding a promotion from scracth or create new promotion
            ViewBag.BookingList = new SelectList(db.Bookings, "Id", "BookingName");
            return View();
        }

        public JsonResult GetListForBookingAutocomplete(string term)
        {
            // Booking staus = 1 means opened booking.
            int _bookingOpened = 1;
            Booking[] matching = string.IsNullOrWhiteSpace(term) ?
                db.Bookings.ToArray() :
                db.Bookings.Where(p => (p.BookingCode.ToUpper().StartsWith(term.ToUpper()) || p.Name.ToUpper().StartsWith(term.ToUpper())) && p.BookingStatu.Id == _bookingOpened).ToArray();

            return Json(matching.Select(m => new
            {
                id = m.Id,
                value = m.Name,
                label = m.Name
            }), JsonRequestBehavior.AllowGet);
        }


        // POST: Services/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,BookingName,GroomName,BrideName,SpecialRequest,Payment,PayAmount,CustomerId,CRMFormId")] Service service, int BookingId=0)
        {
            //booking status 2 => booking confirm
            int _bookingConfirm = 2;
            if (ModelState.IsValid)
            {
                //finding promotion
                //Declare Temp
                var _servicesTmp = TempData["Services"] as ServicesViewModel;
                TempData.Keep();

                Promotion promotion = new Promotion();
                if (BookingId > 0) { promotion = await db.Promotions.FindAsync(BookingId); }
                else { return HttpNotFound(); }

               
                _servicesTmp.Promotion = new PromotionViewModel(promotion);
                
                _promotionCalculator = new PromotionCalculator(_servicesTmp.Promotion);
                TempData["Promotion"] = _promotionCalculator;
                SummarizePrice();

                //Save to DB
                service.Customer = await db.Customers.FindAsync(_servicesTmp.Customer.CustomerId);
                db.Services.Add(service);
                db.SaveChanges();

                //Create Booking as booked
                Booking booking = await db.Bookings.FindAsync(BookingId);
                booking.Service = service;
                booking.BookingStatu = await db.BookingStatus.FindAsync(_bookingConfirm);
                db.Entry(booking).State = EntityState.Modified;

                var result = await db.SaveChangesAsync();

                //Afte finished saveing - creae cache in local memory
                _servicesTmp.CreateService(service);
                TempData["Services"] = _servicesTmp;
                return PartialView("DetailsService",service);
                //return PartialView(@"/Views/Services/CalculateServicesCostSummary.cshtml", _promotionCalculator);
            }

           // ViewBag.CustomerId = new SelectList(db.Customers, "CustomerId", "CustomerName", service.Id);
            return PartialView(@"/Views/Services/CalculateServicesCostSummary.cshtml", new PromotionCalculator());
        }

        public async Task<PartialViewResult> DetailsService(int? Id)
        {
            if (Id != null)
            {
                Service service = await db.Services.FindAsync(Id);
                if (service == null)
                {
                    return PartialView();
                }
                else
                {
                    return PartialView(service);
                }
            }
            else
            {
                return PartialView();
            }
        }


        public async Task<PartialViewResult> EditService(int? Id)
        {
            if (Id != null)
            {
                Service service = await db.Services.FindAsync(Id);
                if (service == null)
                {
                    return PartialView();
                }
                else
                {
                    return PartialView(service);
                }
            }
            return PartialView();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<PartialViewResult> EditService([Bind(Include = "Id,BookingName,GroomName,BrideName,SpecialRequest,Payment,PayAmount,CustomerId,CRMFormId")] Service service)
        {
            if (ModelState.IsValid)
            {
                var _servicesTmp = TempData["Services"] as ServicesViewModel;
                TempData.Keep();
                
                db.Entry(service).State = EntityState.Modified;
                await db.SaveChangesAsync();
                _servicesTmp.CreateService(service);
                TempData["Services"] = _servicesTmp;

                Service _service = service;
                return PartialView("DetailsService", _service);
            }
            return PartialView();
        }


        //public async Task<PartialViewResult> CreateService(int bookingId)
        //{

        //    Booking booking = await db.Bookings.FindAsync(bookingId);
        //    if (booking != null)
        //    {
        //        //Getting Promotion & Booking Information
        //            //_promotion = new PromotionViewModel(booking.Promotion.ExpireDate,booking.Promotion.PhotoGraphDiscount,
        //            //booking.Promotion.EquipmentDiscount,
        //            //booking.Promotion.LocationDiscount, booking.Promotion.OutputDiscount, 
        //            //booking.Promotion.OutsourceDiscount);
        //        //Preparing for creating Service
        //        //CreateService
        //        // One promotion affet to any package -> need to keep price as static
        //        // getting promotion and then extract trype of service -> create item.

        //    }
        //    return PartialView();
        //}

        // GET: Services/Edit/5

        #region Edit
       
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            /*Process of fetching data from DB*/
            _servicesEdit = new ServicesViewModel();
            Service service = db.Services.Find(id);
            var _booking = db.Bookings.Where(booking=>booking.Service.Id == service.Id).Select(booking=>booking).FirstOrDefault();
            var _promotion = db.Promotions.Where(promo => promo.Id == _booking.Promotion.Id).Select(promo => promo).FirstOrDefault();
            if (service == null)
            {
                return HttpNotFound();
            }
            _servicesEdit.CreateCustomer(service.Customer);
            _servicesEdit.CreateService(service);
            _servicesEdit.Promotion = new PromotionViewModel(_promotion);

            foreach (var serviceForm in service.ServiceForms)
            {
                if (serviceForm.ServiceType.Id == PREWEDDING_SERVICE)
                {
                    _servicesEdit.ServiceFormPreWedding = await RetrieveServiceFormsFromDB(serviceForm);
                }
                else if (serviceForm.ServiceType.Id == ENGAGEMENT_SERVICE)
                {
                    _servicesEdit.ServiceFormEngagement = await RetrieveServiceFormsFromDB(serviceForm);
                }
                else if (serviceForm.ServiceType.Id == WEDDING_SERVICE)
                {
                    _servicesEdit.ServiceFormWedding = await RetrieveServiceFormsFromDB(serviceForm);
                }
            }
            _promotionCalculatorEdit = new PromotionCalculator(_servicesEdit.Promotion);
            _servicesEdit.Calculator = _promotionCalculatorEdit;
            TempData["ServiceEdit"] = _servicesEdit;
            TempData["PromotionEdit"] = _promotionCalculatorEdit;
            SummarizePriceForEdit();

            ViewBag.CustomerId = new SelectList(db.Customers, "CustomerId", "CustomerName", service.Id);
            return View(_servicesEdit);
        }

        private async Task<ServiceFormFactory> RetrieveServiceFormsFromDB(ServiceForm serviceForm)
        {
            ServiceFormFactory _serviceFactory = new ServiceFormFactory();
            _serviceFactory.CloneServiceForm(serviceForm);

            var _empServiceList = serviceForm.EmployeeSchedules;
            var _equipmentServiceList = serviceForm.EquipmentSchedules;
            var _locationServiceList = serviceForm.LocationSchedules;
            var _outsourceServiceList = serviceForm.OutsourceSchedules;
            var _outputServiceList = serviceForm.OutputSchedules;

            //PHotograph service
            //select Photograph => photographservice , select employee for phtogotraphy, cameraman
            List<string> photoGraphIds = new List<string>();
            List<string> camearManIds = new List<string>();
            int photoGraphServiceId = _empServiceList.Select(emp => emp.EmployeeServiceId).FirstOrDefault();
            PhotographService photoService = await db.PhotographServices.FindAsync(photoGraphServiceId);
            foreach (var emp in _empServiceList)
            {
                if (emp.Employee.Position == PhotographType)
                {
                    photoGraphIds.Add(emp.Employee.Id);
                }
                else if (emp.Employee.Position == CameraManType)
                {
                    camearManIds.Add(emp.Employee.Id);
                }
            }
            if (photoService != null)
            _serviceFactory.CreatePhotoGraphService(photoService, photoGraphIds, camearManIds, photoGraphServiceId);

            foreach (var equipment in _equipmentServiceList)
            {
                EquipmentService equipmentService = await db.EquipmentServices.FindAsync(equipment.EquipmentServiceId);
                _serviceFactory.CreateEquipmentServiceList(equipmentService, equipmentService.Equipment.EquipmentId, equipment.EquipmentServiceId);
            }

            foreach (var location in _locationServiceList)
            {
                LocationService locationService = await db.LocationServices.FindAsync(location.LocationServiceId);
                _serviceFactory.CreateLocationServiceList(locationService, locationService.Location.LocationId, location.LocationServiceId);
            }

            foreach (var outsource in _outsourceServiceList)
            {
                OutsourceService outsourceService = await db.OutsourceServices.FindAsync(outsource.OutsourceServiceId);
                _serviceFactory.CreateOutSoruceServiceList(outsourceService, outsourceService.OutsourceContact.OutsourceContactId, outsource.OutsourceServiceId);
            }

            foreach (var output in _outputServiceList)
            {
                OutputService outputService = await db.OutputServices.FindAsync(output.OutputServiceId);
                {
                    _serviceFactory.CreateOutputServiceList(outputService,output.Id);
                }
            }

            return _serviceFactory;
        }

        // POST: Services/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<ActionResult> Edit([Bind(Include = "Id,BookingName,GroomName,BrideName,SpecialRequest,Payment,PayAmount,CustomerId,CRMFormId")] Service service)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        db.Entry(service).State = EntityState.Modified;
        //        await db.SaveChangesAsync();
        //        return RedirectToAction("Index");
        //    }
        //    ViewBag.CustomerId = new SelectList(db.Customers, "CustomerId", "CustomerName", service.Customer.CustomerId);
        //    return View(service);
        //}

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,BookingName,GroomName,BrideName,SpecialRequest,Payment,PayAmount,CustomerId,CRMFormId")] Service service, int BookingId = 0)
        {
            if (ModelState.IsValid)
            {
                //finding promotion
                var _servicesTmp = TempData["ServicesEdit"] as ServicesViewModel;
                var _promotionCalculatorTmp = TempData["PromotionEdit"] as PromotionCalculator;
                TempData.Keep();

                Promotion promotion = new Promotion();
                if (BookingId > 0) { promotion = await db.Promotions.FindAsync(BookingId); }
                else if (BookingId == 0)
                {
                    promotion = await db.Promotions.FindAsync(_servicesTmp.Promotion.PromotionId);
                }
                else { return HttpNotFound(); }

                _servicesTmp.Promotion = new PromotionViewModel(promotion);
                _promotionCalculatorTmp = new PromotionCalculator(_servicesTmp.Promotion);
                _servicesTmp.Calculator = _promotionCalculatorTmp;
                SummarizePriceForEdit();

                //Save to DB 
                //service.Customer = await db.Customers.FindAsync(_services.Customer.CustomerId);
                //db.Services.Add(service);
                //db.SaveChanges();
                db.Entry(service).State = EntityState.Modified;

                //Create Booking as booked
                if(BookingId > 0)
                { 
                    Booking booking = await db.Bookings.FindAsync(BookingId);
                    booking.Service = service;
                    //booking.BookingStatu = await db.BookingStatus.FindAsync(_bookingConfirm);
                    db.Entry(booking).State = EntityState.Modified;
                }
                var result = await db.SaveChangesAsync();

                //Afte finished saveing - creae cache in local memory
                _servicesTmp.CreateService(service);
                TempData["ServicesEdit"] = _servicesTmp;
                TempData["PromotionEdit"] = _promotionCalculatorTmp;

                return PartialView("DetailsServiceForEdit", service);
            }

            return PartialView(@"/Views/Services/CalculateServicesCostSummary.cshtml", new PromotionCalculator());
        }


        public async Task<PartialViewResult> DetailsServiceForEdit(int? Id)
        {
            if (Id != null)
            {
                Service service = await db.Services.FindAsync(Id);
                if (service == null)
                {
                    return PartialView();
                }
                else
                {
                    return PartialView(service);
                }
            }
            else
            {
                return PartialView();
            }
        }

        public async Task<PartialViewResult> EditServiceWhenEdit(int? Id)
        {
            if (Id != null)
            {
                Service service = await db.Services.FindAsync(Id);
                if (service == null)
                {
                    return PartialView();
                }
                else
                {
                    return PartialView(service);
                }
            }
            return PartialView();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<PartialViewResult> EditServiceWhenEdit([Bind(Include = "Id,BookingName,GroomName,BrideName,SpecialRequest,Payment,PayAmount,CustomerId,CRMFormId")] Service service)
        {
            if (ModelState.IsValid)
            {
                var _servicesTmp = TempData["ServiceEdit"] as ServicesViewModel;
                TempData.Keep();

                db.Entry(service).State = EntityState.Modified;
                await db.SaveChangesAsync();
                _servicesTmp.CreateService(service);
                TempData["ServiceEdit"] = _servicesTmp;

                Service _service = service;
                return PartialView("DetailsService", _service);
            }
            return PartialView();
        }

        public async Task<PartialViewResult> DetailsCustomerFromServiceWhenEdit(int? id)
        {
            if (id != null)
            {
                Customer customer = await db.Customers.FindAsync(id);
                if (customer == null)
                {
                    return PartialView();
                }
                else
                {
                    return PartialView(customer);
                }
            }
            else
            {
                return PartialView();
            }
        }


        public async Task<PartialViewResult> EditCustomerFromServiceWhenEdit(int? id)
        {
            if (id != null)
            {
                Customer customer = await db.Customers.FindAsync(id);

                if (customer == null)
                {
                    return PartialView();
                }
                else
                {
                    CustomerViewModel customerView = new CustomerViewModel
                    {
                        CustomerId = customer.CustomerId,
                        Address = customer.Address,
                        AnniversaryDate = customer.AnniversaryDate,
                        City = customer.City,
                        CustomerName = customer.CustomerName,
                        Email = customer.Email,
                        PhoneNumber = customer.PhoneNumber,
                        PostcalCode = customer.PostcalCode,
                        ReferenceEmail = customer.PostcalCode,
                        ReferencePerson = customer.ReferencePerson,
                        ReferencePhoneNumber = customer.ReferencePhoneNumber
                    };
                    return PartialView(customerView);
                }
            }
            return PartialView();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<PartialViewResult> EditCustomerFromServiceWhenEdit([Bind(Include = "CustomerId,CustomerName,PhoneNumber,Address,AnniversaryDate,City,Email,PostcalCode,ReferencePerson,ReferenceEmail,ReferencePhoneNumber")] Customer customer)
        {
            if (ModelState.IsValid)
            {
                var _servicesEditTmp = TempData["ServiceEdit"] as ServicesViewModel;
                TempData.Keep("ServiceEdit");

                db.Entry(customer).State = EntityState.Modified;
                await db.SaveChangesAsync();
                _servicesEditTmp.CreateCustomer(customer);
                TempData["ServiceEdit"] = _servicesEditTmp;
                Customer _customer = customer;
                return PartialView("DetailsCustomerFromServiceWhenEdit", _customer);
            }
            return PartialView();
        }


        #endregion 

        #region delete

        // GET: Services/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Service service = await db.Services.FindAsync(id);
            if (service == null)
            {
                return HttpNotFound();
            }
            return View(service);
        }

        // POST: Services/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Service service = await db.Services.FindAsync(id);
            db.Services.Remove(service);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        #endregion 

        #region Create Customer Section
        /***************************Create Customer Section*******************************************************/

        public PartialViewResult CreateCustomerFromService()
        {
            return PartialView();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<PartialViewResult> CreateCustomerFromService([Bind(Include = "CustomerId,CustomerName,PhoneNumber,Address,AnniversaryDate,City,Email,PostcalCode,ReferencePerson,ReferenceEmail,ReferencePhoneNumber")] Customer customer)
        {
            if (ModelState.IsValid)
            {
                db.Customers.Add(customer);
                await db.SaveChangesAsync();
                var _servicesTmp = TempData["Services"] as ServicesViewModel;
                TempData.Keep();
                _servicesTmp.CreateCustomer(customer);
                TempData["Services"] = _servicesTmp;
                Customer _customer = customer;
                return PartialView(@"/Views/Services/DetailsCustomerFromService.cshtml", _customer);
            }

            return PartialView();
        }

        public async Task<PartialViewResult> DetailsCustomerFromService(int? id)
        {
            if (id != null)
            {
                Customer customer = await db.Customers.FindAsync(id);
                if (customer == null)
                {
                    return PartialView();
                }
                else
                {
                    return PartialView(customer);
                }
            }
            else
            {
                return PartialView();
            }
        }

        

        public async Task<PartialViewResult> EditCustomerFromService(int? id)
        {
            if (id != null)
            {
                Customer customer = await db.Customers.FindAsync(id);
                if (customer == null)
                {
                    return PartialView();
                }
                else
                {
                    return PartialView(customer);
                }
            }
            return PartialView();
        }

        

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<PartialViewResult> EditCustomerFromService([Bind(Include = "CustomerId,CustomerName,PhoneNumber,Address,AnniversaryDate,City,Email,PostcalCode,ReferencePerson,ReferenceEmail,ReferencePhoneNumber")] Customer customer)
        {
            if (ModelState.IsValid)
            {
                var _servicesTmp = TempData["Services"] as ServicesViewModel;
                TempData.Keep();

                db.Entry(customer).State = EntityState.Modified;
                await db.SaveChangesAsync();
                _servicesTmp.CreateCustomer(customer);
                TempData["Services"] = _servicesTmp;
                Customer _customer = customer;
                return PartialView(@"/Views/Services/DetailsCustomerFromService.cshtml", _customer);
            }
            return PartialView();
        }

      

        /*Get*/
        public async Task<PartialViewResult> ViewCustomerFromService(int customerid)
        {
            Customer model = await db.Customers.FindAsync(customerid);
            return PartialView(model);
        }


        /***************************Create Customer Section*******************************************************/
        #endregion


        #region Create Service Form Section
        /***************************Create Service Form Section*******************************************************/

        public async Task<PartialViewResult> GetServiceFormById(int serviceFormId)
        {
            ServiceForm model = await db.ServiceForms.FindAsync(serviceFormId);
            return PartialView(model);
        }

          public PartialViewResult CreateServiceFormFromService(string Command)
          {
            //assign value for replacing #id in view
            ViewBag.FormContorl = Command + " form-control";
            ViewBag.ServiceTypeItem = Command;
            ViewBag.ServiceTypeTag = Command + HTMLTagForReplace;
            ViewBag.ServiceTypeDatePickerStart = Command + HTMLTagForDatePickerStart;
            ViewBag.ServiceTypeDatePickerEnd = Command + HTMLTagForDatePickerEnd;
            ViewBag.HTMLTagButtonPhotoGraph = HTMLTagButtonPhotoGraph + Command;
            ViewBag.HTMLTagButtonEquipment = HTMLTagButtonEquipment + Command;
            ViewBag.HTMLTagButtonLocation = HTMLTagButtonLocation + Command;
            ViewBag.HTMLTagButtonOutSource = HTMLTagButtonOutSource + Command;
            ViewBag.HTMLTagButtonOutput = HTMLTagButtonOutput + Command;
            ViewBag.HTMLContainerButtonPhotoGraph = HTMLContainerButtonPhotoGraph + Command;
            ViewBag.HTMLContainerButtonEquipment = HTMLContainerButtonEquipment + Command;
            ViewBag.HTMLContainerButtonLocation = HTMLContainerButtonLocation + Command;
            ViewBag.HTMLContainerButtonOutSource = HTMLContainerButtonOutSource + Command;
            ViewBag.HTMLContainerButtonOutput = HTMLContainerButtonOutput + Command;
            ViewBag.HTMLModalPhotoGraph = HTMLModalPhotoGraph + Command;
            ViewBag.HTMLModalEquipment = HTMLModalEquipment + Command;
            ViewBag.HTMLModalLocation = HTMLModalLocation + Command;
            ViewBag.HTMLModalOutSource = HTMLModalOutSource + Command;
            ViewBag.HTMLModalOutput = HTMLModalOutput + Command;
            ViewBag.HTMLModalPhotoGraphArrow = HTMLModalPhotoGraphArrow + Command;
            ViewBag.HTMLModalEquipmentArrow = HTMLModalEquipmentArrow + Command;
            ViewBag.HTMLModalLocationArrow = HTMLModalLocationArrow + Command;
            ViewBag.HTMLModalOutSourceArrow = HTMLModalOutSourceArrow + Command;
            ViewBag.HTMLModalOutputArrow = HTMLModalOutputArrow + Command;
            ViewBag.HTMLButtonEnabledForm = HTMLButtonEnabledForm + Command;
            ViewBag.HTMLButtonDisabledForm = HTMLButtonDisabledForm + Command;
            return PartialView();
        }

          [HttpPost]
          [ValidateAntiForgeryToken]
          public void CreateServiceFormFromService([Bind(Include = "Name,Status,EventStart,EventEnd,GuestsNumber")] ServiceForm serviceForm, string Command)
          {
              int statusNew = 1;
              if (ModelState.IsValid)
              {
                  ServiceType serviceType = db.ServiceTypes.Where(s => string.Compare(s.ServiceTypeName, Command, true) == 0).FirstOrDefault();
                  if (serviceType != null)
                  {
                      serviceForm.ServiceType = serviceType;
                      //create string for mapping
                      string _mappingServiceType = serviceType.ServiceTypeName + HTMLTagForReplace;
                      ServiceFormFactory serviceFactory = CreateServiceFormByInputSection(_mappingServiceType);
                      if (serviceFactory != null)
                      {
                          //db.ServiceForms.Add(serviceForm);
                          //db.SaveChangesAsync();
                          serviceFactory.CreateServiceForm(serviceForm, statusNew, serviceType.Id);
                      }
                      //else { return PartialView(); }
                     // _services.CreateServiceForm(serviceForm, statusNew, serviceType.Id);
                  }
                 // else { return PartialView(); }

                  //assign value for replacing #id in view
                  //ServiceForm _serviceForm = serviceForm;
                  //return PartialView(@"/Views/ServiceForms/DetailsServiceFormFromService.cshtml", serviceForm);
              }
              //return PartialView();
          }


          public async Task<PartialViewResult> DetailsServiceFormFromService(int? id)
          {
              if (id != null)
              {
                  ServiceForm serviceForm = await db.ServiceForms.FindAsync(id);
                  if (serviceForm == null)
                  {
                      return PartialView();
                  }
                  else
                  {
                      //assign value for replacing #id in view
                      ViewBag.ServiceTypeItem = serviceForm.ServiceType.ServiceTypeName;
                      return PartialView(serviceForm);
                  }
              }
              else
              {
                  return PartialView();
              }
          }

          public async Task<PartialViewResult> EditServiceFormFromService(int? id)
          {
              if (id != null)
              {
                  ServiceForm serviceForm = await db.ServiceForms.FindAsync(id);
                  if (serviceForm == null)
                  {
                      return PartialView();
                  }
                  else
                  {
                      return PartialView(serviceForm);
                  }
              }
              return PartialView();
          }

          [HttpPost]
          [ValidateAntiForgeryToken]
          public async Task<PartialViewResult> EditServiceFormFromService([Bind(Include = "Id,Name,ServiceType,Status,EventStart,EventEnd,GuestsNumber")] ServiceForm serviceForm)
          {
              if (ModelState.IsValid)
              {
                  db.Entry(serviceForm).State = EntityState.Modified;
                  await db.SaveChangesAsync();
                  ServiceForm _serviceForm = serviceForm;
                  return PartialView(@"/Views/Services/DetailsServiceFormFromService.cshtml", serviceForm);
              }
              return PartialView();
          }

          public PartialViewResult RemovePreWeddingService()
          {
              return PartialView(); 
          }


        /***************************Create Service Form Section*******************************************************/
        #endregion

          #region Edit Service Form Section
          public ActionResult CreateServiceFormFromServiceWhenEdit(ServiceFormViewModel serviceForm,string Command)
          {
              //assign value for replacing #id in view
              ServiceForm _serviceForm = new ServiceForm();
              if(serviceForm !=null)
              { 
                  _serviceForm = new ServiceForm 
                  {
                    Id = serviceForm.Id,
                    Name = serviceForm.Name,
                    EventStart = serviceForm.EventStart,
                    EventEnd = serviceForm.EventEnd,
                    GuestsNumber = serviceForm.GuestsNumber
                  };
              }
              ViewBag.FormContorl = Command + " form-control";
              ViewBag.ServiceTypeItem = Command;
              ViewBag.ServiceTypeTag = Command + HTMLTagForReplace;
              ViewBag.ServiceTypeDatePickerStart = Command + HTMLTagForDatePickerStart;
              ViewBag.ServiceTypeDatePickerEnd = Command + HTMLTagForDatePickerEnd;
              ViewBag.HTMLTagButtonPhotoGraph = HTMLTagButtonPhotoGraph + Command;
              ViewBag.HTMLTagButtonEquipment = HTMLTagButtonEquipment + Command;
              ViewBag.HTMLTagButtonLocation = HTMLTagButtonLocation + Command;
              ViewBag.HTMLTagButtonOutSource = HTMLTagButtonOutSource + Command;
              ViewBag.HTMLTagButtonOutput = HTMLTagButtonOutput + Command;
              ViewBag.HTMLContainerButtonPhotoGraph = HTMLContainerButtonPhotoGraph + Command;
              ViewBag.HTMLContainerButtonEquipment = HTMLContainerButtonEquipment + Command;
              ViewBag.HTMLContainerButtonLocation = HTMLContainerButtonLocation + Command;
              ViewBag.HTMLContainerButtonOutSource = HTMLContainerButtonOutSource + Command;
              ViewBag.HTMLContainerButtonOutput = HTMLContainerButtonOutput + Command;
              ViewBag.HTMLModalPhotoGraph = HTMLModalPhotoGraph + Command;
              ViewBag.HTMLModalEquipment = HTMLModalEquipment + Command;
              ViewBag.HTMLModalLocation = HTMLModalLocation + Command;
              ViewBag.HTMLModalOutSource = HTMLModalOutSource + Command;
              ViewBag.HTMLModalOutput = HTMLModalOutput + Command;
              ViewBag.HTMLModalPhotoGraphArrow = HTMLModalPhotoGraphArrow + Command;
              ViewBag.HTMLModalEquipmentArrow = HTMLModalEquipmentArrow + Command;
              ViewBag.HTMLModalLocationArrow = HTMLModalLocationArrow + Command;
              ViewBag.HTMLModalOutSourceArrow = HTMLModalOutSourceArrow + Command;
              ViewBag.HTMLModalOutputArrow = HTMLModalOutputArrow + Command;
              ViewBag.HTMLButtonEnabledForm = HTMLButtonEnabledForm + Command;
              ViewBag.HTMLButtonDisabledForm = HTMLButtonDisabledForm + Command;
              return PartialView(_serviceForm);
          }

          [HttpPost]
          [ValidateAntiForgeryToken]
          public void CreateServiceFormFromServiceWhenEdit([Bind(Include = "Name,Status,EventStart,EventEnd,GuestsNumber")] ServiceForm serviceForm, string Command)
          {
              if (ModelState.IsValid)
              {
                  ServiceType serviceType = db.ServiceTypes.Where(s => string.Compare(s.ServiceTypeName, Command, true) == 0).FirstOrDefault();
                  if (serviceType != null)
                  {
                      serviceForm.ServiceType = serviceType;
                      //create string for mapping
                      string _mappingServiceType = serviceType.ServiceTypeName + HTMLTagForReplace;
                      ServiceFormFactory serviceFactory = CreateServiceFormByInputSectionWhenEdit(_mappingServiceType);
                      if (serviceFactory.ServiceForm != null)
                      {
                          serviceFactory.UpdateServiceForm(serviceForm);
                      }
                      else
                      {
                          serviceFactory.ServiceForm = new ServiceFormViewModel();
                          serviceFactory.UpdateServiceForm(serviceForm);
                      }
                  }                 
              }

          }

          [HttpGet]
          public async Task<PartialViewResult> CreatePhotoGraphServiceByModalWhenEdit(int? id, string serviceType = "")
          {
              ServiceFormFactory serviceFactory = CreateServiceFormByInputSectionWhenEdit(serviceType);
              PhotographService photoGraphService;
              ViewData["PhotoGraphList"] = new SelectList(db.PhotographServices, "Id", "Name");
              if (id != null)
              { 
                  photoGraphService = await db.PhotographServices.FindAsync(id); 
              }
              else
              {
                  if (serviceFactory.PhotoGraphService != null)
                  { photoGraphService = await db.PhotographServices.FindAsync(serviceFactory.PhotoGraphService.PhotoGraphServiceId); }
                  else
                  { photoGraphService = await db.PhotographServices.FirstAsync(); }
              }
              ViewData["Code"] = photoGraphService.Id;


              /*Need ************ Getting date from Form Section */
              DateTime _startDate = DateTime.Now;
              DateTime _endDate = DateTime.Now;
              /*Need ************ Getting date from Form Section */
              
              List<string> _selectedPhotoGraph;
              List<string> _selectedCameraMan;
              if (serviceFactory.PhotoGraphService != null)
              {
                  _selectedPhotoGraph = new List<string>(serviceFactory.PhotoGraphService.PhotoGraphIdList);
                  _selectedCameraMan = new List<string>(serviceFactory.PhotoGraphService.CameraMandIdList);
              }
              else
              {
                  _selectedPhotoGraph = new List<string>();
                  _selectedCameraMan = new List<string>();
              }

              if (serviceFactory.ServiceForm != null)
              {
                  _startDate = serviceFactory.ServiceForm.EventStart;
                  _endDate = serviceFactory.ServiceForm.EventEnd;
                  List<PhotoGraph> existedPhotograph = new List<PhotoGraph>();
                  List<CameraMan> existedCameraMan = new List<CameraMan>();
                  foreach (var pg in _selectedPhotoGraph)
                  {
                      Employee tempEmp = db.Employees.Find(pg);
                      PhotoGraph photograph = new PhotoGraph
                      {
                          Id = tempEmp.Id,
                          Name = tempEmp.Name,
                          IsSelect = true
                      };
                      existedPhotograph.Add(photograph);
                  }

                  foreach (var cm in _selectedCameraMan)
                  {
                      Employee tempEmp = db.Employees.Find(cm);
                      CameraMan cameraman = new CameraMan
                      {
                          Id = tempEmp.Id,
                          Name = tempEmp.Name,
                          IsSelect = true
                      };
                      existedCameraMan.Add(cameraman);
                  }
                 
                  var photoGraphResult = db.Employees.GroupBy(emp => emp.Id)
                                      .Where(emp => emp.Any(empList => empList.Position == PhotographType
                                          && empList.EmployeeSchedules.All(
                                                empS => 
                                                 ((empS.StartTime < _startDate || empS.StartTime > _endDate)
                                                    && (empS.EndTime <= _startDate || empS.EndTime > _endDate))
                                                  || ((empS.StartTime >= _startDate) || (empS.EndTime <=_endDate))
                                              )
                                          )).Select(emp => new PhotoGraph
                                          {
                                              Id = emp.FirstOrDefault().Id,
                                              Name = emp.FirstOrDefault().Name,
                                              IsSelect = _selectedPhotoGraph.Contains(emp.FirstOrDefault().Id)
                                          }).ToList();
                  //ViewBag.PhotoGraphListDetails = new SelectList(photoGraphResult, "Id", "Name");
                  if (existedPhotograph.Count > 0)
                  {
                      ViewBag.PhotoGraphListDetails = existedPhotograph.Union(photoGraphResult).GroupBy(emp=>emp.Id).Select(emp => emp.FirstOrDefault());
                  }
                  else 
                  {
                      ViewBag.PhotoGraphListDetails = photoGraphResult;
                  }
                 

                  //Getting CameraMan
                  var cameraManResult = db.Employees.GroupBy(emp => emp.Id)
                                          .Where(emp => emp.Any(empList => empList.Position == CameraManType
                                              && empList.EmployeeSchedules.All(
                                                 empS => 
                                                 ((empS.StartTime < _startDate || empS.StartTime > _endDate)
                                                    && (empS.EndTime <= _startDate || empS.EndTime > _endDate))
                                                  || ((empS.StartTime >= _startDate) || (empS.EndTime <=_endDate))
                                                  )
                                              )).Select(emp => new CameraMan
                                              {
                                                  Id = emp.FirstOrDefault().Id,
                                                  Name = emp.FirstOrDefault().Name,
                                                  IsSelect = _selectedCameraMan.Contains(emp.FirstOrDefault().Id)
                                              }).ToList();
                  if (existedCameraMan.Count > 0)
                  {
                      ViewBag.CameraManListDetails = existedCameraMan.Union(cameraManResult).GroupBy(emp=>emp.Id).Select(emp=>emp.FirstOrDefault());
                  }
                  else 
                  {
                      ViewBag.CameraManListDetails = cameraManResult;
                  }
                
              }
              else
              {
                  //if user did not select event day , so no need to generate data
                  ViewBag.CameraManListDetails = new List<PhotoGraph>();
                  ViewBag.PhotoGraphListDetails = new List<CameraMan>();
              }

              //Create metadata for webpage structure
              if (string.Compare(serviceType, string.Concat(PreWedding, HTMLTagForReplace)) == 0)
              {
                  ViewData["ButtonIdForDWL"] = HTMLDWLPhotoGraphPreWeddingServices;
                  ViewData["DivServiceForm"] = HTMLPhotoServiceModalPreWedding;
                  ViewData["CollPhotoPanel"] = HTMLCollapsePhotoPreWedding;
                  ViewData["CollCameraPanel"] = HTMLCollapseCameraPreWedding;
                  ViewData["ModalWindowId"] = HTMLTagButtonPhotoGraph + PreWedding;
              }
              else if (string.Compare(serviceType, string.Concat(Engagement, HTMLTagForReplace)) == 0)
              {
                  ViewData["ButtonIdForDWL"] = HTMLDWLPhotoGraphEngagementServices;
                  ViewData["DivServiceForm"] = HTMLPhotoServiceModalEngagement;
                  ViewData["CollPhotoPanel"] = HTMLCollapsePhotoEngagement;
                  ViewData["CollCameraPanel"] = HTMLCollapseCameraEngagement;
                  ViewData["ModalWindowId"] = HTMLTagButtonPhotoGraph + Engagement;
              }
              else if (string.Compare(serviceType, string.Concat(Wedding, HTMLTagForReplace)) == 0)
              {
                  ViewData["ButtonIdForDWL"] = HTMLDWLPhotoGraphWeddingServices;
                  ViewData["DivServiceForm"] = HTMLPhotoServiceModalWedding;
                  ViewData["CollPhotoPanel"] = HTMLCollapsePhotoWedding;
                  ViewData["CollCameraPanel"] = HTMLCollapseCameraWedding;
                  ViewData["ModalWindowId"] = HTMLTagButtonPhotoGraph + Wedding;
              }
              ViewData["ServiceType"] = serviceType;

              return PartialView(photoGraphService);
          }

          [HttpPost]
          public void CreatePhotoGraphServiceListWhenEdit([Bind(Include = "Name,PhotographerNumber,CameraManNumber,Description,Cost,Price")]PhotographService photoGraphService, string[] EmployeeId, string[] CameraId, string ServiceType, int Code)
          {
              PhotographService photo = photoGraphService;
              List<string> empList = new List<string>();
              List<string> camList = new List<string>();
              ServiceFormFactory serviceFactory = CreateServiceFormByInputSectionWhenEdit(ServiceType);
              if (EmployeeId == null && CameraId == null)
              {
                  serviceFactory.CreatePhotoGraphService(photo, empList, camList, Code);
              }
              else if (EmployeeId != null && CameraId != null)
              {
                  serviceFactory.CreatePhotoGraphService(photo, EmployeeId.ToList(), CameraId.ToList(), Code);
              }
              else if (EmployeeId != null)
              {
                  serviceFactory.CreatePhotoGraphService(photo, EmployeeId.ToList(), camList, Code);
              }
              else
              {
                  serviceFactory.CreatePhotoGraphService(photo, empList, CameraId.ToList(), Code);
              }

          }

          //[HttpPost]
          //public async Task<PartialViewResult> CreatePhotoGraphServiceByModal(int? id)
          //{
          //    ViewData["PhotoGraphList"] = new SelectList(db.PhotographServices, "Id", "Name", "Description");
          //    PhotographService photoGraphService = await db.PhotographServices.FindAsync(id);
          //    ViewData["Code"] = photoGraphService.Id;
          //    return PartialView(photoGraphService);
          //}

          [HttpGet]
          public async Task<PartialViewResult> CreateEquipmentServiceByModalWhenEdit(int? id, string serviceType = "")
          {
              ServiceFormFactory serviceFactory = CreateServiceFormByInputSectionWhenEdit(serviceType);
              EquipmentService equipmentService;
              ViewData["EquipmentList"] = new SelectList(db.EquipmentServices, "Id", "Name");
              if (id != null)
              { equipmentService = await db.EquipmentServices.FindAsync(id); }
              else {equipmentService = await db.EquipmentServices.FirstAsync(); }
              ViewData["Code"] = equipmentService.Id;
             
              //Create Equipment Service
              if (serviceFactory.ListEquipmentServices.Count > 0)
              {
                  ViewBag.ListEquipmentItems = new List<EquipmentServiceViewModel>(serviceFactory.ListEquipmentServices);
              }
              else
              {
                  ViewBag.ListEquipmentItems = null;
              }

              //Create metadata for webpage structure
              if (string.Compare(serviceType, string.Concat(PreWedding, HTMLTagForReplace)) == 0)
              {
                  ViewData["ButtonIdForDWL"] = HTMLDWLEquipmentPreWeddingServices;
                  ViewData["DivServiceForm"] = HTMLEquipmentModalPreWedding;
                  ViewData["TblEquipment"] = HTMLTableEquipmentPreWedding;
              }
              else if (string.Compare(serviceType, string.Concat(Engagement, HTMLTagForReplace)) == 0)
              {
                  ViewData["ButtonIdForDWL"] = HTMLDWLEquipmentEngagementServices;
                  ViewData["DivServiceForm"] = HTMLEquipmentModalEngagement;
                  ViewData["TblEquipment"] = HTMLTableEquipmentEngagement;
              }
              else if (string.Compare(serviceType, string.Concat(Wedding, HTMLTagForReplace)) == 0)
              {
                  ViewData["ButtonIdForDWL"] = HTMLDWLEquipmentWeddingServices;
                  ViewData["DivServiceForm"] = HTMLEquipmentModalWedding;
                  ViewData["TblEquipment"] = HTMLTableEquipmentWedding;
              }
              ViewData["ServiceType"] = serviceType;
              return PartialView(equipmentService);
          }

          [HttpPost]
          public async Task<PartialViewResult> CreateEquipmentServiceTableWhenEdit([Bind(Include = "Name,Price,Cost,Description")]EquipmentService equipmentService, int? EquipmentId, string ServiceType, int Code)
          {
              EquipmentService _equipment = equipmentService;
              int _equipmentId = int.Parse(EquipmentId.ToString());
              ServiceFormFactory serviceFactory = CreateServiceFormByInputSectionWhenEdit(ServiceType);
              serviceFactory.CreateEquipmentServiceList(equipmentService, _equipmentId, Code);
              //Create Equipment Service
              if (serviceFactory.ListEquipmentServices.Count > 0)
              {
                  ViewBag.ListEquipmentItems = new List<EquipmentServiceViewModel>(serviceFactory.ListEquipmentServices);
                  //Add to database
              }
              else
              {
                  ViewBag.ListEquipmentItems = null;
              }
              ViewBag.EquipmentServiceType = ServiceType;
              ViewData["TblEquipment"] = ServiceTypeForTable(ServiceType);
              return PartialView();
          }

          public PartialViewResult RemoveEquipmentWhenEdit(int? Id, string serviceType)
          {
              ServiceFormFactory serviceFactory = CreateServiceFormByInputSectionWhenEdit(serviceType);
              if (serviceFactory != null)
              {
                  var deleteTarget = serviceFactory.ListEquipmentServices.Select(element => element.Id == Id);
                  serviceFactory.ListEquipmentServices.RemoveAll(item => item.Id == Id);
              }
              ViewBag.ModifyEquipment = serviceFactory.ListEquipmentServices;
              ViewData["TblEquipment"] = ServiceTypeForTable(serviceType);
              ViewBag.EquipmentServiceType = serviceType;
              return PartialView();
          }

          [HttpGet]
          public async Task<PartialViewResult> CreateLocationServiceByModalWhenEdit(int? id, string serviceType = "")
          {
              ServiceFormFactory serviceFactory = CreateServiceFormByInputSectionWhenEdit(serviceType);
              LocationService locationService;
              ViewData["LocationList"] = new SelectList(db.LocationServices, "Id", "Name");
              if (id != null)
              { locationService = await db.LocationServices.FindAsync(id); }
              else
              { locationService = await db.LocationServices.FirstAsync(); }
              ViewData["Code"] = locationService.Id;

              
              //Create Equipment Service
              if (serviceFactory.ListLocationServices.Count > 0)
              {
                  ViewBag.ListLocationServices = new List<LocationServiceViewModel>(serviceFactory.ListLocationServices);
              }
              else
              {
                  ViewBag.ListLocationServices = null;
              }

              //Create metadata for webpage structure
              if (string.Compare(serviceType, string.Concat(PreWedding, HTMLTagForReplace)) == 0)
              {
                  ViewData["ButtonIdForDWL"] = HTMLDWLLocationPreWeddingServices;
                  ViewData["DivServiceForm"] = HTMLLocationModalPreWedding;
                  ViewData["TblEquipment"] = HTMLTableLocationPreWedding;
              }
              else if (string.Compare(serviceType, string.Concat(Engagement, HTMLTagForReplace)) == 0)
              {
                  ViewData["ButtonIdForDWL"] = HTMLDWLLocationEngagementServices;
                  ViewData["DivServiceForm"] = HTMLLocationModalEngagement;
                  ViewData["TblEquipment"] = HTMLTableLocationEngagement;
              }
              else if (string.Compare(serviceType, string.Concat(Wedding, HTMLTagForReplace)) == 0)
              {
                  ViewData["ButtonIdForDWL"] = HTMLDWLLocationWeddingServices;
                  ViewData["DivServiceForm"] = HTMLLocationModalWedding;
                  ViewData["TblEquipment"] = HTMLTableLocationWedding;
              }
              ViewData["ServiceType"] = serviceType;
              return PartialView(locationService);
          }

          [HttpPost]
          public async Task<PartialViewResult> CreateLocationServiceTableWhenEdit([Bind(Include = "Name,Price,Cost,IsOverNight,OverNightPeriod,Description")]LocationService locationService, int? LocationId, string ServiceType, int Code)
          {
              LocationService _locationService = locationService;
              int _locationId = int.Parse(LocationId.ToString());
              ServiceFormFactory serviceFactory = CreateServiceFormByInputSectionWhenEdit(ServiceType);
              serviceFactory.CreateLocationServiceList(locationService, _locationId, Code);
              //Create Equipment Service
              if (serviceFactory.ListLocationServices.Count > 0)
              {
                  ViewBag.ListLocationServices = new List<LocationServiceViewModel>(serviceFactory.ListLocationServices);
                  //Add to database
              }
              else
              {
                  ViewBag.ListLocationServices = null;
              }
              ViewData["TblEquipment"] = ServiceTypeForTable(ServiceType);
              ViewBag.LocationServiceType = ServiceType;
              return PartialView();
          }

          public PartialViewResult RemoveLocationWhenEdit(int? Id, string serviceType)
          {
              ServiceFormFactory serviceFactory = CreateServiceFormByInputSectionWhenEdit(serviceType);
              if (serviceFactory != null)
              {
                  var deleteTarget = serviceFactory.ListLocationServices.Select(element => element.Id == Id);
                  serviceFactory.ListLocationServices.RemoveAll(item => item.Id == Id);
              }
              ViewBag.ModifyLocation = serviceFactory.ListLocationServices;
              ViewData["TblEquipment"] = ServiceTypeForTable(serviceType);
              ViewBag.LocationServiceType = serviceType;
              return PartialView();
          }

          [HttpGet]
          public async Task<PartialViewResult> CreateOutsourceServiceByModalWhenEdit(int? id, string serviceType = "")
          {
              OutsourceService outsourceService;
              ViewData["OutsourceList"] = new SelectList(db.OutsourceServices, "Id", "Name");
              if (id != null)
              { outsourceService = await db.OutsourceServices.FindAsync(id); }
              else
              { outsourceService = await db.OutsourceServices.FirstAsync(); }
              ViewData["Code"] = outsourceService.Id;

              ServiceFormFactory serviceFactory = CreateServiceFormByInputSectionWhenEdit(serviceType);
              //Create Equipment Service
              if (serviceFactory.ListOutsourceServices.Count > 0)
              {
                  ViewBag.ListOutsourceServices = new List<OutsourceServiceViewModel>(serviceFactory.ListOutsourceServices);
              }
              else
              {
                  ViewBag.ListOutsourceServices = null;
              }

              //Create metadata for webpage structure
              if (string.Compare(serviceType, string.Concat(PreWedding, HTMLTagForReplace)) == 0)
              {
                  ViewData["ButtonIdForDWL"] = HTMLDWLOutsourcePreWeddingServices;
                  ViewData["DivServiceForm"] = HTMLOutsourceModalPreWedding;
                  ViewData["TblEquipment"] = HTMLTableOutsourcePreWedding;
              }
              else if (string.Compare(serviceType, string.Concat(Engagement, HTMLTagForReplace)) == 0)
              {
                  ViewData["ButtonIdForDWL"] = HTMLDWLOutsourceEngagementServices;
                  ViewData["DivServiceForm"] = HTMLOutsourceModalEngagement;
                  ViewData["TblEquipment"] = HTMLTableOutsourceEngagement;
              }
              else if (string.Compare(serviceType, string.Concat(Wedding, HTMLTagForReplace)) == 0)
              {
                  ViewData["ButtonIdForDWL"] = HTMLDWLOutsourceWeddingServices;
                  ViewData["DivServiceForm"] = HTMLOutsourceModalWedding;
                  ViewData["TblEquipment"] = HTMLTableOutsourceWedding;
              }
              ViewData["ServiceType"] = serviceType;
              return PartialView(outsourceService);
          }

          [HttpPost]
          public async Task<PartialViewResult> CreateOutsourceServiceTableWhenEdit([Bind(Include = "Name,PortFolioURL,Price,Cost,Description")]OutsourceService outsourceService, int? OutsourceId, string ServiceType, int Code)
          {
              OutsourceService _outsourceService = outsourceService;
              int _outsourceId = int.Parse(OutsourceId.ToString());
              ServiceFormFactory serviceFactory = CreateServiceFormByInputSectionWhenEdit(ServiceType);
              serviceFactory.CreateOutSoruceServiceList(outsourceService, _outsourceId, Code);
              //Create Equipment Service
              if (serviceFactory.ListOutsourceServices.Count > 0)
              {
                  ViewBag.ListOutsourceServices = new List<OutsourceServiceViewModel>(serviceFactory.ListOutsourceServices);
                  //Add to database
              }
              else
              {
                  ViewBag.ListOutsourceServices = null;
              }
              ViewData["TblEquipment"] = ServiceTypeForTable(ServiceType);
              ViewBag.OutsourceServiceType = ServiceType;
              return PartialView();
          }

          public PartialViewResult RemoveOutsourceWhenEdit(int? Id, string serviceType)
          {
              ServiceFormFactory serviceFactory = CreateServiceFormByInputSectionWhenEdit(serviceType);
              if (serviceFactory != null)
              {
                  var deleteTarget = serviceFactory.ListOutsourceServices.Select(element => element.Id == Id);
                  serviceFactory.ListOutsourceServices.RemoveAll(item => item.Id == Id);
              }
              ViewBag.ModifyOutsource = serviceFactory.ListOutsourceServices;
              ViewData["TblEquipment"] = ServiceTypeForTable(serviceType);
              ViewBag.OutsourceServiceType = serviceType;
              return PartialView();
          }

          [HttpGet]
          public async Task<PartialViewResult> CreateOutputServiceByModalWhenEdit(int? id, string serviceType = "")
          {
              OutputService outputService;
              ViewData["OutputList"] = new SelectList(db.OutputServices, "Id", "Name");
              if (id != null)
              { outputService = await db.OutputServices.FindAsync(id); }
              else
              { outputService = await db.OutputServices.FirstAsync(); }
              ViewData["Code"] = outputService.Id;

              ServiceFormFactory serviceFactory = CreateServiceFormByInputSectionWhenEdit(serviceType);
              //Create Equipment Service
              if (serviceFactory.ListOutputServices.Count > 0)
              {
                  ViewBag.ListOutputServices = new List<OutputServiceViewModel>(serviceFactory.ListOutputServices);
              }
              else
              {
                  ViewBag.ListOutputServices = null;
              }

              //Create metadata for webpage structure
              if (string.Compare(serviceType, string.Concat(PreWedding, HTMLTagForReplace)) == 0)
              {
                  ViewData["ButtonIdForDWL"] = HTMLDWLOutputPreWeddingServices;
                  ViewData["DivServiceForm"] = HTMLOutputModalPreWedding;
                  ViewData["TblEquipment"] = HTMLTableOutputPreWedding;
              }
              else if (string.Compare(serviceType, string.Concat(Engagement, HTMLTagForReplace)) == 0)
              {
                  ViewData["ButtonIdForDWL"] = HTMLDWLOutputEngagementServices;
                  ViewData["DivServiceForm"] = HTMLOutputModalEngagement;
                  ViewData["TblEquipment"] = HTMLTableOutputEngagement;
              }
              else if (string.Compare(serviceType, string.Concat(Wedding, HTMLTagForReplace)) == 0)
              {
                  ViewData["ButtonIdForDWL"] = HTMLDWLOutputWeddingServices;
                  ViewData["DivServiceForm"] = HTMLOutputModalWedding;
                  ViewData["TblEquipment"] = HTMLTableOutputWedding;
              }
              ViewData["ServiceType"] = serviceType;
              return PartialView(outputService);
          }

          [HttpPost]
          public async Task<PartialViewResult> CreateOutputServiceTableWhenEdit([Bind(Include = "Name,PortFolioURL,Price,Cost,Description")]OutputService outputService, string ServiceType, int Code)
          {
              OutputService _outputService = outputService;
              ServiceFormFactory serviceFactory = CreateServiceFormByInputSectionWhenEdit(ServiceType);
              serviceFactory.CreateOutputServiceList(outputService, Code);
              //Create Equipment Service
              if (serviceFactory.ListOutputServices.Count > 0)
              {
                  ViewBag.ListOutputServices = new List<OutputServiceViewModel>(serviceFactory.ListOutputServices);
                  //Add to database
              }
              else
              {
                  ViewBag.ListOutputServices = null;
              }
              ViewData["TblEquipment"] = ServiceTypeForTable(ServiceType);
              ViewBag.OutputServiceType = ServiceType;
              return PartialView();
          }

          public PartialViewResult RemoveOutputWhenEdit(int? Id, string serviceType)
          {
              ServiceFormFactory serviceFactory = CreateServiceFormByInputSectionWhenEdit(serviceType);
              if (serviceFactory != null)
              {
                  var deleteTarget = serviceFactory.ListOutputServices.Select(element => element.Id == Id);
                  serviceFactory.ListOutputServices.RemoveAll(item => item.Id == Id);
              }
              ViewBag.ModifyOutput = serviceFactory.ListOutputServices;
              ViewData["TblEquipment"] = ServiceTypeForTable(serviceType);
              ViewBag.OutputServiceType = serviceType;
              return PartialView();
          }

          #endregion

          #region CreateModal Window section

          [HttpGet]  
        public async Task<PartialViewResult> CreatePhotoGraphServiceByModal(int? id, string serviceType="")
          {
            PhotographService photoGraphService;
            ViewData["PhotoGraphList"] =  new SelectList(db.PhotographServices, "Id", "Name");
            if (id != null)
            { photoGraphService = await db.PhotographServices.FindAsync(id); }
            else
            { photoGraphService = await db.PhotographServices.FirstAsync(); }
            ViewData["Code"] = photoGraphService.Id;

            //Getting PhotoGraph
            ServiceForm PreWeddingFromSection;

            /*Need ************ Getting date from Form Section */
            DateTime _startDate = DateTime.Now;
            DateTime _endDate = DateTime.Now;
            /*Need ************ Getting date from Form Section */
            ServiceFormFactory serviceFactory = CreateServiceFormByInputSection(serviceType);
            List<string> _selectedPhotoGraph;
            List<string> _selectedCameraMan;
            if (serviceFactory.PhotoGraphService != null)
            {
                _selectedPhotoGraph = new List<string>(serviceFactory.PhotoGraphService.PhotoGraphIdList);
                _selectedCameraMan = new List<string>(serviceFactory.PhotoGraphService.CameraMandIdList);
            }
            else
            {
                _selectedPhotoGraph = new List<string>();
                _selectedCameraMan = new List<string>();
            }

            if (serviceFactory.ServiceForm != null)
            {
                _startDate = serviceFactory.ServiceForm.EventStart;
                _endDate = serviceFactory.ServiceForm.EventEnd;
                var photoGraphResult = db.Employees.GroupBy(emp => emp.Id)
                                    .Where(emp => emp.Any(empList => empList.Position == PhotographType
                                        && empList.EmployeeSchedules.All(empS => (empS.StartTime < _startDate || empS.StartTime > _endDate)
                                            && (empS.EndTime <= _startDate || empS.EndTime > _endDate))
                                        )).Select(emp => new PhotoGraph
                                        {
                                            Id = emp.FirstOrDefault().Id,
                                            Name = emp.FirstOrDefault().Name,
                                            IsSelect = _selectedPhotoGraph.Contains(emp.FirstOrDefault().Id)
                                        }).ToList();
                //ViewBag.PhotoGraphListDetails = new SelectList(photoGraphResult, "Id", "Name");
                ViewBag.PhotoGraphListDetails = photoGraphResult;

                //Getting CameraMan
                var cameraManResult = db.Employees.GroupBy(emp => emp.Id)
                                        .Where(emp => emp.Any(empList => empList.Position == CameraManType
                                            && empList.EmployeeSchedules.All(empS => (empS.StartTime < _startDate || empS.StartTime > _endDate)
                                                && (empS.EndTime <= _startDate || empS.EndTime > _endDate))
                                            )).Select(emp => new CameraMan
                                            {
                                                Id = emp.FirstOrDefault().Id,
                                                Name = emp.FirstOrDefault().Name,
                                                IsSelect = _selectedCameraMan.Contains(emp.FirstOrDefault().Id)
                                            }).ToList();
                ViewBag.CameraManListDetails = cameraManResult;
            }
            else
            { 
                //if user did not select event day , so no need to generate data
                ViewBag.CameraManListDetails = new List<PhotoGraph>();
                ViewBag.PhotoGraphListDetails = new List<CameraMan>();
            }

            
            //var _camearManResult = (from cameraEmp in db.Employees
            //                       join empSchedule in db.EmployeeSchedules on cameraEmp.Id equals empSchedule.Employee.Id
            //                       where cameraEmp.Position == CameraManType && empSchedule.StartTime >= _startDate && empSchedule.EndTime <= _endDate
            //                       select cameraEmp).ToList();

            //Create metadata for webpage structure
            if (string.Compare(serviceType, string.Concat(PreWedding, HTMLTagForReplace)) == 0)
            {
                ViewData["ButtonIdForDWL"] = HTMLDWLPhotoGraphPreWeddingServices;
                ViewData["DivServiceForm"] = HTMLPhotoServiceModalPreWedding;
                ViewData["CollPhotoPanel"] = HTMLCollapsePhotoPreWedding;
                ViewData["CollCameraPanel"] = HTMLCollapseCameraPreWedding;
                ViewData["ModalWindowId"] = HTMLTagButtonPhotoGraph + PreWedding;
            }
            else if (string.Compare(serviceType, string.Concat(Engagement, HTMLTagForReplace)) == 0)
            {
                ViewData["ButtonIdForDWL"] = HTMLDWLPhotoGraphEngagementServices;
                ViewData["DivServiceForm"] = HTMLPhotoServiceModalEngagement;
                ViewData["CollPhotoPanel"] = HTMLCollapsePhotoEngagement;
                ViewData["CollCameraPanel"] = HTMLCollapseCameraEngagement;
                ViewData["ModalWindowId"] = HTMLTagButtonPhotoGraph + Engagement;
            }
            else if (string.Compare(serviceType, string.Concat(Wedding, HTMLTagForReplace)) == 0)
            {
                ViewData["ButtonIdForDWL"] = HTMLDWLPhotoGraphWeddingServices;
                ViewData["DivServiceForm"] = HTMLPhotoServiceModalWedding;
                ViewData["CollPhotoPanel"] = HTMLCollapsePhotoWedding;
                ViewData["CollCameraPanel"] = HTMLCollapseCameraWedding;
                ViewData["ModalWindowId"] = HTMLTagButtonPhotoGraph + Wedding;
            }
            ViewData["ServiceType"] = serviceType;
           
            return PartialView(photoGraphService); 
          }

        [HttpPost]
        public void CreatePhotoGraphServiceList([Bind(Include = "Name,PhotographerNumber,CameraManNumber,Description,Cost,Price")]PhotographService photoGraphService, string[] EmployeeId, string[] CameraId, string ServiceType, int Code)
        {
            PhotographService photo = photoGraphService;
            List<string> empList = new List<string>();
            List<string> camList = new List<string>();
            ServiceFormFactory serviceFactory = CreateServiceFormByInputSection(ServiceType);
            if (EmployeeId == null && CameraId == null)
            {
                serviceFactory.CreatePhotoGraphService(photo, empList, camList,Code);
            }
            else if (EmployeeId != null && CameraId != null)
            {
                serviceFactory.CreatePhotoGraphService(photo, EmployeeId.ToList(), CameraId.ToList(),Code); 
            }
            else if (EmployeeId != null)
            {
                serviceFactory.CreatePhotoGraphService(photo, EmployeeId.ToList(), camList,Code);
            }
            else
            {
                serviceFactory.CreatePhotoGraphService(photo, empList, CameraId.ToList(),Code);
            }
            
        }

        //[HttpPost]
        //public async Task<PartialViewResult> CreatePhotoGraphServiceByModal(int? id)
        //{
        //    ViewData["PhotoGraphList"] = new SelectList(db.PhotographServices, "Id", "Name", "Description");
        //    PhotographService photoGraphService = await db.PhotographServices.FindAsync(id);
        //    ViewData["Code"] = photoGraphService.Id;
        //    return PartialView(photoGraphService);
        //}

        [HttpGet]
        public async Task<PartialViewResult> CreateEquipmentServiceByModal(int? id, string serviceType = "")
        {
            EquipmentService equipmentService;
            ViewData["EquipmentList"] = new SelectList(db.EquipmentServices, "Id", "Name");
            if (id != null)
            { equipmentService = await db.EquipmentServices.FindAsync(id); }
            else
            { equipmentService = await db.EquipmentServices.FirstAsync(); }
            ViewData["Code"] = equipmentService.Id;
            ServiceFormFactory serviceFactory = CreateServiceFormByInputSection(serviceType);
            //Create Equipment Service
                 if (serviceFactory.ListEquipmentServices.Count > 0)
                {
                    ViewBag.ListEquipmentItems = new List<EquipmentServiceViewModel>(serviceFactory.ListEquipmentServices);
                }
                else
                {
                    ViewBag.ListEquipmentItems = null;
                }
           

            //Create metadata for webpage structure
            if (string.Compare(serviceType, string.Concat(PreWedding, HTMLTagForReplace)) == 0)
            {
                ViewData["ButtonIdForDWL"] = HTMLDWLEquipmentPreWeddingServices;
                ViewData["DivServiceForm"] = HTMLEquipmentModalPreWedding;
                ViewData["TblEquipment"] = HTMLTableEquipmentPreWedding;
            }
            else if (string.Compare(serviceType, string.Concat(Engagement, HTMLTagForReplace)) == 0)
            {
                ViewData["ButtonIdForDWL"] = HTMLDWLEquipmentEngagementServices;
                ViewData["DivServiceForm"] = HTMLEquipmentModalEngagement;
                ViewData["TblEquipment"] = HTMLTableEquipmentEngagement;
            }
            else if (string.Compare(serviceType, string.Concat(Wedding, HTMLTagForReplace)) == 0)
            {
                ViewData["ButtonIdForDWL"] = HTMLDWLEquipmentWeddingServices;
                ViewData["DivServiceForm"] = HTMLEquipmentModalWedding;
                ViewData["TblEquipment"] = HTMLTableEquipmentWedding;
            }
            ViewData["ServiceType"] = serviceType;
            return PartialView(equipmentService);
        }

        [HttpPost]
        public async Task<PartialViewResult> CreateEquipmentServiceTable([Bind(Include="Name,Price,Cost,Description")]EquipmentService equipmentService, int? EquipmentId, string ServiceType,int Code)
        {
            EquipmentService _equipment = equipmentService;
            int _equipmentId = int.Parse(EquipmentId.ToString());
            ServiceFormFactory serviceFactory = CreateServiceFormByInputSection(ServiceType);
            serviceFactory.CreateEquipmentServiceList(equipmentService, _equipmentId,Code);
            //Create Equipment Service
            if (serviceFactory.ListEquipmentServices.Count > 0)
            {
                ViewBag.ListEquipmentItems = new List<EquipmentServiceViewModel>(serviceFactory.ListEquipmentServices);
                //Add to database
            }
            else
            {
                ViewBag.ListEquipmentItems = null;
            }
            ViewBag.EquipmentServiceType = ServiceType;
            ViewData["TblEquipment"] = ServiceTypeForTable(ServiceType);
            
            return PartialView();
        }

        private string ServiceTypeForTable(string serviceType)
        {
            string viewDataTableEquipment = "";
            if (string.Compare(serviceType, string.Concat(PreWedding, HTMLTagForReplace)) == 0)
            {
                viewDataTableEquipment = HTMLTableEquipmentPreWedding;
            }
            else if (string.Compare(serviceType, string.Concat(Engagement, HTMLTagForReplace)) == 0)
            {
                viewDataTableEquipment = HTMLTableEquipmentEngagement;
            }
            else if (string.Compare(serviceType, string.Concat(Wedding, HTMLTagForReplace)) == 0)
            {
                viewDataTableEquipment = HTMLTableEquipmentWedding;
            }
            return viewDataTableEquipment;
        }

        public PartialViewResult RemoveEquipment(int? Id, string serviceType)
        {
            ServiceFormFactory serviceFactory = CreateServiceFormByInputSection(serviceType);
            if (serviceFactory != null)
            {
                var deleteTarget = serviceFactory.ListEquipmentServices.Select(element => element.Id == Id);
                serviceFactory.ListEquipmentServices.RemoveAll(item => item.Id == Id);
            }
            ViewBag.ModifyEquipment = serviceFactory.ListEquipmentServices;
            ViewData["TblEquipment"] = ServiceTypeForTable(serviceType);
            return PartialView();
        }

        [HttpGet]
        public async Task<PartialViewResult> CreateLocationServiceByModal(int? id, string serviceType = "")
        {
            LocationService locationService;
            ViewData["LocationList"] = new SelectList(db.LocationServices, "Id", "Name");
            if (id != null)
            { locationService = await db.LocationServices.FindAsync(id); }
            else
            { locationService = await db.LocationServices.FirstAsync(); }
            ViewData["Code"] = locationService.Id;

            ServiceFormFactory serviceFactory = CreateServiceFormByInputSection(serviceType);
            //Create Equipment Service
            if (serviceFactory.ListLocationServices.Count > 0)
            {
                ViewBag.ListLocationServices = new List<LocationServiceViewModel>(serviceFactory.ListLocationServices);
            }
            else
            {
                ViewBag.ListLocationServices = null;
            }

            //Create metadata for webpage structure
            if (string.Compare(serviceType, string.Concat(PreWedding, HTMLTagForReplace)) == 0)
            {
                ViewData["ButtonIdForDWL"] = HTMLDWLLocationPreWeddingServices;
                ViewData["DivServiceForm"] = HTMLLocationModalPreWedding;
                ViewData["TblEquipment"] = HTMLTableLocationPreWedding;
            }
            else if (string.Compare(serviceType, string.Concat(Engagement, HTMLTagForReplace)) == 0)
            {
                ViewData["ButtonIdForDWL"] = HTMLDWLLocationEngagementServices;
                ViewData["DivServiceForm"] = HTMLLocationModalEngagement;
                ViewData["TblEquipment"] = HTMLTableLocationEngagement;
            }
            else if (string.Compare(serviceType, string.Concat(Wedding, HTMLTagForReplace)) == 0)
            {
                ViewData["ButtonIdForDWL"] = HTMLDWLLocationWeddingServices;
                ViewData["DivServiceForm"] = HTMLLocationModalWedding;
                ViewData["TblEquipment"] = HTMLTableLocationWedding;
            }
            ViewData["ServiceType"] = serviceType;
            return PartialView(locationService);
        }

        [HttpPost]
        public async Task<PartialViewResult> CreateLocationServiceTable([Bind(Include = "Name,Price,Cost,IsOverNight,OverNightPeriod,Description")]LocationService locationService, int? LocationId, string ServiceType, int Code)
        {
            LocationService _locationService = locationService;
            int _locationId = int.Parse(LocationId.ToString());
            ServiceFormFactory serviceFactory = CreateServiceFormByInputSection(ServiceType);
            serviceFactory.CreateLocationServiceList(locationService, _locationId, Code);
            //Create Equipment Service
            if (serviceFactory.ListLocationServices.Count > 0)
            {
                ViewBag.ListLocationServices = new List<LocationServiceViewModel>(serviceFactory.ListLocationServices);
                //Add to database
            }
            else
            {
                ViewBag.ListLocationServices = null;
            }
            ViewBag.LocationServiceType = ServiceType;
            ViewData["TblEquipment"] = ServiceTypeForTable(ServiceType);
            return PartialView();
        }

        public PartialViewResult RemoveLocation(int? Id, string serviceType)
        {
            ServiceFormFactory serviceFactory = CreateServiceFormByInputSection(serviceType);
            if (serviceFactory != null)
            {
                var deleteTarget = serviceFactory.ListLocationServices.Select(element => element.Id == Id);
                serviceFactory.ListLocationServices.RemoveAll(item => item.Id == Id);
            }
            ViewBag.ModifyLocation = serviceFactory.ListLocationServices;
            ViewData["TblEquipment"] = ServiceTypeForTable(serviceType);
            ViewBag.LocationServiceType = serviceType;
            return PartialView();
        }

        [HttpGet]
        public async Task<PartialViewResult> CreateOutsourceServiceByModal(int? id, string serviceType = "")
        {
            OutsourceService outsourceService;
            ViewData["OutsourceList"] = new SelectList(db.OutsourceServices, "Id", "Name");
            if (id != null)
            { outsourceService = await db.OutsourceServices.FindAsync(id); }
            else
            { outsourceService = await db.OutsourceServices.FirstAsync(); }
            ViewData["Code"] = outsourceService.Id;

            ServiceFormFactory serviceFactory = CreateServiceFormByInputSection(serviceType);
            //Create Equipment Service
            if (serviceFactory.ListOutsourceServices.Count > 0)
            {
                ViewBag.ListOutsourceServices = new List<OutsourceServiceViewModel>(serviceFactory.ListOutsourceServices);
            }
            else
            {
                ViewBag.ListOutsourceServices = null;
            }

            //Create metadata for webpage structure
            if (string.Compare(serviceType, string.Concat(PreWedding, HTMLTagForReplace)) == 0)
            {
                ViewData["ButtonIdForDWL"] = HTMLDWLOutsourcePreWeddingServices;
                ViewData["DivServiceForm"] = HTMLOutsourceModalPreWedding;
                ViewData["TblEquipment"] = HTMLTableOutsourcePreWedding;
            }
            else if (string.Compare(serviceType, string.Concat(Engagement, HTMLTagForReplace)) == 0)
            {
                ViewData["ButtonIdForDWL"] = HTMLDWLOutsourceEngagementServices;
                ViewData["DivServiceForm"] = HTMLOutsourceModalEngagement;
                ViewData["TblEquipment"] = HTMLTableOutsourceEngagement;
            }
            else if (string.Compare(serviceType, string.Concat(Wedding, HTMLTagForReplace)) == 0)
            {
                ViewData["ButtonIdForDWL"] = HTMLDWLOutsourceWeddingServices;
                ViewData["DivServiceForm"] = HTMLOutsourceModalWedding;
                ViewData["TblEquipment"] = HTMLTableOutsourceWedding;
            }
            ViewData["ServiceType"] = serviceType;
            return PartialView(outsourceService);
        }

        [HttpPost]
        public async Task<PartialViewResult> CreateOutsourceServiceTable([Bind(Include = "Name,PortFolioURL,Price,Cost,Description")]OutsourceService outsourceService, int? OutsourceId, string ServiceType, int Code)
        {
            OutsourceService _outsourceService = outsourceService;
            int _outsourceId = int.Parse(OutsourceId.ToString());
            ServiceFormFactory serviceFactory = CreateServiceFormByInputSection(ServiceType);
            serviceFactory.CreateOutSoruceServiceList(outsourceService, _outsourceId,Code);
            //Create Equipment Service
            if (serviceFactory.ListOutsourceServices.Count > 0)
            {
                ViewBag.ListOutsourceServices = new List<OutsourceServiceViewModel>(serviceFactory.ListOutsourceServices);
                //Add to database
            }
            else
            {
                ViewBag.ListOutsourceServices = null;
            }
            ViewBag.OutsourceServiceType = ServiceType;
            ViewData["TblEquipment"] = ServiceTypeForTable(ServiceType);
            return PartialView();
        }

        public PartialViewResult RemoveOutsource(int? Id, string serviceType)
        {
            ServiceFormFactory serviceFactory = CreateServiceFormByInputSection(serviceType);
            if (serviceFactory != null)
            {
                var deleteTarget = serviceFactory.ListOutsourceServices.Select(element => element.Id == Id);
                serviceFactory.ListOutsourceServices.RemoveAll(item => item.Id == Id);
            }
            ViewBag.ModifyOutsource = serviceFactory.ListOutsourceServices;
            ViewData["TblEquipment"] = ServiceTypeForTable(serviceType);
            ViewBag.OutsourceServiceType = serviceType;
            return PartialView();
        }

        [HttpGet]
        public async Task<PartialViewResult> CreateOutputServiceByModal(int? id, string serviceType = "")
        {
            OutputService outputService;
            ViewData["OutputList"] = new SelectList(db.OutputServices, "Id", "Name");
            if (id != null)
            { outputService = await db.OutputServices.FindAsync(id); }
            else
            { outputService = await db.OutputServices.FirstAsync(); }
            ViewData["Code"] = outputService.Id;

            ServiceFormFactory serviceFactory = CreateServiceFormByInputSection(serviceType);
            //Create Equipment Service
            if (serviceFactory.ListOutputServices.Count > 0)
            {
                ViewBag.ListOutputServices = new List<OutputServiceViewModel>(serviceFactory.ListOutputServices);
            }
            else
            {
                ViewBag.ListOutputServices = null;
            }
            
            //Create metadata for webpage structure
            if (string.Compare(serviceType, string.Concat(PreWedding, HTMLTagForReplace)) == 0)
            {
                ViewData["ButtonIdForDWL"] = HTMLDWLOutputPreWeddingServices;
                ViewData["DivServiceForm"] = HTMLOutputModalPreWedding;
                ViewData["TblEquipment"] = HTMLTableOutputPreWedding;
            }
            else if (string.Compare(serviceType, string.Concat(Engagement, HTMLTagForReplace)) == 0)
            {
                ViewData["ButtonIdForDWL"] = HTMLDWLOutputEngagementServices;
                ViewData["DivServiceForm"] = HTMLOutputModalEngagement;
                ViewData["TblEquipment"] = HTMLTableOutputEngagement;
            }
            else if (string.Compare(serviceType, string.Concat(Wedding, HTMLTagForReplace)) == 0)
            {
                ViewData["ButtonIdForDWL"] = HTMLDWLOutputWeddingServices;
                ViewData["DivServiceForm"] = HTMLOutputModalWedding;
                ViewData["TblEquipment"] = HTMLTableOutputWedding;
            }
            ViewData["ServiceType"] = serviceType;
            return PartialView(outputService);
        }

        [HttpPost]
        public async Task<PartialViewResult> CreateOutputServiceTable([Bind(Include = "Name,PortFolioURL,Price,Cost,Description")]OutputService outputService, string ServiceType,int Code)
        {
            OutputService _outputService = outputService;
            ServiceFormFactory serviceFactory = CreateServiceFormByInputSection(ServiceType);
            serviceFactory.CreateOutputServiceList(outputService,Code);
            //Create Equipment Service
            if (serviceFactory.ListOutputServices.Count > 0)
            {
                ViewBag.ListOutputServices = new List<OutputServiceViewModel>(serviceFactory.ListOutputServices);
                //Add to database
            }
            else
            {
                ViewBag.ListOutputServices = null;
            }
            ViewData["TblEquipment"] = ServiceTypeForTable(ServiceType);
            ViewBag.OutputServiceType = ServiceType;
            return PartialView();
        }

        public PartialViewResult RemoveOutput(int? Id, string serviceType)
        {
            ServiceFormFactory serviceFactory = CreateServiceFormByInputSection(serviceType);
            if (serviceFactory != null)
            {
                var deleteTarget = serviceFactory.ListOutputServices.Select(element => element.Id == Id);
                serviceFactory.ListOutputServices.RemoveAll(item => item.Id == Id);
            }
            ViewBag.ModifyOutput = serviceFactory.ListOutputServices;
            ViewData["TblEquipment"] = ServiceTypeForTable(serviceType);
            ViewBag.OutputServiceType = serviceType;
            return PartialView();
        }

        #endregion

        #region Save Form and Calculate Service Charge

        private void SummarizePrice()
        {
            decimal _photoGraphPrice = 0;
            decimal _equipmentPrice = 0;
            decimal _locationPrice = 0;
            decimal _outsourcePrice = 0;
            decimal _outputPrice = 0;

            var _servicesTmp = TempData["Services"] as ServicesViewModel;
            var _promotionCalculatorTmp = TempData["Promotion"] as PromotionCalculator;
            TempData.Keep();

            _photoGraphPrice = GettingPriceFromPhotographService(_servicesTmp.ServiceFormPreWedding)
                                + GettingPriceFromPhotographService(_servicesTmp.ServiceFormEngagement)
                                + GettingPriceFromPhotographService(_servicesTmp.ServiceFormWedding);
            _equipmentPrice = GettingPriceFromEquipmentService(_servicesTmp.ServiceFormPreWedding)
                               + GettingPriceFromEquipmentService(_servicesTmp.ServiceFormEngagement)
                               + GettingPriceFromEquipmentService(_servicesTmp.ServiceFormWedding);
            _locationPrice = GettingPriceFromLocationService(_servicesTmp.ServiceFormPreWedding)
                                + GettingPriceFromLocationService(_servicesTmp.ServiceFormEngagement)
                                + GettingPriceFromLocationService(_servicesTmp.ServiceFormWedding);
            _outsourcePrice = GettingPriceFromOutsourceService(_servicesTmp.ServiceFormPreWedding)
                                + GettingPriceFromOutsourceService(_servicesTmp.ServiceFormEngagement)
                                + GettingPriceFromOutsourceService(_servicesTmp.ServiceFormWedding);
            _outputPrice = GettingPriceFromOutputService(_servicesTmp.ServiceFormPreWedding)
                                + GettingPriceFromOutputService(_servicesTmp.ServiceFormEngagement)
                                + GettingPriceFromOutputService(_servicesTmp.ServiceFormWedding);

            _promotionCalculatorTmp.CalculateCurrentPrice(_photoGraphPrice, _equipmentPrice, _locationPrice, _outsourcePrice, _outputPrice);
            
            //Get price from service section
            if (_servicesTmp.ServiceFormPreWedding != null)
            {
                if (_servicesTmp.ServiceFormPreWedding.ServiceForm != null)
                {
                    _servicesTmp.ServiceFormPreWedding.ServiceForm.ServicePrice = GettingPriceFromPhotographService(_servicesTmp.ServiceFormPreWedding)
                                                                           + GettingPriceFromEquipmentService(_servicesTmp.ServiceFormPreWedding)
                                                                           + GettingPriceFromLocationService(_servicesTmp.ServiceFormPreWedding)
                                                                           + GettingPriceFromOutsourceService(_servicesTmp.ServiceFormPreWedding)
                                                                           + GettingPriceFromOutputService(_servicesTmp.ServiceFormPreWedding);

                    _servicesTmp.ServiceFormPreWedding.ServiceForm.ServiceCost = GettingCostFromPhotographService(_servicesTmp.ServiceFormPreWedding)
                                                                       + GettingCostFromEquipmentService(_servicesTmp.ServiceFormPreWedding)
                                                                       + GettingCostFromLocationService(_servicesTmp.ServiceFormPreWedding)
                                                                       + GettingCostFromOutsourceService(_servicesTmp.ServiceFormPreWedding)
                                                                       + GettingCostFromOutputService(_servicesTmp.ServiceFormPreWedding);
                }
            }

            if (_servicesTmp.ServiceFormEngagement != null)
            {
                if (_servicesTmp.ServiceFormEngagement.ServiceForm != null)
                {
                    _servicesTmp.ServiceFormEngagement.ServiceForm.ServicePrice = GettingPriceFromPhotographService(_servicesTmp.ServiceFormEngagement)
                                                                        + GettingPriceFromEquipmentService(_servicesTmp.ServiceFormEngagement)
                                                                        + GettingPriceFromLocationService(_servicesTmp.ServiceFormEngagement)
                                                                        + GettingPriceFromOutsourceService(_servicesTmp.ServiceFormEngagement)
                                                                        + GettingPriceFromOutputService(_servicesTmp.ServiceFormEngagement);

                    _servicesTmp.ServiceFormEngagement.ServiceForm.ServiceCost = GettingCostFromPhotographService(_servicesTmp.ServiceFormEngagement)
                                                                        + GettingCostFromEquipmentService(_servicesTmp.ServiceFormEngagement)
                                                                        + GettingCostFromLocationService(_servicesTmp.ServiceFormEngagement)
                                                                        + GettingCostFromOutsourceService(_servicesTmp.ServiceFormEngagement)
                                                                        + GettingCostFromOutputService(_servicesTmp.ServiceFormEngagement);
                }
            }

            if (_servicesTmp.ServiceFormWedding != null)
            {
                if (_servicesTmp.ServiceFormWedding.ServiceForm != null)
                {
                    _servicesTmp.ServiceFormWedding.ServiceForm.ServicePrice = GettingPriceFromPhotographService(_servicesTmp.ServiceFormWedding)
                                                                      + GettingPriceFromEquipmentService(_servicesTmp.ServiceFormWedding)
                                                                      + GettingPriceFromLocationService(_servicesTmp.ServiceFormWedding)
                                                                      + GettingPriceFromOutsourceService(_servicesTmp.ServiceFormWedding)
                                                                      + GettingPriceFromOutputService(_servicesTmp.ServiceFormWedding);

                    _servicesTmp.ServiceFormWedding.ServiceForm.ServiceCost = GettingCostFromPhotographService(_servicesTmp.ServiceFormWedding)
                                                                               + GettingCostFromEquipmentService(_servicesTmp.ServiceFormWedding)
                                                                               + GettingCostFromLocationService(_servicesTmp.ServiceFormWedding)
                                                                               + GettingCostFromOutsourceService(_servicesTmp.ServiceFormWedding)
                                                                               + GettingCostFromOutputService(_servicesTmp.ServiceFormWedding);
                }
            }

            TempData["Services"] = _servicesTmp;
            TempData["Promotion"] = _promotionCalculatorTmp;
        }

        private void SummarizePriceForEdit()
        {
            decimal _photoGraphPrice = 0;
            decimal _equipmentPrice = 0;
            decimal _locationPrice = 0;
            decimal _outsourcePrice = 0;
            decimal _outputPrice = 0;

            var _servicesTmp = TempData["ServiceEdit"] as ServicesViewModel;
            var _promotionCalculatorTmp = TempData["PromotionEdit"] as PromotionCalculator;
            TempData.Keep();

            _photoGraphPrice = GettingPriceFromPhotographService(_servicesTmp.ServiceFormPreWedding)
                                + GettingPriceFromPhotographService(_servicesTmp.ServiceFormEngagement)
                                + GettingPriceFromPhotographService(_servicesTmp.ServiceFormWedding);
            _equipmentPrice = GettingPriceFromEquipmentService(_servicesTmp.ServiceFormPreWedding)
                               + GettingPriceFromEquipmentService(_servicesTmp.ServiceFormEngagement)
                               + GettingPriceFromEquipmentService(_servicesTmp.ServiceFormWedding);
            _locationPrice = GettingPriceFromLocationService(_servicesTmp.ServiceFormPreWedding)
                                + GettingPriceFromLocationService(_servicesTmp.ServiceFormEngagement)
                                + GettingPriceFromLocationService(_servicesTmp.ServiceFormWedding);
            _outsourcePrice = GettingPriceFromOutsourceService(_servicesTmp.ServiceFormPreWedding)
                                + GettingPriceFromOutsourceService(_servicesTmp.ServiceFormEngagement)
                                + GettingPriceFromOutsourceService(_servicesTmp.ServiceFormWedding);
            _outputPrice = GettingPriceFromOutputService(_servicesTmp.ServiceFormPreWedding)
                                + GettingPriceFromOutputService(_servicesTmp.ServiceFormEngagement)
                                + GettingPriceFromOutputService(_servicesTmp.ServiceFormWedding);

            _promotionCalculatorTmp.CalculateCurrentPrice(_photoGraphPrice, _equipmentPrice, _locationPrice, _outsourcePrice, _outputPrice);
            _servicesTmp.Calculator.CalculateCurrentPrice(_photoGraphPrice, _equipmentPrice, _locationPrice, _outsourcePrice, _outputPrice);

            //Get price from service section
            if (_servicesTmp.ServiceFormPreWedding != null)
            {
                if (_servicesTmp.ServiceFormPreWedding.ServiceForm != null)
                {
                    _servicesTmp.ServiceFormPreWedding.ServiceForm.ServicePrice = GettingPriceFromPhotographService(_servicesTmp.ServiceFormPreWedding)
                                                                           + GettingPriceFromEquipmentService(_servicesTmp.ServiceFormPreWedding)
                                                                           + GettingPriceFromLocationService(_servicesTmp.ServiceFormPreWedding)
                                                                           + GettingPriceFromOutsourceService(_servicesTmp.ServiceFormPreWedding)
                                                                           + GettingPriceFromOutputService(_servicesTmp.ServiceFormPreWedding);

                    _servicesTmp.ServiceFormPreWedding.ServiceForm.ServiceCost = GettingCostFromPhotographService(_servicesTmp.ServiceFormPreWedding)
                                                                       + GettingCostFromEquipmentService(_servicesTmp.ServiceFormPreWedding)
                                                                       + GettingCostFromLocationService(_servicesTmp.ServiceFormPreWedding)
                                                                       + GettingCostFromOutsourceService(_servicesTmp.ServiceFormPreWedding)
                                                                       + GettingCostFromOutputService(_servicesTmp.ServiceFormPreWedding);
                }
            }

            if (_servicesTmp.ServiceFormEngagement != null)
            {
                if (_servicesTmp.ServiceFormEngagement.ServiceForm != null)
                {
                    _servicesTmp.ServiceFormEngagement.ServiceForm.ServicePrice = GettingPriceFromPhotographService(_servicesTmp.ServiceFormEngagement)
                                                                        + GettingPriceFromEquipmentService(_servicesTmp.ServiceFormEngagement)
                                                                        + GettingPriceFromLocationService(_servicesTmp.ServiceFormEngagement)
                                                                        + GettingPriceFromOutsourceService(_servicesTmp.ServiceFormEngagement)
                                                                        + GettingPriceFromOutputService(_servicesTmp.ServiceFormEngagement);

                    _servicesTmp.ServiceFormEngagement.ServiceForm.ServiceCost = GettingCostFromPhotographService(_servicesTmp.ServiceFormEngagement)
                                                                        + GettingCostFromEquipmentService(_servicesTmp.ServiceFormEngagement)
                                                                        + GettingCostFromLocationService(_servicesTmp.ServiceFormEngagement)
                                                                        + GettingCostFromOutsourceService(_servicesTmp.ServiceFormEngagement)
                                                                        + GettingCostFromOutputService(_servicesTmp.ServiceFormEngagement);
                }
            }

            if (_servicesTmp.ServiceFormWedding != null)
            {
                if (_servicesTmp.ServiceFormWedding.ServiceForm != null)
                {
                    _servicesTmp.ServiceFormWedding.ServiceForm.ServicePrice = GettingPriceFromPhotographService(_servicesTmp.ServiceFormWedding)
                                                                      + GettingPriceFromEquipmentService(_servicesTmp.ServiceFormWedding)
                                                                      + GettingPriceFromLocationService(_servicesTmp.ServiceFormWedding)
                                                                      + GettingPriceFromOutsourceService(_servicesTmp.ServiceFormWedding)
                                                                      + GettingPriceFromOutputService(_servicesTmp.ServiceFormWedding);

                    _servicesTmp.ServiceFormWedding.ServiceForm.ServiceCost = GettingCostFromPhotographService(_servicesTmp.ServiceFormWedding)
                                                                               + GettingCostFromEquipmentService(_servicesTmp.ServiceFormWedding)
                                                                               + GettingCostFromLocationService(_servicesTmp.ServiceFormWedding)
                                                                               + GettingCostFromOutsourceService(_servicesTmp.ServiceFormWedding)
                                                                               + GettingCostFromOutputService(_servicesTmp.ServiceFormWedding);
                }
            }

            TempData["ServicesEdit"] = _servicesTmp;
            TempData["PromotionEdit"] = _promotionCalculatorTmp;
        }

        [HttpPost]
        public PartialViewResult CalculateServicesCostSummary()
        {
            var _promotionCalculatorTmp = TempData["Promotion"] as PromotionCalculator;
            TempData.Keep();
            if (_promotionCalculatorTmp != null)
            {
                //Get Cost from PreWedding , Engagement , Wedding 
                SummarizePrice();
                return PartialView(_promotionCalculatorTmp);
            }
            else
            {
                return PartialView(new PromotionCalculator());
            }
        }

        [HttpPost]
        public PartialViewResult CalculateServicesCostSummaryWhenEdit()
        {
            PromotionCalculator _promotionCalculatorTmp = TempData["PromotionEdit"] as PromotionCalculator;
            TempData.Keep();
            if (_promotionCalculatorTmp != null)
            {
                SummarizePriceForEdit();
                return PartialView(_promotionCalculatorTmp);
            }
            else
            {
                return PartialView(new PromotionCalculator());
            }
        }

        [HttpPost]
        public async Task<PartialViewResult> SaveAllDocument()
        {
            List<String> messages = new List<string>();
            var _servicesTmp = TempData["Services"] as ServicesViewModel;
            TempData.Keep();
            if (_servicesTmp.Customer != null)
            {
                if (_servicesTmp.ServiceFormEngagement.ServiceForm != null)
                { 
                    //Save ServiceForm Engagement, service status =1 => New
                    ServiceFormFactory _serviceFormFactory = CreateServiceFormByInputSection(Engagement + HTMLTagForReplace);
                    ServiceType serviceType = db.ServiceTypes.Where(s => string.Compare(s.ServiceTypeName, Engagement, true) == 0).FirstOrDefault();
                    await SaveServiceByCategory(_serviceFormFactory,serviceType);

                    #region original form
                    //ServiceStatu serviceStatus = db.ServiceStatus.Where(s=>s.Id ==1).FirstOrDefault();
                    //Service service = await db.Services.FindAsync(_services.Id);
                    //ServiceForm serviceForm = new ServiceForm
                    //{
                    //    Name = _services.ServiceFormEngagement.ServiceForm.Name,
                    //    ServiceType = serviceType,
                    //    ServiceStatu = serviceStatus,
                    //    EventStart = _services.ServiceFormEngagement.ServiceForm.EventStart,
                    //    EventEnd = _services.ServiceFormEngagement.ServiceForm.EventEnd,
                    //    GuestsNumber = _services.ServiceFormEngagement.ServiceForm.GuestsNumber,
                    //    ServiceCost = _services.ServiceFormEngagement.ServiceForm.ServiceCost,
                    //    ServicePrice = _services.ServiceFormEngagement.ServiceForm.ServicePrice,
                    //    Service = service
                    //};
                    //try
                    //{
                    //    db.ServiceForms.Add(serviceForm);
                    //    await db.SaveChangesAsync();
                    //}
                    //catch (DbEntityValidationException dbEx)
                    //{
                    //    foreach (var validationErrors in dbEx.EntityValidationErrors)
                    //    {
                    //        foreach (var validationError in validationErrors.ValidationErrors)
                    //        {
                    //            Trace.TraceInformation("Class: {0}, Property: {1}, Error: {2}",
                    //                validationErrors.Entry.Entity.GetType().FullName,
                    //                validationError.PropertyName,
                    //                validationError.ErrorMessage);
                    //        }
                    //    }

                    //    throw new Exception(dbEx.Message);
                    //}
                    

                    //List<string> listPhotograph = _services.ServiceFormEngagement.PhotoGraphService.PhotoGraphIdList;
                    //List<string> listCameraMan = _services.ServiceFormEngagement.PhotoGraphService.CameraMandIdList;
                    //foreach (var emp in listPhotograph)
                    //{
                    //    Employee photograph = await db.Employees.FindAsync(emp);
                    //    EmployeeSchedule empSchedule = new EmployeeSchedule
                    //    {
                    //        Id = serviceForm.Id,
                    //        ServiceForm = serviceForm,
                    //        Employee = photograph,
                    //        StartTime = _services.ServiceFormEngagement.ServiceForm.EventStart,
                    //        EndTime = _services.ServiceFormEngagement.ServiceForm.EventEnd
                    //    };
                    //    db.EmployeeSchedules.Add(empSchedule);
                    //}

                    //foreach (var emp in listCameraMan)
                    //{
                    //    Employee photograph = await db.Employees.FindAsync(emp);
                    //    EmployeeSchedule empSchedule = new EmployeeSchedule
                    //    {
                    //        Id = serviceForm.Id,
                    //        ServiceForm = serviceForm,
                    //        Employee = photograph,
                    //        StartTime = _services.ServiceFormEngagement.ServiceForm.EventStart,
                    //        EndTime = _services.ServiceFormEngagement.ServiceForm.EventEnd
                    //    };
                    //    db.EmployeeSchedules.Add(empSchedule);
                    //}
                    //try
                    //{
                    //    var result = await db.SaveChangesAsync();
                    //}
                    //catch (DbEntityValidationException dbEx)
                    //{
                    //    foreach (var validationErrors in dbEx.EntityValidationErrors)
                    //    {
                    //        foreach (var validationError in validationErrors.ValidationErrors)
                    //        {
                    //            Trace.TraceInformation("Class: {0}, Property: {1}, Error: {2}",
                    //                validationErrors.Entry.Entity.GetType().FullName,
                    //                validationError.PropertyName,
                    //                validationError.ErrorMessage);
                    //        }
                    //    }
                    //    throw new Exception(dbEx.Message);
                    //}
#endregion
                    
                }

                if (_servicesTmp.ServiceFormPreWedding.ServiceForm != null)
                {
                    ServiceFormFactory _serviceFormFactory = CreateServiceFormByInputSection(PreWedding + HTMLTagForReplace);
                    ServiceType serviceType = db.ServiceTypes.Where(s => string.Compare(s.ServiceTypeName, PreWedding, true) == 0).FirstOrDefault();
                    await SaveServiceByCategory(_serviceFormFactory, serviceType);
                }

                if (_servicesTmp.ServiceFormWedding.ServiceForm != null)
                {
                    ServiceFormFactory _serviceFormFactory = CreateServiceFormByInputSection(Wedding + HTMLTagForReplace);
                    ServiceType serviceType = db.ServiceTypes.Where(s => string.Compare(s.ServiceTypeName, Wedding, true) == 0).FirstOrDefault();
                    await SaveServiceByCategory(_serviceFormFactory, serviceType);
                }
            }
            else
            {
                messages.Add("Please create customer information");
            }
            //return RedirectToAction("Index");
            //Calculator Cost for all services
            var _promotionCalculatorTmp = TempData["Promotion"] as PromotionCalculator;
            SummarizePrice();
            decimal price = Convert.ToDecimal(_promotionCalculatorTmp.NetPrice);
            Service _service = await db.Services.FindAsync(_servicesTmp.Id);
            if (_service != null)
            { 
                _service.PayAmount = price;
                db.Entry(_service).State = EntityState.Modified;
                await db.SaveChangesAsync();
            }

            TempData.Clear();
            return PartialView();
        }

        private async Task SaveServiceByCategory(ServiceFormFactory _serviceFactory, ServiceType _serviceType)
        {
            var _servicesTmp = TempData["Services"] as ServicesViewModel;
            TempData.Keep();
            ServiceStatu serviceStatus = db.ServiceStatus.Where(s => s.Id == 1).FirstOrDefault();
            Service service = await db.Services.FindAsync(_servicesTmp.Id);
            ServiceForm serviceForm = new ServiceForm
            {
                Name = _serviceFactory.ServiceForm.Name,
                ServiceType = _serviceType,
                ServiceStatu = serviceStatus,
                EventStart = _serviceFactory.ServiceForm.EventStart,
                EventEnd = _serviceFactory.ServiceForm.EventEnd,
                GuestsNumber = _serviceFactory.ServiceForm.GuestsNumber,
                ServiceCost = _serviceFactory.ServiceForm.ServiceCost,
                ServicePrice = _serviceFactory.ServiceForm.ServicePrice,
                Service = service
            };
            try
            {
                db.ServiceForms.Add(serviceForm);
                await db.SaveChangesAsync();
            }
            catch (DbEntityValidationException dbEx)
            {
                foreach (var validationErrors in dbEx.EntityValidationErrors)
                {
                    foreach (var validationError in validationErrors.ValidationErrors)
                    {
                        Trace.TraceInformation("Class: {0}, Property: {1}, Error: {2}",
                            validationErrors.Entry.Entity.GetType().FullName,
                            validationError.PropertyName,
                            validationError.ErrorMessage);
                    }
                }

                throw new Exception(dbEx.Message);
            }

            if (_serviceFactory.PhotoGraphService != null)
            {
                List<string> listPhotograph = _serviceFactory.PhotoGraphService.PhotoGraphIdList;
                foreach (var emp in listPhotograph)
                {
                    Employee photograph = await db.Employees.FindAsync(emp);
                    EmployeeSchedule empSchedule = new EmployeeSchedule
                    {
                        ServiceForm = serviceForm,
                        Employee = photograph,
                        StartTime = _serviceFactory.ServiceForm.EventStart,
                        EndTime = _serviceFactory.ServiceForm.EventEnd,
                        EmployeeServiceId = _serviceFactory.PhotoGraphService.PhotoGraphServiceId
                    };
                    db.EmployeeSchedules.Add(empSchedule);
                }
            }

            if (_serviceFactory.PhotoGraphService != null)
            {
                List<string> listCameraMan = _serviceFactory.PhotoGraphService.CameraMandIdList;
                foreach (var emp in listCameraMan)
                {
                    Employee photograph = await db.Employees.FindAsync(emp);
                    EmployeeSchedule empSchedule = new EmployeeSchedule
                    {
                        ServiceForm = serviceForm,
                        Employee = photograph,
                        StartTime = _serviceFactory.ServiceForm.EventStart,
                        EndTime = _serviceFactory.ServiceForm.EventEnd,
                        EmployeeServiceId = _serviceFactory.PhotoGraphService.PhotoGraphServiceId
                    };
                    db.EmployeeSchedules.Add(empSchedule);
                }
            }


            //Equipment Section
            if (_serviceFactory.ListEquipmentServices.Count > 0)
            {
                List<EquipmentServiceViewModel> lstEqp = new List<EquipmentServiceViewModel>(_serviceFactory.ListEquipmentServices);
                foreach (var eqp in lstEqp)
                {
                    EquipmentSchedule equipSchedule = new EquipmentSchedule
                    {
                        ServiceForm = serviceForm,
                        EquipmentId = eqp.EquipmentId,
                        StartTime = _serviceFactory.ServiceForm.EventStart,
                        EndTime = _serviceFactory.ServiceForm.EventEnd,
                        EquipmentServiceId = eqp.EquipmentServiceId
                    };
                    db.EquipmentSchedules.Add(equipSchedule);
                }
            }

            //Location Section
            if (_serviceFactory.ListLocationServices.Count > 0)
            {
                List<LocationServiceViewModel> lstLocation = new List<LocationServiceViewModel>(_serviceFactory.ListLocationServices);
                foreach (var loc in lstLocation)
                {
                    LocationSchedule locationSchedule = new LocationSchedule
                    {
                        ServiceForm = serviceForm,
                        LocationId = loc.LocationId,
                        StartTime = _serviceFactory.ServiceForm.EventStart,
                        EndTime = _serviceFactory.ServiceForm.EventEnd,
                        LocationServiceId = loc.LocationServiceId
                    };
                    db.LocationSchedules.Add(locationSchedule);
                }
            }

            //Outsource Section
            if (_serviceFactory.ListOutsourceServices.Count > 0)
            {
                List<OutsourceServiceViewModel> lstOutsourceService = new List<OutsourceServiceViewModel>(_serviceFactory.ListOutsourceServices);
                foreach (var outsource in lstOutsourceService)
                {
                    OutsourceSchedule outsourceSchedule = new OutsourceSchedule
                    {
                        ServiceForm = serviceForm,
                        OutsourceId = outsource.OutsourceId,
                        StartTime = _serviceFactory.ServiceForm.EventStart,
                        EndTime = _serviceFactory.ServiceForm.EventEnd,
                        OutsourceServiceId = outsource.OutsourceServiceId
                    };
                    db.OutsourceSchedules.Add(outsourceSchedule);
                }
            }

            //Output Section
            if(_serviceFactory.ListOutputServices.Count > 0)
            {
                //status new -> 1
                int _statusNewOutput = 1;
                List<OutputServiceViewModel> lstOutputService = new List<OutputServiceViewModel>(_serviceFactory.ListOutputServices);
                foreach (var output in lstOutputService)
                {
                    OutputSchedule outputSchedule = new OutputSchedule
                    {
                        ServiceForm = serviceForm,
                        OutputServiceId = output.OutputServiceId,
                        TargetDate = serviceForm.EventEnd,
                        HandOnDate = serviceForm.EventEnd.AddDays(14),
                        PackageName = output.Name,
                        Status = _statusNewOutput,
                    };
                    db.OutputSchedules.Add(outputSchedule);
                }
            }

            try
            {
                var result = await db.SaveChangesAsync();
            }
            catch (DbEntityValidationException dbEx)
            {
                foreach (var validationErrors in dbEx.EntityValidationErrors)
                {
                    foreach (var validationError in validationErrors.ValidationErrors)
                    {
                        Trace.TraceInformation("Class: {0}, Property: {1}, Error: {2}",
                            validationErrors.Entry.Entity.GetType().FullName,
                            validationError.PropertyName,
                            validationError.ErrorMessage);
                    }
                }
                throw new Exception(dbEx.Message);
            }
        }

        #region Save Document for Editing
        [HttpPost]
        public async Task<PartialViewResult> SaveAllDocumentWhenEdit()
        {
            List<String> messages = new List<string>();
            var _servicesTmp = TempData["ServicesEdit"] as ServicesViewModel;
            TempData.Keep();
            if (_servicesTmp.Customer != null)
            {
                if (_servicesTmp.ServiceFormEngagement.ServiceForm != null)
                {
                    //Save ServiceForm Engagement, service status =1 => New
                    ServiceFormFactory _serviceFormFactory = CreateServiceFormByInputSectionWhenEdit(Engagement + HTMLTagForReplace);
                    ServiceType serviceType = db.ServiceTypes.Where(s => string.Compare(s.ServiceTypeName, Engagement, true) == 0).FirstOrDefault();
                    await EditServiceByCategory(_serviceFormFactory, serviceType);

                    #region original form
                    //ServiceStatu serviceStatus = db.ServiceStatus.Where(s=>s.Id ==1).FirstOrDefault();
                    //Service service = await db.Services.FindAsync(_services.Id);
                    //ServiceForm serviceForm = new ServiceForm
                    //{
                    //    Name = _services.ServiceFormEngagement.ServiceForm.Name,
                    //    ServiceType = serviceType,
                    //    ServiceStatu = serviceStatus,
                    //    EventStart = _services.ServiceFormEngagement.ServiceForm.EventStart,
                    //    EventEnd = _services.ServiceFormEngagement.ServiceForm.EventEnd,
                    //    GuestsNumber = _services.ServiceFormEngagement.ServiceForm.GuestsNumber,
                    //    ServiceCost = _services.ServiceFormEngagement.ServiceForm.ServiceCost,
                    //    ServicePrice = _services.ServiceFormEngagement.ServiceForm.ServicePrice,
                    //    Service = service
                    //};
                    //try
                    //{
                    //    db.ServiceForms.Add(serviceForm);
                    //    await db.SaveChangesAsync();
                    //}
                    //catch (DbEntityValidationException dbEx)
                    //{
                    //    foreach (var validationErrors in dbEx.EntityValidationErrors)
                    //    {
                    //        foreach (var validationError in validationErrors.ValidationErrors)
                    //        {
                    //            Trace.TraceInformation("Class: {0}, Property: {1}, Error: {2}",
                    //                validationErrors.Entry.Entity.GetType().FullName,
                    //                validationError.PropertyName,
                    //                validationError.ErrorMessage);
                    //        }
                    //    }

                    //    throw new Exception(dbEx.Message);
                    //}


                    //List<string> listPhotograph = _services.ServiceFormEngagement.PhotoGraphService.PhotoGraphIdList;
                    //List<string> listCameraMan = _services.ServiceFormEngagement.PhotoGraphService.CameraMandIdList;
                    //foreach (var emp in listPhotograph)
                    //{
                    //    Employee photograph = await db.Employees.FindAsync(emp);
                    //    EmployeeSchedule empSchedule = new EmployeeSchedule
                    //    {
                    //        Id = serviceForm.Id,
                    //        ServiceForm = serviceForm,
                    //        Employee = photograph,
                    //        StartTime = _services.ServiceFormEngagement.ServiceForm.EventStart,
                    //        EndTime = _services.ServiceFormEngagement.ServiceForm.EventEnd
                    //    };
                    //    db.EmployeeSchedules.Add(empSchedule);
                    //}

                    //foreach (var emp in listCameraMan)
                    //{
                    //    Employee photograph = await db.Employees.FindAsync(emp);
                    //    EmployeeSchedule empSchedule = new EmployeeSchedule
                    //    {
                    //        Id = serviceForm.Id,
                    //        ServiceForm = serviceForm,
                    //        Employee = photograph,
                    //        StartTime = _services.ServiceFormEngagement.ServiceForm.EventStart,
                    //        EndTime = _services.ServiceFormEngagement.ServiceForm.EventEnd
                    //    };
                    //    db.EmployeeSchedules.Add(empSchedule);
                    //}
                    //try
                    //{
                    //    var result = await db.SaveChangesAsync();
                    //}
                    //catch (DbEntityValidationException dbEx)
                    //{
                    //    foreach (var validationErrors in dbEx.EntityValidationErrors)
                    //    {
                    //        foreach (var validationError in validationErrors.ValidationErrors)
                    //        {
                    //            Trace.TraceInformation("Class: {0}, Property: {1}, Error: {2}",
                    //                validationErrors.Entry.Entity.GetType().FullName,
                    //                validationError.PropertyName,
                    //                validationError.ErrorMessage);
                    //        }
                    //    }
                    //    throw new Exception(dbEx.Message);
                    //}
                    #endregion

                }

                if (_servicesTmp.ServiceFormPreWedding.ServiceForm != null)
                {
                    ServiceFormFactory _serviceFormFactory = CreateServiceFormByInputSectionWhenEdit(PreWedding + HTMLTagForReplace);
                    ServiceType serviceType = db.ServiceTypes.Where(s => string.Compare(s.ServiceTypeName, PreWedding, true) == 0).FirstOrDefault();
                    await EditServiceByCategory(_serviceFormFactory, serviceType);
                }

                if (_servicesTmp.ServiceFormWedding.ServiceForm != null)
                {
                    ServiceFormFactory _serviceFormFactory = CreateServiceFormByInputSectionWhenEdit(Wedding + HTMLTagForReplace);
                    ServiceType serviceType = db.ServiceTypes.Where(s => string.Compare(s.ServiceTypeName, Wedding, true) == 0).FirstOrDefault();
                    await EditServiceByCategory(_serviceFormFactory, serviceType);
                }
            }
            else
            {
                messages.Add("Please create customer information");
            }
            //return RedirectToAction("Index");
            //Calculator Cost for all services
            var _promotionCalculatorTmp = TempData["PromotionEdit"] as PromotionCalculator;
            SummarizePrice();
            decimal price = Convert.ToDecimal(_promotionCalculatorTmp.NetPrice);
            Service _service = await db.Services.FindAsync(_servicesTmp.Id);
            if (_service != null)
            {
                _service.PayAmount = price;
                db.Entry(_service).State = EntityState.Modified;
                await db.SaveChangesAsync();
            }

            TempData.Clear();
            return PartialView();
        }

        private async Task EditServiceByCategory(ServiceFormFactory _serviceFactory, ServiceType _serviceType)
        {
            var _servicesTmp = TempData["ServicesEdit"] as ServicesViewModel;
            TempData.Keep();
            ServiceStatu serviceStatus = db.ServiceStatus.Where(s => s.Id == 1).FirstOrDefault();
            Service service = await db.Services.FindAsync(_servicesTmp.Id);
            ServiceForm serviceForm = service.ServiceForms.Where(serf => serf.ServiceType.Id == _serviceType.Id).SingleOrDefault();
            serviceForm.Name = _serviceFactory.ServiceForm.Name;
            serviceForm.ServiceType = _serviceType;
            serviceForm.ServiceStatu = serviceStatus;
            serviceForm.EventStart = _serviceFactory.ServiceForm.EventStart;
            serviceForm.EventEnd = _serviceFactory.ServiceForm.EventEnd;
            serviceForm.GuestsNumber = _serviceFactory.ServiceForm.GuestsNumber;
            serviceForm. ServiceCost = _serviceFactory.ServiceForm.ServiceCost;
            serviceForm.ServicePrice = _serviceFactory.ServiceForm.ServicePrice;
            serviceForm.Service = service;
            //ServiceForm serviceForm = new ServiceForm
            //{
            //    Id = service.ServiceForms.Where(serForm => serForm.ServiceType.Id == _serviceType.Id).FirstOrDefault().Id,
            //    Name = _serviceFactory.ServiceForm.Name,
            //    ServiceType = _serviceType,
            //    ServiceStatu = serviceStatus,
            //    EventStart = _serviceFactory.ServiceForm.EventStart,
            //    EventEnd = _serviceFactory.ServiceForm.EventEnd,
            //    GuestsNumber = _serviceFactory.ServiceForm.GuestsNumber,
            //    ServiceCost = _serviceFactory.ServiceForm.ServiceCost,
            //    ServicePrice = _serviceFactory.ServiceForm.ServicePrice,
            //    Service = service
            //};
            try
            {
                db.Entry(serviceForm).State = EntityState.Modified;
                await db.SaveChangesAsync();
            }
            catch (DbEntityValidationException dbEx)
            {
                foreach (var validationErrors in dbEx.EntityValidationErrors)
                {
                    foreach (var validationError in validationErrors.ValidationErrors)
                    {
                        Trace.TraceInformation("Class: {0}, Property: {1}, Error: {2}",
                            validationErrors.Entry.Entity.GetType().FullName,
                            validationError.PropertyName,
                            validationError.ErrorMessage);
                    }
                }

                throw new Exception(dbEx.Message);
            }

            if (_serviceFactory.PhotoGraphService != null)
            {
                List<string> listPhotograph = _serviceFactory.PhotoGraphService.PhotoGraphIdList;
                List<int> listExistedPhotograph = db.EmployeeSchedules.Where(emp => emp.ServiceForm.Id == serviceForm.Id && emp.Employee.Position == PhotographType).Select(emp => emp.Id).ToList();
                foreach (var oldEmp in listExistedPhotograph)
                { 
                    EmployeeSchedule _existEmp = await db.EmployeeSchedules.FindAsync(oldEmp);
                    db.EmployeeSchedules.Remove(_existEmp);
                }
                
                foreach (var emp in listPhotograph)
                {
                    Employee photograph = await db.Employees.FindAsync(emp);
                    EmployeeSchedule empSchedule = new EmployeeSchedule
                    {
                        ServiceForm = serviceForm,
                        Employee = photograph,
                        StartTime = _serviceFactory.ServiceForm.EventStart,
                        EndTime = _serviceFactory.ServiceForm.EventEnd,
                        EmployeeServiceId = _serviceFactory.PhotoGraphService.PhotoGraphServiceId
                    };
                    db.EmployeeSchedules.Add(empSchedule);
                }
            }

            if (_serviceFactory.PhotoGraphService != null)
            {
                List<string> listCameraMan = _serviceFactory.PhotoGraphService.CameraMandIdList;
                List<int> listExistedPhotograph = db.EmployeeSchedules.Where(emp => emp.ServiceForm.Id == serviceForm.Id && emp.Employee.Position == CameraManType).Select(emp => emp.Id).ToList();
                foreach (var oldEmp in listExistedPhotograph)
                {
                    EmployeeSchedule _existEmp = await db.EmployeeSchedules.FindAsync(oldEmp);
                    db.EmployeeSchedules.Remove(_existEmp);
                }

                foreach (var emp in listCameraMan)
                {
                    Employee photograph = await db.Employees.FindAsync(emp);
                    EmployeeSchedule empSchedule = new EmployeeSchedule
                    {
                        ServiceForm = serviceForm,
                        Employee = photograph,
                        StartTime = _serviceFactory.ServiceForm.EventStart,
                        EndTime = _serviceFactory.ServiceForm.EventEnd,
                        EmployeeServiceId = _serviceFactory.PhotoGraphService.PhotoGraphServiceId
                    };
                    db.EmployeeSchedules.Add(empSchedule);
                }
            }


            //Equipment Section
            if (_serviceFactory.ListEquipmentServices.Count > 0)
            {
                List<EquipmentServiceViewModel> lstEqp = new List<EquipmentServiceViewModel>(_serviceFactory.ListEquipmentServices);
                List<int> lstExistEqp = db.EquipmentSchedules.Where(eqp => eqp.ServiceForm.Id == serviceForm.Id ).Select(eqp => eqp.Id).ToList();
                foreach (var oldEqp in lstExistEqp)
                {
                    EquipmentSchedule _existEqp = await db.EquipmentSchedules.FindAsync(oldEqp);
                    db.EquipmentSchedules.Remove(_existEqp);
                }

                foreach (var eqp in lstEqp)
                {
                    EquipmentSchedule equipSchedule = new EquipmentSchedule
                    {
                        ServiceForm = serviceForm,
                        EquipmentId = eqp.EquipmentId,
                        StartTime = _serviceFactory.ServiceForm.EventStart,
                        EndTime = _serviceFactory.ServiceForm.EventEnd,
                        EquipmentServiceId = eqp.EquipmentServiceId
                    };
                    db.EquipmentSchedules.Add(equipSchedule);
                }
            }

            //Location Section
            if (_serviceFactory.ListLocationServices.Count > 0)
            {
                List<LocationServiceViewModel> lstLocation = new List<LocationServiceViewModel>(_serviceFactory.ListLocationServices);
                List<int> lstExistLoc = db.LocationSchedules.Where(loc => loc.ServiceForm.Id == serviceForm.Id).Select(loc => loc.Id).ToList();
                foreach (var oldLoc in lstExistLoc)
                {
                    LocationSchedule _existLoc = await db.LocationSchedules.FindAsync(oldLoc);
                    db.LocationSchedules.Remove(_existLoc);
                }

                foreach (var loc in lstLocation)
                {
                    LocationSchedule locationSchedule = new LocationSchedule
                    {
                        ServiceForm = serviceForm,
                        LocationId = loc.LocationId,
                        StartTime = _serviceFactory.ServiceForm.EventStart,
                        EndTime = _serviceFactory.ServiceForm.EventEnd,
                        LocationServiceId = loc.LocationServiceId
                    };
                    db.LocationSchedules.Add(locationSchedule);
                }
            }

            //Outsource Section
            if (_serviceFactory.ListOutsourceServices.Count > 0)
            {
                List<OutsourceServiceViewModel> lstOutsourceService = new List<OutsourceServiceViewModel>(_serviceFactory.ListOutsourceServices);
                List<int> lstExisOutSource = db.OutsourceSchedules.Where(outs => outs.ServiceForm.Id == serviceForm.Id).Select(outs => outs.Id).ToList();
                foreach (var oldOuts in lstExisOutSource)
                {
                    OutsourceSchedule _existOuts = await db.OutsourceSchedules.FindAsync(oldOuts);
                    db.OutsourceSchedules.Remove(_existOuts);
                }
                foreach (var outsource in lstOutsourceService)
                {
                    OutsourceSchedule outsourceSchedule = new OutsourceSchedule
                    {
                        ServiceForm = serviceForm,
                        OutsourceId = outsource.OutsourceId,
                        StartTime = _serviceFactory.ServiceForm.EventStart,
                        EndTime = _serviceFactory.ServiceForm.EventEnd,
                        OutsourceServiceId = outsource.OutsourceServiceId
                    };
                    db.OutsourceSchedules.Add(outsourceSchedule);
                }
            }

            //Output Section
            if (_serviceFactory.ListOutputServices.Count > 0)
            {
                //status new -> 1
                int _statusNewOutput = 1;
                List<OutputServiceViewModel> lstOutputService = new List<OutputServiceViewModel>(_serviceFactory.ListOutputServices);
                List<int> lstExisOutput= db.OutputSchedules.Where(outp => outp.ServiceForm.Id == serviceForm.Id).Select(outp => outp.Id).ToList();
                foreach (var oldOutp in lstExisOutput)
                {
                    OutputSchedule _existOutp = await db.OutputSchedules.FindAsync(oldOutp);
                    db.OutputSchedules.Remove(_existOutp);
                }

                foreach (var output in lstOutputService)
                {
                    OutputSchedule outputSchedule = new OutputSchedule
                    {
                        ServiceForm = serviceForm,
                        OutputServiceId = output.OutputServiceId,
                        TargetDate = serviceForm.EventEnd,
                        HandOnDate = serviceForm.EventEnd.AddDays(14),
                        PackageName = output.Name,
                        Status = _statusNewOutput,
                    };
                    db.OutputSchedules.Add(outputSchedule);
                }
            }

            try
            {
                var result = await db.SaveChangesAsync();
            }
            catch (DbEntityValidationException dbEx)
            {
                foreach (var validationErrors in dbEx.EntityValidationErrors)
                {
                    foreach (var validationError in validationErrors.ValidationErrors)
                    {
                        Trace.TraceInformation("Class: {0}, Property: {1}, Error: {2}",
                            validationErrors.Entry.Entity.GetType().FullName,
                            validationError.PropertyName,
                            validationError.ErrorMessage);
                    }
                }
                throw new Exception(dbEx.Message);
            }
        }
        #endregion

        #endregion



        #region Private Section

        private ServiceFormFactory CreateServiceFormByInputSection(string serviceType)
        {
            var _servicesTmp = TempData["Services"] as ServicesViewModel;
            TempData.Keep();

            if (_servicesTmp != null)
            {
                if (string.Compare(serviceType, string.Concat(PreWedding, HTMLTagForReplace)) == 0)
                {
                    return _servicesTmp.ServiceFormPreWedding;
                }
                else if (string.Compare(serviceType, string.Concat(Engagement, HTMLTagForReplace)) == 0)
                {
                    return _servicesTmp.ServiceFormEngagement;
                }
                else if (string.Compare(serviceType, string.Concat(Wedding, HTMLTagForReplace)) == 0)
                {
                    return _servicesTmp.ServiceFormWedding;
                }
                else
                { return null; }
            }
            else
            {
                return null;
            }  
        }

        private ServiceFormFactory CreateServiceFormByInputSectionWhenEdit(string serviceType)
        {
            var _servicesTmp = TempData["ServiceEdit"] as ServicesViewModel;
            TempData.Keep();

            if (_servicesTmp != null)
            {
                if (string.Compare(serviceType, string.Concat(PreWedding, HTMLTagForReplace)) == 0)
                {
                    return _servicesTmp.ServiceFormPreWedding;
                }
                else if (string.Compare(serviceType, string.Concat(Engagement, HTMLTagForReplace)) == 0)
                {
                    return _servicesTmp.ServiceFormEngagement;
                }
                else if (string.Compare(serviceType, string.Concat(Wedding, HTMLTagForReplace)) == 0)
                {
                    return _servicesTmp.ServiceFormWedding;
                }
                else
                { return null; }
            }
            else
            {
                return null;
            }
        }

        private decimal GettingPriceFromPhotographService(ServiceFormFactory serviceFormFactory)
        {
            if (serviceFormFactory != null)
            {
                if (serviceFormFactory.PhotoGraphService != null)
                {
                    return Convert.ToDecimal(serviceFormFactory.PhotoGraphService.Price);
                }
                else
                {
                    return 0;
                }
            }
            else
            {
                return 0;
            }
        }

        private decimal GettingPriceFromEquipmentService(ServiceFormFactory serviceFormFactory)
        {
            if (serviceFormFactory != null)
            {
                if (serviceFormFactory.ListEquipmentServices.Count > 0)
                {
                    return Convert.ToDecimal(serviceFormFactory.ListEquipmentServices.Sum(item => item.Price));
                }
                else
                {
                    return 0;
                }
            }
            else
            {
                return 0;
            }
        }

        private decimal GettingPriceFromLocationService(ServiceFormFactory serviceFormFactory)
        {
            if (serviceFormFactory != null)
            {
                if (serviceFormFactory.ListLocationServices.Count > 0)
                {
                    return Convert.ToDecimal(serviceFormFactory.ListLocationServices.Sum(item => item.Price));
                }
                else
                {
                    return 0;
                }
            }
            else
            {
                return 0;
            }
        }

        private decimal GettingPriceFromOutsourceService(ServiceFormFactory serviceFormFactory)
        {
            if (serviceFormFactory != null)
            {
                if (serviceFormFactory.ListOutsourceServices.Count > 0)
                {
                    return Convert.ToDecimal(serviceFormFactory.ListOutsourceServices.Sum(item => item.Price));
                }
                else
                {
                    return 0;
                }
            }
            else
            {
                return 0;
            }
        }

        private decimal GettingPriceFromOutputService(ServiceFormFactory serviceFormFactory)
        {
            if (serviceFormFactory != null)
            {
                if (serviceFormFactory.ListOutputServices.Count > 0)
                {
                    return Convert.ToDecimal(serviceFormFactory.ListOutputServices.Sum(item => item.Price));
                }
                else
                {
                    return 0;
                }
            }
            else
            {
                return 0;
            }
        }


        private decimal GettingCostFromPhotographService(ServiceFormFactory serviceFormFactory)
        {
            if (serviceFormFactory != null)
            {
                if (serviceFormFactory.PhotoGraphService != null)
                {
                    return Convert.ToDecimal(serviceFormFactory.PhotoGraphService.Cost);
                }
                else
                {
                    return 0;
                }
            }
            else
            {
                return 0;
            }
        }

        private decimal GettingCostFromEquipmentService(ServiceFormFactory serviceFormFactory)
        {
            if (serviceFormFactory != null)
            {
                if (serviceFormFactory.ListEquipmentServices.Count > 0)
                {
                    return Convert.ToDecimal(serviceFormFactory.ListEquipmentServices.Sum(item => item.Cost));
                }
                else
                {
                    return 0;
                }
            }
            else
            {
                return 0;
            }
        }

        private decimal GettingCostFromLocationService(ServiceFormFactory serviceFormFactory)
        {
            if (serviceFormFactory != null)
            {
                if (serviceFormFactory.ListLocationServices.Count > 0)
                {
                    return Convert.ToDecimal(serviceFormFactory.ListLocationServices.Sum(item => item.Cost));
                }
                else
                {
                    return 0;
                }
            }
            else
            {
                return 0;
            }
        }

        private decimal GettingCostFromOutsourceService(ServiceFormFactory serviceFormFactory)
        {
            if (serviceFormFactory != null)
            {
                if (serviceFormFactory.ListOutsourceServices.Count > 0)
                {
                    return Convert.ToDecimal(serviceFormFactory.ListOutsourceServices.Sum(item => item.Cost));
                }
                else
                {
                    return 0;
                }
            }
            else
            {
                return 0;
            }
        }

        private decimal GettingCostFromOutputService(ServiceFormFactory serviceFormFactory)
        {
            if (serviceFormFactory != null)
            {
                if (serviceFormFactory.ListOutputServices.Count > 0)
                {
                    return Convert.ToDecimal(serviceFormFactory.ListOutputServices.Sum(item => item.Cost));
                }
                else
                {
                    return 0;
                }
            }
            else
            {
                return 0;
            }
        }

    }
}
        #endregion