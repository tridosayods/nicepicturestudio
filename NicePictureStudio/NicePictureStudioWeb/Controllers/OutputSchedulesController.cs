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
    public class OutputSchedulesController : Controller
    {
        private NicePictureStudioDBEntities db = new NicePictureStudioDBEntities();

        // GET: OutputSchedules
        public async Task<ActionResult> Index()
        {
            //Employee Management

            //Service Management
            var _OutputTypeList = db.OutputTypes.ToList();
            OutputType defaultOutputType = new OutputType { Id = Constant.DEFAULT, Name = "เลือกทั้งหมด" };
            _OutputTypeList.Insert(0, defaultOutputType);

            //Status Management
            var _statusList = db.ServiceStatus.ToList();
            ServiceStatu defaultStatus = new ServiceStatu { Id = Constant.DEFAULT, StatusName = "เลือกทั้งหมด" };
            _statusList.Insert(0, defaultStatus);

            ViewBag.ServiceTypeList = new SelectList(_OutputTypeList, "Id", "Name");
            ViewBag.StatusList = new SelectList(_statusList, "Id", "StatusName");
            return View(await db.OutputSchedules.ToListAsync());
        }

        // GET: OutputSchedules/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            OutputSchedule outputSchedule = await db.OutputSchedules.FindAsync(id);
            if (outputSchedule == null)
            {
                return HttpNotFound();
            }
            return View(outputSchedule);
        }

        // GET: OutputSchedules/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: OutputSchedules/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,PackageName,Status,TargetDate,HandOnDate,ReviseDate,ReviseCount,OutputServiceId")] OutputSchedule outputSchedule)
        {
            if (ModelState.IsValid)
            {
                db.OutputSchedules.Add(outputSchedule);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(outputSchedule);
        }

        // GET: OutputSchedules/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            OutputSchedule outputSchedule = await db.OutputSchedules.FindAsync(id);
            if (outputSchedule == null)
            {
                return HttpNotFound();
            }
            return View(outputSchedule);
        }

        // POST: OutputSchedules/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,PackageName,Status,TargetDate,HandOnDate,ReviseDate,ReviseCount,OutputServiceId")] OutputSchedule outputSchedule)
        {
            if (ModelState.IsValid)
            {
                db.Entry(outputSchedule).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(outputSchedule);
        }

        // GET: OutputSchedules/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            OutputSchedule outputSchedule = await db.OutputSchedules.FindAsync(id);
            if (outputSchedule == null)
            {
                return HttpNotFound();
            }
            return View(outputSchedule);
        }

        // POST: OutputSchedules/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            OutputSchedule outputSchedule = await db.OutputSchedules.FindAsync(id);
            db.OutputSchedules.Remove(outputSchedule);
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

        public PartialViewResult OutputScheduler(string photographId, int? serviceTypeId, int? statusId, bool? isConfirm, bool? isNotFinish)
        {
            ViewBag.PhotographerId = photographId == string.Empty ? Constant.UNDEFINED : photographId;
            ViewBag.ServiceTypeId = serviceTypeId == null ? Constant.DEFAULT : serviceTypeId;
            ViewBag.Status = statusId == null ? Constant.DEFAULT : statusId;
            ViewBag.IsConfirm = isConfirm == null ? false : isConfirm;
            ViewBag.IsNotFinish = isNotFinish == null ? false : isNotFinish;
            return PartialView();
        }

        public virtual JsonResult Outputs_Read([DataSourceRequest] DataSourceRequest request, string phothgraphId, int serviceTypeId, int? statusId, bool? isConfirm, bool? isNotFinish)
        {
            IQueryable<OutputSchedulerViewModels> tasks = CreateOutputSchedules(phothgraphId, serviceTypeId, statusId, isConfirm, isNotFinish).Select(task => new OutputSchedulerViewModels()
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
                selectedStatus = task.selectedStatus,
                ReviseDate = task.ReviseDate,
                ReviseCount = task.ReviseCount
            }
            ).AsQueryable();
            return Json(tasks.ToDataSourceResult(request));
        }

        private List<OutputSchedulerViewModels> CreateOutputSchedules(string photographId, int? serviceTypeId, int? statusId, bool? isConfirm, bool? isNotFinish)
        {
            List<OutputSchedulerViewModels> _listSchecule = new List<OutputSchedulerViewModels>();
            var alloutputs = (from outputSchedule in db.OutputSchedules
                               join output in db.OutputServices on outputSchedule.OutputServiceId equals output.Id
                               select new { outputSchedule = outputSchedule, output = output }).ToList();

            //Add condition for filtering
            var filterServiceForms = alloutputs;
            var photoId = 0;
            if (Int32.TryParse(photographId, out photoId))
            {
                if (photoId > Constant.DEFAULT && photographId != null)
                {
                    filterServiceForms = filterServiceForms.Where(s => s.outputSchedule.ServiceForm.Service.Id == photoId).Select(s => s).ToList();
                }
            }
            

            if (serviceTypeId > 0 && statusId != null)
            {
                List<int> IdServiceType = db.OutputServices.Where(ots => ots.OutputType.Id == serviceTypeId).Select(s => s.Id).ToList();
                filterServiceForms = filterServiceForms.Where(s => IdServiceType.Contains(s.outputSchedule.OutputServiceId)).ToList();
            }

            if (statusId > 0 && statusId != null)
            {
                filterServiceForms = filterServiceForms.Where(s => s.outputSchedule.Status == statusId).ToList();
            }

            if (isConfirm != null)
            {
                if (isConfirm == true && statusId < Constant.SERVICE_STATUS_NEW)
                { filterServiceForms = filterServiceForms.Where(s => s.outputSchedule.Status <= Constant.SERVICE_STATUS_CONFIRM).ToList(); }
            }

            if (isNotFinish != null)
            {
                if (isNotFinish == true && statusId < Constant.SERVICE_STATUS_NEW)
                {
                    var currentDate = DateTime.Now;
                    filterServiceForms = filterServiceForms.Where(s => (s.outputSchedule.HandOnDate - currentDate).TotalDays > 3 && s.outputSchedule.Status <= Constant.SERVICE_STATUS_CONFIRM).ToList();
                }
            }
            //Add condition for filtering

            foreach (var item in filterServiceForms)
            {
                if (item.outputSchedule.ServiceForm != null)
                {
                    OutputSchedulerViewModels _scheduler = new OutputSchedulerViewModels
                    {
                        Id = item.outputSchedule.Id,
                        Title = item.output.Name,
                        Description = item.output.Description,
                        Start = (DateTime)item.outputSchedule.TargetDate,
                        End = item.outputSchedule.HandOnDate,
                        selectedStatus = item.outputSchedule.Status,
                        ReviseDate = item.outputSchedule.ReviseDate,
                        ReviseCount = item.outputSchedule.ReviseCount
                    };
                    _listSchecule.Add(_scheduler);
                }
            }
            return _listSchecule;
        }

        public virtual JsonResult Outputs_Update([DataSourceRequest] DataSourceRequest request, OutputSchedulerViewModels service)
        {
            if (ModelState.IsValid)
            {
                if (ValidateModel(service, ModelState))
                {
                    if (ValidateModel(service, ModelState))
                    {
                        SchedulerViewModels sheduler = new SchedulerViewModels { 
                            Id = service.Id,
                            Description = service.Description,
                            selectedStatus = service.selectedStatus,
                            Start = service.Start,
                            End = service.End
                        };
                        int? scheduleStatus = db.OutputSchedules.Find(service.Id).Status;
                        if (scheduleStatus != null)
                        {
                            if (ValidateServiceTableClass.ValidateStatus(sheduler, ModelState, (int)scheduleStatus))
                            {
                                if (string.IsNullOrEmpty(service.Title))
                                {
                                    service.Title = "";
                                }
                                var entity = db.OutputSchedules.FirstOrDefault(m => m.Id == service.Id);
                                entity.TargetDate = service.Start;
                                entity.HandOnDate = service.End;
                                entity.ReviseDate = service.ReviseDate;
                                entity.ReviseCount = service.ReviseCount;
                                entity.Status = service.selectedStatus;
                                db.Entry(entity).State = EntityState.Modified;
                                db.SaveChanges();
                            }
                        }
                    }
                   
                }
            }

            return Json(new[] { service }.ToDataSourceResult(request, ModelState));
        }

        private bool ValidateModel(OutputSchedulerViewModels service, ModelStateDictionary modelState)
        {
            if (service.Start > service.End)
            {
                modelState.AddModelError("errors", "End date must be greater or equal to Start date.");
                return false;
            }

            if (service.ReviseDate < service.End)
            {
                modelState.AddModelError("errors", "Revise date must be greater or equal to End date.");
                return false;
            }

            return true;
        }

        public ActionResult DetailsOutput(int? OutputScheduleId)
        {
            //ViewBag.EquipmentSetId = equipmentId;

            //Generate Service Details
            //Getting Information from Service
            //ServiceFormId => schedule for equipment
            if (OutputScheduleId != null)
            {
                //find ServiceForm Id from emp schedule id
                var serviceFormId = db.ServiceForms.Where(sv => sv.OutputSchedules.Any(eqps => eqps.Id == OutputScheduleId)).Select(s => s.Id).FirstOrDefault();
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
                    OutsourceId = OutputScheduleId,
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

        public PartialViewResult DescriptionOutput(int? outputScheduleId)
        {
            //Select Outsource ID
            if (outputScheduleId != null)
            {
                int outId = (int)outputScheduleId;
                var outputSchedule = db.OutputSchedules.Find(outId);
                var outputId = db.OutputSchedules.Where(os => os.Id == outId).FirstOrDefault().OutputServiceId;
                var output = db.OutputServices.Find(outputId);
                OutputInformation outputInfo = new OutputInformation
                {
                    Id = output.Id,
                    OutputName = output.Name,
                    Description = output.Description,
                    OutputSize = output.OutputSize.Name,
                    OutputType = output.OutputType.Name,
                    OutputURL = output.OutputURL,
                    Quantity = outputSchedule.OutputQuantity
                };
                return PartialView(outputInfo);
            }

            return PartialView();
        }
    }
}
