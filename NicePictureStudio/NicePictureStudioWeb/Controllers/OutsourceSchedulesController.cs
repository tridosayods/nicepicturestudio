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
    public class OutsourceSchedulesController : Controller
    {
        private NicePictureStudioDBEntities db = new NicePictureStudioDBEntities();

        // GET: OutsourceSchedules
        public async Task<ActionResult> Index()
        {
            //Employee Management

            //Service Management
            var _OutsourceTypeList = db.OutsourceServiceTypes.ToList();
            OutsourceServiceType defaultOutsourceType = new OutsourceServiceType { Id = Constant.DEFAULT, TypeName = "เลือกทั้งหมด" };
            _OutsourceTypeList.Insert(0, defaultOutsourceType);

            //Status Management
            var _statusList = db.ServiceStatus.ToList();
            ServiceStatu defaultStatus = new ServiceStatu { Id = Constant.DEFAULT, StatusName = "เลือกทั้งหมด" };
            _statusList.Insert(0, defaultStatus);

            ViewBag.ServiceTypeList = new SelectList(_OutsourceTypeList, "Id", "TypeName");
            ViewBag.StatusList = new SelectList(_statusList, "Id", "StatusName");
            return View(await db.OutsourceSchedules.ToListAsync());
        }

        // GET: OutsourceSchedules/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            OutsourceSchedule outsourceSchedule = await db.OutsourceSchedules.FindAsync(id);
            if (outsourceSchedule == null)
            {
                return HttpNotFound();
            }
            return View(outsourceSchedule);
        }

        // GET: OutsourceSchedules/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: OutsourceSchedules/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,StartTime,EndTime,OutsourceId,OutsourceServiceId,Status")] OutsourceSchedule outsourceSchedule)
        {
            if (ModelState.IsValid)
            {
                db.OutsourceSchedules.Add(outsourceSchedule);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(outsourceSchedule);
        }

        // GET: OutsourceSchedules/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            OutsourceSchedule outsourceSchedule = await db.OutsourceSchedules.FindAsync(id);
            if (outsourceSchedule == null)
            {
                return HttpNotFound();
            }
            return View(outsourceSchedule);
        }

        // POST: OutsourceSchedules/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,StartTime,EndTime,OutsourceId,OutsourceServiceId,Status")] OutsourceSchedule outsourceSchedule)
        {
            if (ModelState.IsValid)
            {
                db.Entry(outsourceSchedule).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(outsourceSchedule);
        }

        // GET: OutsourceSchedules/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            OutsourceSchedule outsourceSchedule = await db.OutsourceSchedules.FindAsync(id);
            if (outsourceSchedule == null)
            {
                return HttpNotFound();
            }
            return View(outsourceSchedule);
        }

        // POST: OutsourceSchedules/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            OutsourceSchedule outsourceSchedule = await db.OutsourceSchedules.FindAsync(id);
            db.OutsourceSchedules.Remove(outsourceSchedule);
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

        public PartialViewResult OutsourceScheduler(string photographId, int? serviceTypeId, int? statusId, bool? isConfirm, bool? isNotFinish)
        {
            ViewBag.PhotographerId = photographId == string.Empty ? Constant.UNDEFINED : photographId;
            ViewBag.ServiceTypeId = serviceTypeId == null ? Constant.DEFAULT : serviceTypeId;
            ViewBag.Status = statusId == null ? Constant.DEFAULT : statusId;
            ViewBag.IsConfirm = isConfirm == null ? false : isConfirm;
            ViewBag.IsNotFinish = isNotFinish == null ? false : isNotFinish;
            return PartialView();
        }

        public virtual JsonResult Outsources_Read([DataSourceRequest] DataSourceRequest request, string phothgraphId, int serviceTypeId, int? statusId, bool? isConfirm, bool? isNotFinish)
        {
            IQueryable<SchedulerViewModels> tasks = CreateOutsouceSchedules(phothgraphId, serviceTypeId, statusId, isConfirm, isNotFinish).Select(task => new SchedulerViewModels()
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

        private List<SchedulerViewModels> CreateOutsouceSchedules(string photographId, int? serviceTypeId, int? statusId, bool? isConfirm, bool? isNotFinish)
        {
            List<SchedulerViewModels> _listSchecule = new List<SchedulerViewModels>();
            var allOutsources = (from outsourceSchedule in db.OutsourceSchedules
                               join outsource in db.OutsourceContacts on outsourceSchedule.OutsourceId equals outsource.OutsourceContactId
                               select new { outsourceSchedule = outsourceSchedule, outsource = outsource }).ToList();

            //Add condition for filtering
            var filterServiceForms = allOutsources;

            if (serviceTypeId > 0 && statusId != null)
            {
                filterServiceForms = filterServiceForms.Where(s => s.outsource.OutsourceServiceType.Id == serviceTypeId).ToList();
            }

            if (statusId > 0 && statusId != null)
            {
                filterServiceForms = filterServiceForms.Where(s => s.outsourceSchedule.Status == statusId).ToList();
            }

            if (isConfirm != null)
            {
                if (isConfirm == true && statusId < Constant.SERVICE_STATUS_NEW)
                { filterServiceForms = filterServiceForms.Where(s => s.outsourceSchedule.Status <= Constant.SERVICE_STATUS_CONFIRM).ToList(); }
            }

            if (isNotFinish != null)
            {
                if (isNotFinish == true && statusId < Constant.SERVICE_STATUS_NEW)
                {
                    var currentDate = DateTime.Now;
                    filterServiceForms = filterServiceForms.Where(s => (s.outsourceSchedule.StartTime - currentDate).TotalDays > 3 && s.outsourceSchedule.Status <= Constant.SERVICE_STATUS_CONFIRM).ToList();
                }
            }
            //Add condition for filtering

            foreach (var item in filterServiceForms)
            {
                if (item.outsourceSchedule.ServiceForm != null)
                {
                    SchedulerViewModels _scheduler = new SchedulerViewModels
                    {
                        Id = item.outsourceSchedule.Id,
                        Title = item.outsource.OutsourceName +" : " + item.outsource.OutsourceServiceType.TypeName,
                        Description = item.outsource.Detail,
                        Start = item.outsourceSchedule.StartTime,
                        End = item.outsourceSchedule.EndTime,
                        selectedStatus = item.outsourceSchedule.Status
                    };
                    _listSchecule.Add(_scheduler);
                }
            }
            return _listSchecule;
        }

        public virtual JsonResult Outsources_Update([DataSourceRequest] DataSourceRequest request, SchedulerViewModels service)
        {
            if (ModelState.IsValid)
            {
                if (ValidateModel(service, ModelState))
                {
                    if (string.IsNullOrEmpty(service.Title))
                    {
                        service.Title = "";
                    }
                    var entity = db.OutsourceSchedules.FirstOrDefault(m => m.Id == service.Id);
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

        public ActionResult DetailsOutsource(int? OutSourceScheduleId)
        {
            //ViewBag.EquipmentSetId = equipmentId;

            //Generate Service Details
            //Getting Information from Service
            //ServiceFormId => schedule for equipment
            if (OutSourceScheduleId != null)
            {
                //find ServiceForm Id from emp schedule id
                var serviceFormId = db.ServiceForms.Where(sv => sv.OutsourceSchedules.Any(eqps => eqps.Id == OutSourceScheduleId)).Select(s => s.Id).FirstOrDefault();
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

                var TableReport = new TableReportModel
                {
                    OutsourceId = OutSourceScheduleId,
                    MainPhotoGraph = empPhotoGraph.Count > 0 ? empPhotoGraph.FirstOrDefault().Name : "",
                    Position = empPhotoGraph.Count > 0 ? empPhotoGraph.FirstOrDefault().Position : "",
                    PhotoGraphPhoneNumber = empPhotoGraph.Count > 0 ? empPhotoGraph.FirstOrDefault().PhoneNumber : "",
                    Bride = services.BrideName,
                    Groom = services.GroomName,
                    SpecialRequest = services.SpecialRequest,
                    Suggestion = suggestion,
                    Location = locationName,
                    LocationDetails = locationDetails,
                    Map = Map,
                    LocatioNumber = locationNumber,
                    BookingCode = booking.BookingCode,
                    BookingRequest = bookingSpecialRequest
                };

                return PartialView(TableReport);
            }

            return PartialView();
        }

        public PartialViewResult DescriptionOutsource(int? outsourceScheduleId)
        {
            //Select Outsource ID
            if (outsourceScheduleId != null )
            {
                int orsId = (int)outsourceScheduleId;
                var outsourceId = db.OutsourceSchedules.Where(os=>os.Id == orsId).FirstOrDefault().OutsourceId;
                var outsource = db.OutsourceContacts.Find(outsourceId);
                OutsourceInformation outsourceInfo = new OutsourceInformation 
                {
                    Id = outsource.OutsourceContactId,
                    Name = outsource.OutsourceName,
                    Address = outsource.Address,
                    CloseTime = outsource.CloseTime,
                    OpenTime = outsource.OpenTime,
                    Detail = outsource.Detail,
                    NumericNumber = outsource.PhoneNumber != null ? Convert.ToInt32(outsource.PhoneNumber) : 0,
                    PhoneNumber =outsource.PhoneNumber,
                    OutsourceTypeName = outsource.OutsourceServiceType.TypeName
                };
                return PartialView(outsourceInfo);
            }
            
            return PartialView();
        }
    }
}
