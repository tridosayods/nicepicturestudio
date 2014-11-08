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
    public class EquipmentSchedulesController : Controller
    {
        private NicePictureStudioDBEntities db = new NicePictureStudioDBEntities();

        // GET: EquipmentSchedules
        [OutputCache(Duration = 0)]
        public async Task<ActionResult> Index()
        {
            //Employee Management

            //Service Management
            var _equipmentTypeList = db.EquipmentServices.ToList();
            EquipmentService defaultEquipmentType = new EquipmentService { Id = Constant.DEFAULT, Name = "เลือกทั้งหมด" };
            _equipmentTypeList.Insert(0, defaultEquipmentType);

            //Status Management
            var _statusList = db.ServiceStatus.ToList();
            ServiceStatu defaultStatus = new ServiceStatu { Id = Constant.DEFAULT, StatusName = "เลือกทั้งหมด" };
            _statusList.Insert(0, defaultStatus);

            ViewBag.ServiceTypeList = new SelectList(_equipmentTypeList, "Id", "Name");
            ViewBag.StatusList = new SelectList(_statusList, "Id", "StatusName");
            return View(await db.EquipmentSchedules.ToListAsync());
        }

        // GET: EquipmentSchedules/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EquipmentSchedule equipmentSchedule = await db.EquipmentSchedules.FindAsync(id);
            if (equipmentSchedule == null)
            {
                return HttpNotFound();
            }
            return View(equipmentSchedule);
        }

        // GET: EquipmentSchedules/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: EquipmentSchedules/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,StartTime,EndTime,EquipmentId,EquipmentServiceId,Status")] EquipmentSchedule equipmentSchedule)
        {
            if (ModelState.IsValid)
            {
                db.EquipmentSchedules.Add(equipmentSchedule);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(equipmentSchedule);
        }

        // GET: EquipmentSchedules/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EquipmentSchedule equipmentSchedule = await db.EquipmentSchedules.FindAsync(id);
            if (equipmentSchedule == null)
            {
                return HttpNotFound();
            }
            return View(equipmentSchedule);
        }

        // POST: EquipmentSchedules/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,StartTime,EndTime,EquipmentId,EquipmentServiceId,Status")] EquipmentSchedule equipmentSchedule)
        {
            if (ModelState.IsValid)
            {
                db.Entry(equipmentSchedule).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(equipmentSchedule);
        }

        // GET: EquipmentSchedules/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EquipmentSchedule equipmentSchedule = await db.EquipmentSchedules.FindAsync(id);
            if (equipmentSchedule == null)
            {
                return HttpNotFound();
            }
            return View(equipmentSchedule);
        }

        // POST: EquipmentSchedules/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            EquipmentSchedule equipmentSchedule = await db.EquipmentSchedules.FindAsync(id);
            db.EquipmentSchedules.Remove(equipmentSchedule);
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

        public PartialViewResult EquipmentScheduler(string photographId, int? serviceTypeId, int? statusId, bool? isConfirm, bool? isNotFinish)
        {
            ViewBag.PhotographerId = photographId == string.Empty ? Constant.UNDEFINED : photographId;
            ViewBag.ServiceTypeId = serviceTypeId == null ? Constant.DEFAULT : serviceTypeId;
            ViewBag.Status = statusId == null ? Constant.DEFAULT : statusId;
            ViewBag.IsConfirm = isConfirm == null ? false : isConfirm;
            ViewBag.IsNotFinish = isNotFinish == null ? false : isNotFinish;
            return PartialView();
        }

        public PartialViewResult GetRemainNumberFromDatabase(int? id)
        {
            var equipmentSchedule = db.EquipmentSchedules.Find(id);
            int remainNumber = 0;
            if (equipmentSchedule != null)
            {
                remainNumber = Convert.ToInt32(equipmentSchedule.Remain);
            }
            return PartialView(remainNumber);
        }

        public virtual JsonResult Equipments_Read([DataSourceRequest] DataSourceRequest request, string phothgraphId, int serviceTypeId,int? statusId, bool? isConfirm, bool? isNotFinish)
        {
            IQueryable<EquipmentSchedulerViewModel> tasks = CreateEquipmentSchedules(phothgraphId, serviceTypeId, statusId, isConfirm, isNotFinish).Select(task => new EquipmentSchedulerViewModel()
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
                RemainItem = task.RemainItem,
                Quantity = task.Quantity,
                EquipmentSetIndex = task.EquipmentSetIndex
            }
            ).AsQueryable();
            return Json(tasks.ToDataSourceResult(request),JsonRequestBehavior.AllowGet);
        }

        
        public virtual JsonResult Equipments_Update([DataSourceRequest] DataSourceRequest request, EquipmentSchedulerViewModel service)
        {
            //int statusConfirm  =2;
            //int statusNew = 1;
            //int statusCancel = 3;
            List<EquipmentSchedulerViewModel> lstEquipmentServiceItems = new List<EquipmentSchedulerViewModel>();
            if (ModelState.IsValid)
            {
                if (ValidateModel(service, ModelState))
                {
                    if (string.IsNullOrEmpty(service.Title))
                    {
                        service.Title = "";
                    }

                    var entity = db.EquipmentSchedules.FirstOrDefault(m => m.Id == service.Id);
                    //var matchEntityAtTheSameTime = (from schedule in db.EquipmentSchedules
                    //                                where (schedule.StartTime >= entity.StartTime && schedule.StartTime <= entity.EndTime) && (schedule.EquipmentId == entity.EquipmentId)
                    //                                select (schedule)).ToList();
                    //matchEntityAtTheSameTime.Remove(entity);
                   
                    //if (matchEntityAtTheSameTime != null)
                    //{
                    //    if (service.selectedStatus == statusConfirm && (entity.Status == statusNew || entity.Status == statusCancel))
                    //    {
                    //        // reduce remain 
                    //        // select item at the same schedule to reduce remain number
                    //        // Find the number of service which is count as confirm
                    //        int numberOfConfirmService = matchEntityAtTheSameTime.Count(confirm => confirm.Status == 2);
                    //        int remainQuantity = service.Quantity - numberOfConfirmService - 1;
                    //        foreach (var eqpItem in matchEntityAtTheSameTime)
                    //        {
                    //            var entityChange = db.EquipmentSchedules.Find(eqpItem.Id);
                    //            entityChange.Remain = remainQuantity;
                    //            db.Entry(entityChange).State = EntityState.Modified;

                    //            var changeItem = GenerateEquipmentModel(entityChange);
                    //            lstEquipmentServiceItems.Add(changeItem);
                    //        }
                    //        entity.Remain = remainQuantity;
                    //        service.RemainItem = remainQuantity;
                    //    }
                    //    else if ((service.selectedStatus == statusNew && entity.Status == statusConfirm) || (service.selectedStatus == statusCancel && entity.Status == statusConfirm))
                    //    {
                    //        //increase remain
                    //        //select item at the same schedule to increase remain number
                    //        int remainQuantity = service.RemainItem + 1;
                    //        foreach (var eqpItem in matchEntityAtTheSameTime)
                    //        {
                    //            var entityChange = db.EquipmentSchedules.Find(eqpItem.Id);
                    //            entityChange.Remain = remainQuantity;
                    //            db.Entry(entityChange).State = EntityState.Modified;

                    //            var changeItem = GenerateEquipmentModel(entityChange);
                    //            lstEquipmentServiceItems.Add(changeItem);
                    //        }
                    //        entity.Remain = remainQuantity;
                    //        service.RemainItem = remainQuantity;
                    //    }
                    //}

                    entity.StartTime = service.Start;
                    entity.EndTime = service.End;
                    entity.Status = service.selectedStatus;
                    db.Entry(entity).State = EntityState.Modified;
                    db.SaveChanges();
                }
            }
            //lstEquipmentServiceItems.Add(new[] { service }.ToDataSourceResult(request, ModelState));
            lstEquipmentServiceItems.Add(service);
            lstEquipmentServiceItems.Reverse();
            //return Json(new[]{
            //    {new[] { service,xx }.ToDataSourceResult(request, ModelState)},
            //    {new[] { xx }.ToDataSourceResult(request, ModelState)}
            //});
            ViewBag.Id = service.Id;
            return Json(lstEquipmentServiceItems.AsQueryable().ToDataSourceResult(request));
            //return Json(lstEquipmentServiceItems);
            //return Json(tasks.ToDataSourceResult(request));
            //return Json(new[] { service }.ToDataSourceResult(request, ModelState));
        }

        private bool ValidateModel(EquipmentSchedulerViewModel service, ModelStateDictionary modelState)
        {
            if (service.Start > service.End)
            {
                modelState.AddModelError("errors", "End date must be greater or equal to Start date.");
                return false;
            }

            return true;
        }

        private EquipmentSchedulerViewModel GenerateEquipmentModel(EquipmentSchedule entity)
        {
            int EquipmentStatusVacant = 1;
            var Equipment = db.Equipments.Where(eqp => eqp.EquipmentId == entity.EquipmentId).FirstOrDefault();
                                
            EquipmentSchedulerViewModel _eqpItem = new EquipmentSchedulerViewModel
            {
                Id = entity.Id,
                Title = Equipment.EquipmentName,
                Description = Equipment.EquipmentDetail,
                Start = entity.StartTime,
                End = entity.EndTime,
                selectedStatus = entity.Status,
                Quantity = Equipment.Quantity,
                RemainItem = Convert.ToInt32(entity.Remain)
            };

            return _eqpItem;
        }

        public JsonResult EquipmentSets_Read([DataSourceRequest] DataSourceRequest request,int serviceId)
        {
            var eqpIndex = serviceId;
            var listEquipment = (from equipment in db.Equipments
                                 join eqset in db.EquipmentSets on equipment.EquipmentId equals eqset.Equipment.EquipmentId
                                 where (eqset.EquipmentService.Id == eqpIndex)
                                 select (equipment)).ToList();

            IQueryable<Equipment> tasks = listEquipment.Select(task => new Equipment()
            {
               EquipmentId = task.EquipmentId,
               EquipmentDetail = task.EquipmentDetail,
               EquipmentName = task.EquipmentName, 
               Quantity =task.Quantity,
               ModelName = task.ModelName
            }
            ).AsQueryable();
            return Json(tasks.ToDataSourceResult(request),JsonRequestBehavior.AllowGet);
        }

        private List<EquipmentSchedulerViewModel> CreateEquipmentSchedules(string photographId, int? serviceTypeId, int? statusId, bool? isConfirm, bool? isNotFinish)
        {
            int EquipmentStatusVacant = Constant.EQUIPMENT_STATUS_VACANT;
            List<EquipmentSchedulerViewModel> _listSchecule = new List<EquipmentSchedulerViewModel>();
            //var allEquipments = (from equipmentSchedule in db.EquipmentSchedules
            //                     join eqp in db.Equipments on equipmentSchedule.EquipmentId equals eqp.EquipmentId
            //                     where (eqp.EquipmentStatu.Id == EquipmentStatusVacant)
            //                     select new { eqpSchedule = equipmentSchedule, eqpItem = eqp }).ToList();
            var allEquipments = (from equipmentSchedule in db.EquipmentSchedules
                                 join eqp in db.EquipmentServices on equipmentSchedule.EquipmentServiceId equals eqp.Id
                                // where (eqp.EquipmentStatu.Id == EquipmentStatusVacant)
                                 select new { eqpSchedule = equipmentSchedule, eqpService = eqp }).ToList();

            //Add condition for filtering
            var filterServiceForms = allEquipments;
          
            if (serviceTypeId > 0 && statusId != null)
            {
                filterServiceForms = filterServiceForms.Where(s => s.eqpSchedule.EquipmentServiceId == serviceTypeId).ToList();
            }

            if (statusId > 0 && statusId != null)
            {
                filterServiceForms = filterServiceForms.Where(s => s.eqpSchedule.Status == statusId).ToList();
            }

            if (isConfirm != null)
            {
                if (isConfirm == true && statusId < Constant.SERVICE_STATUS_NEW)
                { filterServiceForms = filterServiceForms.Where(s => s.eqpSchedule.Status <= Constant.SERVICE_STATUS_CONFIRM).ToList(); }
            }

            if (isNotFinish != null)
            {
                if (isNotFinish == true && statusId < Constant.SERVICE_STATUS_NEW)
                {
                    var currentDate = DateTime.Now;
                    filterServiceForms = filterServiceForms.Where(s => (s.eqpSchedule.StartTime - currentDate).TotalDays > 3 && s.eqpSchedule.Status <= Constant.SERVICE_STATUS_CONFIRM).ToList();
                }
            }
            //Add condition for filtering

            foreach (var item in filterServiceForms)
            {
                if (item.eqpSchedule.ServiceForm != null)
                {
                    //var entity = db.EquipmentSchedules.FirstOrDefault(m => m.Id == item.eqpSchedule.Id);
                    //var matchEntityAtTheSameTime = (from schedule in db.EquipmentSchedules
                    //                                where (schedule.StartTime >= entity.StartTime && schedule.StartTime <= entity.EndTime) && (schedule.EquipmentId == entity.EquipmentId)
                    //                                select (schedule)).ToList();
                    //seek item list from Equipment Set at EquipmentId from Employee schedule
                    var eqpIndex = item.eqpSchedule.EquipmentId;
                    //var listEquipment = (from equipment in db.Equipments
                    //                     join eqset in db.EquipmentSets on equipment.EquipmentId equals eqset.Equipment.EquipmentId
                    //                     where (eqset.EquipmentService.Id == eqpIndex)
                    //                     select (equipment)).ToList();

                    EquipmentSchedulerViewModel _scheduler = new EquipmentSchedulerViewModel
                    {
                        Id = item.eqpSchedule.Id,
                        Title = item.eqpService.Name,
                        Description = item.eqpService.Description,
                        Start = item.eqpSchedule.StartTime,
                        End = item.eqpSchedule.EndTime,
                        selectedStatus = item.eqpSchedule.Status,
                        EquipmentSetIndex = eqpIndex
                        //Quantity = item.eqpService.Quantity,
                        //RemainItem = item.eqpItem.Quantity - matchEntityAtTheSameTime.Count
                    };
                   
                    _listSchecule.Add(_scheduler);
                }
            }
            return _listSchecule;
        }

        public ActionResult DetailsEquipment(int? equipmentId,int? empscheduleId)
        {
            ViewBag.EquipmentSetId = equipmentId;
            
            //Generate Service Details
            //Getting Information from Service
            //ServiceFormId => schedule for equipment
            if(empscheduleId != null)
            {
                //find ServiceForm Id from emp schedule id
                var serviceFormId = db.ServiceForms.Where(sv => sv.EquipmentSchedules.Any(eqps => eqps.Id == empscheduleId)).Select(s => s.Id).FirstOrDefault();
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
                        Name = employee.EmployeeInfoes.FirstOrDefault().Title +" " +
                                employee.EmployeeInfoes.FirstOrDefault().Name + employee.EmployeeInfoes.FirstOrDefault().Surname+ "("+
                                employee.EmployeeInfoes.FirstOrDefault().Nickname +")",
                        Position = employee.EmployeePositions.FirstOrDefault().Name
                    };
                    empPhotoGraph.Add(empDetail);
                }

                //Location
                var locationName =  "";
                var locationDetails = "";
                var Map = "";
                var location = services.ServiceForms.Where(s => s.Id == serviceFormId).Select(s => s.Locations).FirstOrDefault();
                if (location !=null)
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
                        var _location = db.Locations.Find(servicelocation.FirstOrDefault().LocationId);
                        locationName = _location.LocationName;
                        locationDetails = _location.MapExplanation;
                        Map = _location.MapURL;
                    }

                }
                else
                {
                    var servicelocation = services.ServiceForms.Where(s => s.Id == serviceFormId).Select(s => s.LocationSchedules).FirstOrDefault();
                    var _location = db.Locations.Find(servicelocation.FirstOrDefault().LocationId);
                    locationName = _location.LocationName;
                    locationDetails = _location.MapExplanation;
                    Map = _location.MapURL;
                }

                //Customer
                var bookingSpecialRequest = "";
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

                var suggestion = "";
                foreach (var item in services.ServiceSuggestions)
                {
                     if (suggestion =="")
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
                    MainPhotoGraph = empPhotoGraph.FirstOrDefault().Name,
                    Position = empPhotoGraph.FirstOrDefault().Position,
                    Bride = services.BrideName,
                    Groom = services.GroomName,
                    SpecialRequest = services.SpecialRequest,
                    Suggestion = suggestion,
                    Location = locationName,
                    LocationDetails = locationDetails,
                    Map = Map,
                    BookingCode = booking.BookingCode,
                    BookingRequest = bookingSpecialRequest
                };

                return PartialView(TableReport);
            }
            
            return PartialView();
        }
    }
}
