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

namespace NicePictureStudio
{
    public class ServicesController : Controller
    {
        private NicePictureStudioDBEntities db = new NicePictureStudioDBEntities();

        private static ServicesViewModel _services;
        private PromotionViewModel _promotion;

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
            ViewBag.CustomerId = new SelectList(db.Customers, "CustomerId", "CustomerName");
            ViewBag.BookingId = new SelectList(db.Bookings, "Id", "Name");
            //Binding a promotion from scracth or create new promotion
            ViewBag.BookingList = new SelectList(db.Bookings, "Id", "BookingName");
            return View();
        }

        public JsonResult GetListForBookingAutocomplete(string term)
        {
            Booking[] matching = string.IsNullOrWhiteSpace(term) ?
                db.Bookings.ToArray() :
                db.Bookings.Where(p => p.BookingCode.ToUpper().StartsWith(term.ToUpper()) || p.Name.ToUpper().StartsWith(term.ToUpper())).ToArray();

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
        public async Task<ActionResult> Create([Bind(Include = "Id,BookingName,GroomName,BrideName,SpecialRequest,Payment,PayAmount,CustomerId,CRMFormId")] Service service)
        {
            if (ModelState.IsValid)
            {
                //db.Services.Add(service); 
                //await db.SaveChangesAsync();
                _services.CreateService(service);
                //return RedirectToAction("Index");
                return View(service);
            }

            ViewBag.CustomerId = new SelectList(db.Customers, "CustomerId", "CustomerName", service.Id);
            return View(service);
        }

        public async Task<PartialViewResult> CreateService(int bookingId)
        {

            Booking booking = await db.Bookings.FindAsync(bookingId);
            if (booking != null)
            {
                //Getting Promotion & Booking Information
                    _promotion = new PromotionViewModel(booking.Promotion.ExpireDate,booking.Promotion.PhotoGraphDiscount,
                    booking.Promotion.EquipmentDiscount,
                    booking.Promotion.LocationDiscount, booking.Promotion.OutputDiscount, 
                    booking.Promotion.OutsourceDiscount);
                //Preparing for creating Service
                //CreateService
                // One promotion affet to any package -> need to keep price as static
                // getting promotion and then extract trype of service -> create item.

            }
            return PartialView();
        }

        // GET: Services/Edit/5
        public async Task<ActionResult> Edit(int? id)
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
            ViewBag.CustomerId = new SelectList(db.Customers, "CustomerId", "CustomerName", service.Id);
            return View(service);
        }

        // POST: Services/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,BookingName,GroomName,BrideName,SpecialRequest,Payment,PayAmount,CustomerId,CRMFormId")] Service service)
        {
            if (ModelState.IsValid)
            {
                db.Entry(service).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.CustomerId = new SelectList(db.Customers, "CustomerId", "CustomerName", service.Customer.CustomerId);
            return View(service);
        }

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

        #region Create Customer Section
        /***************************Create Customer Section*******************************************************/

        public PartialViewResult CreateCustomerFromService()
        {
            return PartialView();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<PartialViewResult> CreateCustomerFromService([Bind(Include = "CustomerId,CustomerName,PhoneNumber,Address,AnniversaryDate")] Customer customer)
        {
            if (ModelState.IsValid)
            {
                //db.Customers.Add(customer);
                //await db.SaveChangesAsync();
                _services.CreateCustomer(customer);
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
        public async Task<PartialViewResult> EditCustomerFromService([Bind(Include = "CustomerId,CustomerName,PhoneNumber,Address,AnniversaryDate")] Customer customer)
        {
            if (ModelState.IsValid)
            {
                db.Entry(customer).State = EntityState.Modified;
                await db.SaveChangesAsync();
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
            return PartialView();
        }

          [HttpPost]
          [ValidateAntiForgeryToken]
          public async Task<PartialViewResult> CreateServiceFormFromService([Bind(Include = "Id,Name,ServiceType,Status,EventStart,EventEnd,GuestsNumber")] ServiceForm serviceForm, string Command)
          {
              int statusNew = 1;
              if (ModelState.IsValid)
              {
                  ServiceType serviceType = db.ServiceTypes.Where(s => string.Compare(s.ServiceTypeName, Command, true) == 0).FirstOrDefault();
                  if (serviceType != null)
                  {
                      serviceForm.ServiceType = serviceType;
                      ServiceFormFactory serviceFactory = CreateServiceFormByInputSection(serviceType.ServiceTypeName);
                      if (serviceFactory != null)
                      {
                          serviceFactory.CreateServiceForm(serviceForm, statusNew, serviceType.Id);
                          //db.ServiceForms.Add(serviceForm);
                          //await db.SaveChangesAsync();
                      }
                      else { return PartialView(); }
                     // _services.CreateServiceForm(serviceForm, statusNew, serviceType.Id);
                  }
                  else { return PartialView(); }

                  //assign value for replacing #id in view
                  ServiceForm _serviceForm = serviceForm;
                  return PartialView(@"/Views/ServiceForms/DetailsServiceFormFromService.cshtml", serviceForm);
              }
              return PartialView();
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
            DateTime _startDate = DateTime.MinValue;
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

            var photoGraphResult = db.Employees.GroupBy(emp => emp.Id)
                                    .Where(emp => emp.Any(empList => empList.Position == PhotographType
                                        && empList.EmployeeSchedules.All(empS => (empS.StartTime < _startDate || empS.StartTime > _endDate)
                                            && (empS.EndTime <= _startDate || empS.EndTime > _endDate))
                                        )).Select(emp => new PhotoGraph { 
                                            Id = emp.FirstOrDefault().Id,
                                            Name = emp.FirstOrDefault().Name,
                                            IsSelect = _selectedPhotoGraph.Contains(emp.FirstOrDefault().Id)
                                        }).ToList();
            //ViewBag.PhotoGraphListDetails = new SelectList(photoGraphResult, "Id", "Name");
            ViewBag.PhotoGraphListDetails = photoGraphResult;

            //Getting CameraMan
            var cameraManResult = db.Employees.GroupBy(emp => emp.Id)
                                    .Where(emp => emp.Any(empList => empList.Position == CameraManType
                                        && empList.EmployeeSchedules.All(empS  =>(empS.StartTime < _startDate || empS.StartTime > _endDate )
                                            &&( empS.EndTime <= _startDate || empS.EndTime > _endDate))
                                        )).Select(emp => new CameraMan
                                        {
                                            Id = emp.FirstOrDefault().Id,
                                            Name = emp.FirstOrDefault().Name,
                                            IsSelect = _selectedPhotoGraph.Contains(emp.FirstOrDefault().Id)
                                        }).ToList();

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
        public void CreatePhotoGraphServiceList([Bind(Include = "Name,PhotographerNumber,CameraManNumber,Description")]PhotographService photoGraphService, string[] EmployeeId, string[] CameraId, string ServiceType)
        {
            PhotographService photo = photoGraphService;
            List<string> empList = new List<string>();
            List<string> camList = new List<string>();
            ServiceFormFactory serviceFactory = CreateServiceFormByInputSection(ServiceType);
            if (EmployeeId == null && CameraId == null)
            {
                serviceFactory.CreatePhotoGraphService(photo, empList, camList);
            }
            else if (EmployeeId != null && CameraId != null)
            {
                serviceFactory.CreatePhotoGraphService(photo, EmployeeId.ToList(), CameraId.ToList()); 
            }
            else if (EmployeeId != null)
            {
                serviceFactory.CreatePhotoGraphService(photo, EmployeeId.ToList(), camList);
            }
            else
            {
                serviceFactory.CreatePhotoGraphService(photo, empList, CameraId.ToList());
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
        public async Task<PartialViewResult> CreateEquipmentServiceTable([Bind(Include="Name,Price,Cost,Description")]EquipmentService equipmentService, int? EquipmentId, string ServiceType)
        {
            EquipmentService _equipment = equipmentService;
            int _equipmentId = int.Parse(EquipmentId.ToString());
            ServiceFormFactory serviceFactory = CreateServiceFormByInputSection(ServiceType);
            serviceFactory.CreateEquipmentServiceList(equipmentService, _equipmentId);
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
        public async Task<PartialViewResult> CreateLocationServiceTable([Bind(Include = "Name,Price,Cost,IsOverNight,OverNightPeriod,Description")]LocationService locationService, int? LocationId, string ServiceType)
        {
            LocationService _locationService = locationService;
            int _locationId = int.Parse(LocationId.ToString());
            ServiceFormFactory serviceFactory = CreateServiceFormByInputSection(ServiceType);
            serviceFactory.CreateLocationServiceList(locationService, _locationId);
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
        public async Task<PartialViewResult> CreateOutsourceServiceTable([Bind(Include = "Name,PortFolioURL,Price,Cost,Description")]OutsourceService outsourceService, int? OutsourceId, string ServiceType)
        {
            OutsourceService _outsourceService = outsourceService;
            int _outsourceId = int.Parse(OutsourceId.ToString());
            ServiceFormFactory serviceFactory = CreateServiceFormByInputSection(ServiceType);
            serviceFactory.CreateOutSoruceServiceList(outsourceService, _outsourceId);
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
        public async Task<PartialViewResult> CreateOutputServiceTable([Bind(Include = "Name,PortFolioURL,Price,Cost,Description")]OutputService outputService, string ServiceType)
        {
            OutputService _outputService = outputService;
            ServiceFormFactory serviceFactory = CreateServiceFormByInputSection(ServiceType);
            serviceFactory.CreateOutputServiceList(outputService);
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
            return PartialView();
        }

        #endregion

        private ServiceFormFactory CreateServiceFormByInputSection(string serviceType)
        {
            if (_services != null)
            {
                if (string.Compare(serviceType, string.Concat(PreWedding, HTMLTagForReplace)) == 0)
                {
                    return _services.ServiceFormPreWedding;
                }
                else if (string.Compare(serviceType, string.Concat(Engagement, HTMLTagForReplace)) == 0)
                {
                    return _services.ServiceFormEngagement;
                }
                else if (string.Compare(serviceType, string.Concat(Wedding, HTMLTagForReplace)) == 0)
                {
                    return _services.ServiceFormWedding;
                }
                else
                { return null; }
            }
            else
            {
                return null;
            }
           
        }
    }
}
