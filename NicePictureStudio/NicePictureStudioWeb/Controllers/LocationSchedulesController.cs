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
using Kendo.Mvc.UI;
using NicePictureStudio.Models;
using Kendo.Mvc.Extensions;
using NicePictureStudio.Utils;
using System.Text.RegularExpressions;

namespace NicePictureStudio
{
    public class LocationSchedulesController : Controller
    {
        private NicePictureStudioDBEntities db = new NicePictureStudioDBEntities();

        // GET: LocationSchedules
        public async Task<ActionResult> Index()
        {
            //Employee Management
            var _empList = db.Employees.Where(e => e.EmployeePositions.Any(em => em.Id == Constant.EMPLOYEE_POSITION_PHOTOGRAPH
                || em.Id == Constant.EMPLOYEE_POSITION_CAMERAMAN)).ToList();
            Employee defaultEmp = new Employee
            {
                Id = "",
                Name = "เลือกทั้งหมด"
            };
            _empList.Insert(0, defaultEmp);

            //Service Management
            var _serviceTypeList = db.ServiceTypes.ToList();
            ServiceType defaultServiceType = new ServiceType { Id = Constant.DEFAULT, ServiceTypeName = "เลือกทั้งหมด" };
            _serviceTypeList.Insert(0, defaultServiceType);

            //Status Management
            var _statusList = db.ServiceStatus.ToList();
            ServiceStatu defaultStatus = new ServiceStatu { Id = Constant.DEFAULT, StatusName = "เลือกทั้งหมด" };
            _statusList.Insert(0, defaultStatus);

            ViewBag.EmployeeList = new SelectList(_empList, "Id", "Name");
            ViewBag.ServiceTypeList = new SelectList(_serviceTypeList, "Id", "ServiceTypeName");
            ViewBag.StatusList = new SelectList(_statusList, "Id", "StatusName");
            return View(await db.LocationSchedules.ToListAsync());
        }

        // GET: LocationSchedules/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LocationSchedule locationSchedule = await db.LocationSchedules.FindAsync(id);
            if (locationSchedule == null)
            {
                return HttpNotFound();
            }
            return View(locationSchedule);
        }

        // GET: LocationSchedules/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: LocationSchedules/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,StartTime,EndTime,LocationId,LocationServiceId,Status")] LocationSchedule locationSchedule)
        {
            if (ModelState.IsValid)
            {
                db.LocationSchedules.Add(locationSchedule);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(locationSchedule);
        }

        // GET: LocationSchedules/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LocationSchedule locationSchedule = await db.LocationSchedules.FindAsync(id);
            if (locationSchedule == null)
            {
                return HttpNotFound();
            }
            return View(locationSchedule);
        }

        // POST: LocationSchedules/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,StartTime,EndTime,LocationId,LocationServiceId,Status")] LocationSchedule locationSchedule)
        {
            if (ModelState.IsValid)
            {
                db.Entry(locationSchedule).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(locationSchedule);
        }

        // GET: LocationSchedules/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LocationSchedule locationSchedule = await db.LocationSchedules.FindAsync(id);
            if (locationSchedule == null)
            {
                return HttpNotFound();
            }
            return View(locationSchedule);
        }

        // POST: LocationSchedules/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            LocationSchedule locationSchedule = await db.LocationSchedules.FindAsync(id);
            db.LocationSchedules.Remove(locationSchedule);
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

        public PartialViewResult LocationScheduler(string photographId, int? serviceTypeId, int? statusId, bool? isConfirm, bool? isNotFinish)
        {
            ViewBag.PhotographerId = photographId == string.Empty ? Constant.UNDEFINED : photographId;
            ViewBag.ServiceTypeId = serviceTypeId == null ? Constant.DEFAULT : serviceTypeId;
            ViewBag.Status = statusId == null ? Constant.DEFAULT : statusId;
            ViewBag.IsConfirm = isConfirm == null ? false : isConfirm;
            ViewBag.IsNotFinish = isNotFinish == null ? false : isNotFinish;
            return PartialView();
        }

        public virtual JsonResult Locations_Read([DataSourceRequest] DataSourceRequest request, string phothgraphId, int serviceTypeId, int? statusId, bool? isConfirm, bool? isNotFinish)
        {
            IQueryable<SchedulerViewModels> tasks = CreateLocationSchedules(phothgraphId, serviceTypeId, statusId, isConfirm, isNotFinish).Select(task => new SchedulerViewModels()
            {
                Id = task.Id,
                Title = task.Title,
                Description = task.Description,
                Start = task.Start,
                End = task.End,
                StartTimezone = task.StartTimezone,
                EndTimezone = task.EndTimezone,
                IsAllDay = task.IsAllDay,
                Recurrence = task.Recurrence,
                RecurrenceException = task.RecurrenceException,
                selectedStatus = task.selectedStatus
            }
            ).AsQueryable();
            return Json(tasks.ToDataSourceResult(request));
        }

        private List<SchedulerViewModels> CreateLocationSchedules(string photographId, int? serviceTypeId, int? statusId, bool? isConfirm, bool? isNotFinish)
        {
            int statuLocationInvalid = Constant.LOCATION_STATUS_CLOSED;
            List<SchedulerViewModels> _listSchecule = new List<SchedulerViewModels>();
            var allLocation = (from locatonSchedule in db.LocationSchedules
                               join loc in db.Locations on locatonSchedule.LocationId equals loc.LocationId
                               where (loc.LocationStatu.Id != statuLocationInvalid)
                               select new { locSchedule = locatonSchedule, location = loc }).ToList();

            //Add condition for filtering
            var filterServiceForms = allLocation;
            if (photographId != Constant.UNDEFINED && photographId != null)
            {
                filterServiceForms = filterServiceForms.Where(s => s.locSchedule.ServiceForm.EmployeeSchedules.Any(ems=>ems.Employee.Id == photographId)).Select(s => s).ToList();
            }

            if (serviceTypeId > 0 && statusId != null)
            {
                filterServiceForms = filterServiceForms.Where(s => s.locSchedule.ServiceForm.ServiceType.Id == serviceTypeId).ToList();
            }

            if (statusId > 0 && statusId != null)
            {
                filterServiceForms = filterServiceForms.Where(s => s.locSchedule.Status == statusId).ToList();
            }

            if (isConfirm != null)
            {
                if (isConfirm == true && statusId < Constant.SERVICE_STATUS_NEW)
                { filterServiceForms = filterServiceForms.Where(s => s.locSchedule.Status <= Constant.SERVICE_STATUS_CONFIRM).ToList(); }
            }

            if (isNotFinish != null)
            {
                if (isNotFinish == true && statusId < Constant.SERVICE_STATUS_NEW)
                {
                    var currentDate = DateTime.Now;
                    filterServiceForms = filterServiceForms.Where(s => (s.locSchedule.StartTime - currentDate).TotalDays > 3 && s.locSchedule.Status <= Constant.SERVICE_STATUS_CONFIRM).ToList();
                }
            }
            //Add condition for filtering

            foreach (var item in filterServiceForms)
            {
                if (item.locSchedule.ServiceForm != null)
                {
                    SchedulerViewModels _scheduler = new SchedulerViewModels
                    {
                        Id = item.locSchedule.Id,
                        Title = item.location.LocationName,
                        Description = item.location.Detail,
                        Start = item.locSchedule.StartTime,
                        End = item.locSchedule.EndTime,
                        selectedStatus = item.locSchedule.Status
                    };
                    _listSchecule.Add(_scheduler);
                }
            }
            return _listSchecule;
        }

        public virtual JsonResult Locations_Update([DataSourceRequest] DataSourceRequest request, SchedulerViewModels service)
        {
            if (ModelState.IsValid)
            {
                if (ValidateModel(service, ModelState))
                {
                     int? scheduleStatus = db.LocationSchedules.Find(service.Id).Status;
                     if (scheduleStatus != null)
                     {
                         if (ValidateServiceTableClass.ValidateStatus(service, ModelState, (int)scheduleStatus))
                         {
                             if (string.IsNullOrEmpty(service.Title))
                             {
                                 service.Title = "";
                             }
                             var entity = db.LocationSchedules.FirstOrDefault(m => m.Id == service.Id);
                             entity.StartTime = service.Start;
                             entity.EndTime = service.End;
                             entity.Status = service.selectedStatus;
                             db.Entry(entity).State = EntityState.Modified;
                             db.SaveChanges();
                         }
                         
                     }
                }
            }

            return Json(new[] { service }.ToDataSourceResult(request, ModelState));
        }

        private bool ValidateModel(SchedulerViewModels service, ModelStateDictionary modelState)
        {
            if (service.Start > service.End)
            {
                modelState.AddModelError("errors", "End date must be greater or equal to Start date.");
                return false;
            }

            return true;
        }

        public ActionResult DetailsLocation(int? locScheduleId)
        {
            //ViewBag.EquipmentSetId = equipmentId;

            //Generate Service Details
            //Getting Information from Service
            //ServiceFormId => schedule for equipment
            if (locScheduleId != null)
            {
                //find ServiceForm Id from emp schedule id
                var serviceFormId = db.ServiceForms.Where(sv => sv.LocationSchedules.Any(eqps => eqps.Id == locScheduleId)).Select(s => s.Id).FirstOrDefault();
                var services = db.Services.Where(s => s.ServiceForms.Any(srv => srv.Id == serviceFormId)).FirstOrDefault();
                var photoSchedules = services.ServiceForms.Where(s => s.Id == serviceFormId).Select(s => s.EmployeeSchedules).FirstOrDefault();
                var booking = services.Bookings.FirstOrDefault();
                List<EmployeeDetails> empPhotoGraph = new List<EmployeeDetails>();
                foreach (var emp in photoSchedules)
                {
                    var index = emp.Employee.Id;
                    var employee = db.Employees.Find(index);
                    var empDetail = new EmployeeDetails
                    {
                        Name = employee.EmployeeInfoes.FirstOrDefault().Title + " " +
                                employee.EmployeeInfoes.FirstOrDefault().Name + employee.EmployeeInfoes.FirstOrDefault().Surname + "(" +
                                employee.EmployeeInfoes.FirstOrDefault().Nickname + ")",
                        Position = employee.EmployeePositions.FirstOrDefault().Name,
                        Email = employee.Email,
                        PhoneNumber = Regex.Replace(employee.PhoneNumber, @"(\d{3})(\d{3})(\d{4})", "$1-$2-$3"),
                        Specialibity = employee.Specialability
                    };
                    empPhotoGraph.Add(empDetail);
                }

                //Location
                var locationName = "";
                var locationDetails = "";
                var Map = "";
                var locationNumber = "";
                var location = services.ServiceForms.Where(s => s.Id == serviceFormId).Select(s => s.Locations).FirstOrDefault();
                try
                {
                    if (location != null)
                    {
                        if (location.Count > 0)
                        {
                            locationName = location.FirstOrDefault().LocationName;
                            locationDetails = location.FirstOrDefault().MapExplanation;
                            Map = location.FirstOrDefault().MapURL;
                            locationNumber = location.FirstOrDefault().PhoneNumber;
                        }
                        else
                        {
                            var servicelocation = services.ServiceForms.Where(s => s.Id == serviceFormId).Select(s => s.LocationSchedules).FirstOrDefault();
                            if (servicelocation.Count > 0)
                            {
                                var _location = db.Locations.Find(servicelocation.FirstOrDefault().LocationId);
                                locationName = _location.LocationName;
                                locationDetails = _location.MapExplanation;
                                Map = _location.MapURL;
                                locationNumber = _location.PhoneNumber;
                            }
                           
                        }

                    }
                    else
                    {
                        var servicelocation = services.ServiceForms.Where(s => s.Id == serviceFormId).Select(s => s.LocationSchedules).FirstOrDefault();
                        if (servicelocation.Count > 0)
                        {
                            var _location = db.Locations.Find(servicelocation.FirstOrDefault().LocationId);
                            locationName = _location.LocationName;
                            locationDetails = _location.MapExplanation;
                            Map = _location.MapURL;
                            locationNumber = _location.PhoneNumber;
                        }
                       
                    }
                }
                catch { }

                //Customer
                var bookingSpecialRequest = "";
                if (booking != null)
                {
                    foreach (var item in booking.BookingSpecialRequests)
                    {
                        if (bookingSpecialRequest == "")
                        {
                            bookingSpecialRequest += item.Name;
                        }
                        else
                        {
                            bookingSpecialRequest += ", " + item.Name;
                        }
                    }
                }

                var suggestion = "";
                foreach (var item in services.ServiceSuggestions)
                {
                    if (suggestion == "")
                    {
                        suggestion += item.Name;
                    }
                    else
                    {
                        suggestion += ", " + item.Name;
                    }
                }

                //New stuff info
                var serviceForm = db.ServiceForms.Find(serviceFormId);
                var eventStart = serviceForm.EventStart;
                var eventEnd = serviceForm.EventEnd;
                var groomEmail = services.Customer.Email;
                var groomPhone = services.Customer.PhoneNumber;
                var brideEmail = services.Customer.CoupleEmail;
                var bridePhone = services.Customer.CouplePhoneNumber;
                var address = services.Customer.Address + " " +
                    services.Customer.Subdistrict + " " + services.Customer.District + " " + services.Customer.Province + " " + services.Customer.PostcalCode;
                var serviceType = serviceForm.ServiceType.ServiceTypeName;
                var guestNumber = serviceForm.GuestsNumber.ToString();
                var serviceId = services.Id;
                var bookingCode = booking == null ? "" : booking.BookingCode;

                var TableReport = new TableReportModel
                {
                    OutsourceId = locScheduleId,
                    MainPhotoGraph = empPhotoGraph.Count > 0 ? empPhotoGraph.FirstOrDefault().Name : "",
                    Position = empPhotoGraph.Count > 0 ? empPhotoGraph.FirstOrDefault().Position : "",
                    PhotoGraphPhoneNumber = empPhotoGraph.Count > 0 ? empPhotoGraph.FirstOrDefault().PhoneNumber : "",
                    Bride = services.BrideName,
                    Groom = services.GroomName,
                    SpecialRequest = services.SpecialRequest,
                    Suggestion = suggestion,
                    Location = locationName,
                    LocationDetails = locationDetails,
                    LocatioNumber = locationNumber,
                    Map = Map,
                    BookingCode = booking == null?  Constant.UNDEFINED : booking.BookingCode,
                    BookingRequest = booking ==null? string.Empty : bookingSpecialRequest,
                    listEmployee = empPhotoGraph,
                    EventStart = eventStart,
                    EventEnd = eventEnd,
                    GroomMail = groomEmail,
                    BrideMail = brideEmail,
                    GroomPhone = groomPhone,
                    BridePhone = bridePhone,
                    Address = address,
                    ServiceType = serviceType,
                    GuestNumber = guestNumber,
                    ServiceId = serviceId
                };

                return PartialView(TableReport);
            }

            return PartialView();
        }
    }
}
