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

namespace NicePictureStudio
{
    public class EmployeeSchedulesController : Controller
    {
        private NicePictureStudioDBEntities db = new NicePictureStudioDBEntities();

        // GET: EmployeeSchedules
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
            return View(await db.EmployeeSchedules.ToListAsync());
        }

        // GET: EmployeeSchedules/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EmployeeSchedule employeeSchedule = await db.EmployeeSchedules.FindAsync(id);
            if (employeeSchedule == null)
            {
                return HttpNotFound();
            }
            return View(employeeSchedule);
        }

        // GET: EmployeeSchedules/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: EmployeeSchedules/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,StartTime,EndTime,EmployeeServiceId")] EmployeeSchedule employeeSchedule)
        {
            if (ModelState.IsValid)
            {
                db.EmployeeSchedules.Add(employeeSchedule);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(employeeSchedule);
        }

        // GET: EmployeeSchedules/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EmployeeSchedule employeeSchedule = await db.EmployeeSchedules.FindAsync(id);
            if (employeeSchedule == null)
            {
                return HttpNotFound();
            }
            return View(employeeSchedule);
        }

        // POST: EmployeeSchedules/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,StartTime,EndTime,EmployeeServiceId")] EmployeeSchedule employeeSchedule)
        {
            if (ModelState.IsValid)
            {
                db.Entry(employeeSchedule).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(employeeSchedule);
        }

        // GET: EmployeeSchedules/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EmployeeSchedule employeeSchedule = await db.EmployeeSchedules.FindAsync(id);
            if (employeeSchedule == null)
            {
                return HttpNotFound();
            }
            return View(employeeSchedule);
        }

        // POST: EmployeeSchedules/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            EmployeeSchedule employeeSchedule = await db.EmployeeSchedules.FindAsync(id);
            db.EmployeeSchedules.Remove(employeeSchedule);
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

        public PartialViewResult EmployeesScheduler(string photographId, int? serviceTypeId, int? statusId, bool? isConfirm, bool? isNotFinish)
        {
            ViewBag.PhotographerId = photographId == string.Empty ? Constant.UNDEFINED : photographId;
            ViewBag.ServiceTypeId = serviceTypeId == null ? Constant.DEFAULT : serviceTypeId;
            ViewBag.Status = statusId == null ? Constant.DEFAULT : statusId;
            ViewBag.IsConfirm = isConfirm == null ? false : isConfirm;
            ViewBag.IsNotFinish = isNotFinish == null ? false : isNotFinish;
            return PartialView();
        }

        public virtual JsonResult Employees_Read([DataSourceRequest] DataSourceRequest request, string phothgraphId, int serviceTypeId,int? statusId, bool? isConfirm, bool? isNotFinish)
        {
            IQueryable<SchedulerViewModels> tasks = CreateEmployeeSchedules(phothgraphId, serviceTypeId, statusId, isConfirm, isNotFinish).Select(task => new SchedulerViewModels()
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

        public virtual JsonResult Employees_Update([DataSourceRequest] DataSourceRequest request, SchedulerViewModels service)
        {
            if (ModelState.IsValid)
            {
                if (ValidateModel(service, ModelState))
                {
                    if (string.IsNullOrEmpty(service.Title))
                    {
                        service.Title = "";
                    }
                    var entity = db.EmployeeSchedules.FirstOrDefault(m => m.Id == service.Id);
                    entity.StartTime = service.Start;
                    entity.EndTime = service.End;
                    entity.Status = service.selectedStatus;
                    db.Entry(entity).State = EntityState.Modified;
                    db.SaveChanges();
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

        private List<SchedulerViewModels> CreateEmployeeSchedules(string photographId, int? serviceTypeId, int? statusId, bool? isConfirm, bool? isNotFinish)
        {
            List<SchedulerViewModels> _listSchecule = new List<SchedulerViewModels>();
            var allemployee = (from empSchedule in db.EmployeeSchedules
                              join emp in db.Employees on empSchedule.Employee.Id equals emp.Id
                              where (emp.EmployeePositions.FirstOrDefault().Id == Constant.EMPLOYEE_POSITION_PHOTOGRAPH || emp.EmployeePositions.FirstOrDefault().Id == Constant.EMPLOYEE_POSITION_CAMERAMAN)
                               select empSchedule).ToList();

            //Add condition for filtering
            var filterServiceForms = allemployee;
            if (photographId !=Constant.UNDEFINED && photographId != null)
            {
                filterServiceForms = filterServiceForms.Where(s => s.Employee.Id == photographId).Select(s => s).ToList();
            }

            if (serviceTypeId > 0 && statusId != null)
            {
               filterServiceForms = filterServiceForms.Where(s => s.ServiceForm.ServiceType.Id == serviceTypeId).ToList();
            }

            if (statusId > 0 && statusId != null)
            {
                filterServiceForms = filterServiceForms.Where(s => s.Status == statusId).ToList();
            }

            if (isConfirm != null)
            {
                if (isConfirm == true && statusId < Constant.SERVICE_STATUS_NEW)
                { filterServiceForms = filterServiceForms.Where(s => s.Status <= Constant.SERVICE_STATUS_CONFIRM).ToList(); }
            }

            if (isNotFinish != null)
            {
                if (isNotFinish == true && statusId < Constant.SERVICE_STATUS_NEW)
                {
                    var currentDate = DateTime.Now;
                    filterServiceForms = filterServiceForms.Where(s => (s.StartTime - currentDate).TotalDays > 3 && s.Status <= Constant.SERVICE_STATUS_CONFIRM).ToList();
                }
            }
            //Add condition for filtering
            foreach (var item in filterServiceForms)
            {
                if (item.ServiceForm != null)
                {
                    SchedulerViewModels _scheduler = new SchedulerViewModels
                    {
                        Id = item.Id,
                        Title = item.Employee.Name,
                        Description = item.ServiceForm.Name,
                        Start = item.StartTime,
                        End = item.EndTime,
                        selectedStatus = item.Status
                    };
                    _listSchecule.Add(_scheduler);
                }
            }
            return _listSchecule;
        }

        public ActionResult DetailsEmployee(int? EmployeeScheduleId)
        {
            //ViewBag.EquipmentSetId = equipmentId;

            //Generate Service Details
            //Getting Information from Service
            //ServiceFormId => schedule for equipment
            if (EmployeeScheduleId != null)
            {
                //find ServiceForm Id from emp schedule id
                var serviceFormId = db.ServiceForms.Where(sv => sv.EmployeeSchedules.Any(eqps => eqps.Id == EmployeeScheduleId)).Select(s => s.Id).FirstOrDefault();
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
                        PhoneNumber = employee.PhoneNumber,
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

                var TableReport = new TableReportModel
                {
                    OutsourceId = EmployeeScheduleId,
                    MainPhotoGraph = empPhotoGraph.Count > 0 ? empPhotoGraph.FirstOrDefault().Name : "",
                    Position = empPhotoGraph.Count > 0 ? empPhotoGraph.FirstOrDefault().Position : "",
                    PhotoGraphPhoneNumber = empPhotoGraph.Count > 0 ? empPhotoGraph.FirstOrDefault().PhoneNumber : "",
                    Bride = services.BrideName,
                    Groom = services.GroomName,
                    SpecialRequest = services.SpecialRequest != null ? services.SpecialRequest : "",
                    Suggestion = suggestion,
                    Location = locationName,
                    LocationDetails = locationDetails != null ? locationDetails : "",
                    Map = Map,
                    LocatioNumber = locationNumber,
                    BookingCode = booking != null ? booking.BookingCode : Constant.UNDEFINED,
                    BookingRequest = bookingSpecialRequest,
                    listEmployee = empPhotoGraph
                };

                return PartialView(TableReport);
            }

            return PartialView();
        }

        public PartialViewResult DescriptionEmployee(int? employeeScheduleId, List<EmployeeDetails> lstEmp)
        {
            //Select Outsource ID
            ViewBag.PhotoGrapherList = new List<EmployeeDetails>(lstEmp);
            if (employeeScheduleId != null)
            {
                var serviceForm = db.EmployeeSchedules.Find(employeeScheduleId).ServiceForm;
                var photoServiceId = db.EmployeeSchedules.Find(employeeScheduleId).EmployeeServiceId;
                var photoGraphService = db.PhotographServices.Find(photoServiceId);
                PhotographInfo photoItem = new PhotographInfo 
                { 
                    Numphotographer = photoGraphService.PhotographerNumber,
                    NumCameraman = photoGraphService.CameraManNumber,
                    GuestsNumber = serviceForm.GuestsNumber,
                    Description = photoGraphService.Name
                };
                return PartialView(photoItem);
            }

            return PartialView();
        }
    }
}
