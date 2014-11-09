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
using Kendo.Mvc.Extensions;
using NicePictureStudio.Models;
using System.Collections;
using NicePictureStudio.Utils;

namespace NicePictureStudio
{
    public class ServiceFormsController : Controller
    {
        private NicePictureStudioDBEntities db = new NicePictureStudioDBEntities();

        // GET: ServiceForms
        public async Task<ActionResult> Index()
        {
            //Employee Management
            var _empList = db.Employees.Where(e => e.EmployeePositions.Any(em => em.Id == Constant.EMPLOYEE_POSITION_PHOTOGRAPH
                || em.Id == Constant.EMPLOYEE_POSITION_CAMERAMAN)).ToList();
            Employee defaultEmp = new Employee{
                Id ="",
                Name ="เลือกทั้งหมด"
            };
            _empList.Insert(0, defaultEmp);

            //Service Management
            var _serviceTypeList = db.ServiceTypes.ToList();
            ServiceType defaultServiceType = new ServiceType { Id=Constant.DEFAULT,ServiceTypeName = "เลือกทั้งหมด"};
            _serviceTypeList.Insert(0, defaultServiceType);

            //Status Management
            var _statusList = db.ServiceStatus.ToList();
            ServiceStatu defaultStatus = new ServiceStatu { Id=Constant.DEFAULT,StatusName ="เลือกทั้งหมด"};
            _statusList.Insert(0, defaultStatus);

            ViewBag.EmployeeList = new SelectList(_empList, "Id", "Name");
            ViewBag.ServiceTypeList = new SelectList(_serviceTypeList, "Id", "ServiceTypeName");
            ViewBag.StatusList = new SelectList(_statusList, "Id", "StatusName");
            return View(await db.ServiceForms.ToListAsync());
        }

        // GET: ServiceForms/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ServiceForm serviceForm = await db.ServiceForms.FindAsync(id);
            if (serviceForm == null)
            {
                return HttpNotFound();
            }
            return View(serviceForm);
        }

        // GET: ServiceForms/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ServiceForms/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,Name,EventStart,EventEnd,GuestsNumber,ServiceCost,ServicePrice")] ServiceForm serviceForm)
        {
            if (ModelState.IsValid)
            {
                db.ServiceForms.Add(serviceForm);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(serviceForm);
        }

        // GET: ServiceForms/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ServiceForm serviceForm = await db.ServiceForms.FindAsync(id);
            if (serviceForm == null)
            {
                return HttpNotFound();
            }
            return View(serviceForm);
        }

        // POST: ServiceForms/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,Name,EventStart,EventEnd,GuestsNumber,ServiceCost,ServicePrice")] ServiceForm serviceForm)
        {
            if (ModelState.IsValid)
            {
                db.Entry(serviceForm).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(serviceForm);
        }

        // GET: ServiceForms/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ServiceForm serviceForm = await db.ServiceForms.FindAsync(id);
            if (serviceForm == null)
            {
                return HttpNotFound();
            }
            return View(serviceForm);
        }

        // POST: ServiceForms/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            ServiceForm serviceForm = await db.ServiceForms.FindAsync(id);
            db.ServiceForms.Remove(serviceForm);
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

        public PartialViewResult ServiceFormScheduler(string photographId,int? serviceTypeId, int? statusId,bool? isConfirm, bool? isNotFinish )
        {
            ViewBag.PhotographerId = photographId == string.Empty ? Constant.UNDEFINED : photographId;
            ViewBag.ServiceTypeId = serviceTypeId == null ? Constant.DEFAULT : serviceTypeId;
            ViewBag.Status = statusId == null ? Constant.DEFAULT : statusId;
            ViewBag.IsConfirm = isConfirm == null ? false : isConfirm;
            ViewBag.IsNotFinish = isNotFinish == null ? false : isNotFinish;
            return PartialView();
        }

        public virtual JsonResult Services_Read([DataSourceRequest] DataSourceRequest request, string phothgraphId, int serviceTypeId,int? statusId, bool? isConfirm, bool? isNotFinish)
        {
            IQueryable<SchedulerViewModels> tasks = CreateSchedulerServiceForm(phothgraphId, serviceTypeId, statusId,isConfirm,isNotFinish).Select(task => new SchedulerViewModels()
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
                PhotoGraphStatus = task.PhotoGraphStatus,
                EquipmentStatus = task.EquipmentStatus,
                LocationStatus = task.LocationStatus,
                OutsourceStatus = task.OutsourceStatus,
                OutputStatus = task.OutputStatus
            }
            ).AsQueryable();
            return Json(tasks.ToDataSourceResult(request));
        }

        #region
        private List<SchedulerViewModels> createListProjection()
        {
            List<ServiceStatusViewModel> _status = new List<ServiceStatusViewModel> { 
                new ServiceStatusViewModel {
                    Id = 1,Name ="New",Description = "..."
                },
                new ServiceStatusViewModel {
                    Id = 2,Name ="Confirm",Description = "..."
                }
            };
            // IEnumerable<SelectList> status = (new SelectList(_status, "Id", "Name"));
            List<SchedulerViewModels> cinemaSchedule = new List<SchedulerViewModels> {
        new SchedulerViewModels {
            Id = 1,
            Title = "Fast and furious 6",
            Start = new DateTime(2013,7,13,17,00,00),
            End= new DateTime(2013,7,13,18,30,00),
            selectedStatus =2
        },
        new SchedulerViewModels {
            Id =2,
            Title= "The Internship",
            Start= new DateTime(2013,6,13,14,00,00),
            End= new DateTime(2013,6,13,15,30,00),
            selectedStatus =3
        },
        new SchedulerViewModels {
            Id=3,
            Title = "The Perks of Being a Wallflower",
            Start =  new DateTime(2013,6,13,16,00,00),
            End =  new DateTime(2013,6,13,17,30,00),
            selectedStatus =1
        }};
            return cinemaSchedule;
        }
        #endregion

        private List<SchedulerViewModels> CreateSchedulerServiceForm(string photographId, int? serviceTypeId, int? statusId, bool? isConfirm, bool? isNotFinish)
        {
            List<SchedulerViewModels> _listSchecule = new List<SchedulerViewModels>();
            //Add condition for filtering
            var filterServiceForms = db.ServiceForms.ToList();
            if (photographId != Constant.UNDEFINED && photographId != null)
            {
                filterServiceForms = filterServiceForms.Where(s => s.EmployeeSchedules.Any(e => e.Employee.Id == photographId)).Select(s => s).ToList();
            }

            if (serviceTypeId >0 && statusId !=null)
            {
                filterServiceForms = filterServiceForms.Where(s => s.ServiceType.Id == serviceTypeId).ToList();
            }

            if (statusId > 0 && statusId !=null)
            {
                filterServiceForms = filterServiceForms.Where(s => s.ServiceStatu.Id == statusId).ToList();
            }

            if (isConfirm != null)
            {
                if (isConfirm == true && statusId < Constant.SERVICE_STATUS_NEW)
                { filterServiceForms = filterServiceForms.Where(s => s.ServiceStatu.Id <= Constant.SERVICE_STATUS_CONFIRM).ToList(); }
            }

            if (isNotFinish != null)
            {
                if (isNotFinish == true && statusId < Constant.SERVICE_STATUS_NEW)
                { 
                    var currentDate = DateTime.Now;
                    filterServiceForms = filterServiceForms.Where(s => (s.EventStart - currentDate).TotalDays > 3 && s.ServiceStatu.Id <= Constant.SERVICE_STATUS_CONFIRM).ToList();
                }
            }
            //Add condition for filtering
           // var allServiceForms = db.ServiceForms.ToList();
            var allServiceForms = filterServiceForms;

            foreach (var item in allServiceForms)
            {
                //Getting All of Status from every Schedules
                var PhotoStatus = "";
                var EquipmentStatus = "";
                var LocationStatus = "";
                var OutsourceStatus = "";
                var OutputStatus = "";

               //Assing OverStatus 
                PhotoStatus = AnalysisServiceFormStatus(item, Constant.SERVICE_PHOTOGRAPH);
                EquipmentStatus= AnalysisServiceFormStatus(item, Constant.SERVICE_EQUIPMENT);
                LocationStatus = AnalysisServiceFormStatus(item, Constant.SERVICE_LOCATION);
                OutsourceStatus = AnalysisServiceFormStatus(item, Constant.SERVICE_OUTSOURCE);
                OutputStatus = AnalysisServiceFormStatus(item, Constant.SERVICE_OUTPUT);

                SchedulerViewModels _scheduler = new SchedulerViewModels { 
                    Id = item.Id,
                    Title = item.ServiceType.ServiceTypeName,
                    Description = item.Name,
                    Start = item.EventStart,
                    End = item.EventEnd,
                    selectedStatus = item.ServiceStatu.Id,
                    PhotoGraphStatus = PhotoStatus,
                    EquipmentStatus = EquipmentStatus,
                    LocationStatus = LocationStatus,
                    OutsourceStatus = OutsourceStatus,
                    OutputStatus  = OutputStatus
                };
                _listSchecule.Add(_scheduler);
            }
            return _listSchecule;
        }


        private string AnalysisServiceFormStatus(ServiceForm serviceFrom,int serviceType)
        {

            if(serviceType==Constant.SERVICE_PHOTOGRAPH)
            {
                if (serviceFrom.EmployeeSchedules.All(emps => emps.Status <= Constant.SERVICE_STATUS_NEW))
                {
                    return Constant.SERVICE_FORM_STATUS_NEW;
                }
                else if (serviceFrom.EmployeeSchedules.All(emps => emps.Status == Constant.SERVICE_STATUS_CONFIRM))
                {
                    return Constant.SERVICE_FORM_STATUS_CONFIRM;
                }
                else if (serviceFrom.EmployeeSchedules.All(emps => emps.Status <= Constant.SERVICE_STATUS_CONFIRM))
                {
                    return Constant.SERVICE_FORM_STATUS_PARTIAL_NEW;
                }
                else if (serviceFrom.EmployeeSchedules.All(emps => emps.Status == Constant.SERVICE_STATUS_COMPLETE))
                {
                    return Constant.SERVICE_FORM_STATUS_FINISH;
                }
                else if (serviceFrom.EmployeeSchedules.All(emps => emps.Status <= Constant.SERVICE_STATUS_COMPLETE))
                {
                    return Constant.SERVICE_FORM_STATUS_PARTIAL_FINISH;
                }
                else if (serviceFrom.EmployeeSchedules.All(emps => emps.Status == Constant.SERVICE_STATUS_CANCEL))
                {
                    return Constant.SERVICE_FORM_STATUS_CANCEL;
                }
                else if (serviceFrom.EmployeeSchedules.All(emps => emps.Status <= Constant.SERVICE_STATUS_CANCEL))
                {
                    return Constant.SERVICE_FORM_STATUS_PARTIAL_CANCEL;
                }
                else
                {
                    return string.Empty;
                }
            }
            else if (serviceType == Constant.SERVICE_EQUIPMENT)
            {
                if (serviceFrom.EquipmentSchedules.All(emps => emps.Status <= Constant.SERVICE_STATUS_NEW))
                {
                    return Constant.SERVICE_FORM_STATUS_NEW;
                }
                else if (serviceFrom.EquipmentSchedules.All(emps => emps.Status == Constant.SERVICE_STATUS_CONFIRM))
                {
                    return Constant.SERVICE_FORM_STATUS_CONFIRM;
                }
                else if (serviceFrom.EquipmentSchedules.All(emps => emps.Status <= Constant.SERVICE_STATUS_CONFIRM))
                {
                    return Constant.SERVICE_FORM_STATUS_PARTIAL_NEW;
                }
                else if (serviceFrom.EquipmentSchedules.All(emps => emps.Status == Constant.SERVICE_STATUS_COMPLETE))
                {
                    return Constant.SERVICE_FORM_STATUS_FINISH;
                }
                else if (serviceFrom.EquipmentSchedules.All(emps => emps.Status <= Constant.SERVICE_STATUS_COMPLETE))
                {
                    return Constant.SERVICE_FORM_STATUS_PARTIAL_FINISH;
                }
                else if (serviceFrom.EquipmentSchedules.All(emps => emps.Status == Constant.SERVICE_STATUS_CANCEL))
                {
                    return Constant.SERVICE_FORM_STATUS_CANCEL;
                }
                else if (serviceFrom.EquipmentSchedules.All(emps => emps.Status <= Constant.SERVICE_STATUS_CANCEL))
                {
                    return Constant.SERVICE_FORM_STATUS_PARTIAL_CANCEL;
                }
                else
                {
                    return string.Empty;
                }
            }
            else if (serviceType == Constant.SERVICE_LOCATION)
            {
                if (serviceFrom.LocationSchedules.All(emps => emps.Status <= Constant.SERVICE_STATUS_NEW))
                {
                    return Constant.SERVICE_FORM_STATUS_NEW;
                }
                else if (serviceFrom.LocationSchedules.All(emps => emps.Status == Constant.SERVICE_STATUS_CONFIRM))
                {
                    return Constant.SERVICE_FORM_STATUS_CONFIRM;
                }
                else if (serviceFrom.LocationSchedules.All(emps => emps.Status <= Constant.SERVICE_STATUS_CONFIRM))
                {
                    return Constant.SERVICE_FORM_STATUS_PARTIAL_NEW;
                }
                else if (serviceFrom.LocationSchedules.All(emps => emps.Status == Constant.SERVICE_STATUS_COMPLETE))
                {
                    return Constant.SERVICE_FORM_STATUS_FINISH;
                }
                else if (serviceFrom.LocationSchedules.All(emps => emps.Status <= Constant.SERVICE_STATUS_COMPLETE))
                {
                    return Constant.SERVICE_FORM_STATUS_PARTIAL_FINISH;
                }
                else if (serviceFrom.LocationSchedules.All(emps => emps.Status == Constant.SERVICE_STATUS_CANCEL))
                {
                    return Constant.SERVICE_FORM_STATUS_CANCEL;
                }
                else if (serviceFrom.LocationSchedules.All(emps => emps.Status <= Constant.SERVICE_STATUS_CANCEL))
                {
                    return Constant.SERVICE_FORM_STATUS_PARTIAL_CANCEL;
                }
                else
                {
                    return string.Empty;
                }
            }
            else if (serviceType == Constant.SERVICE_OUTSOURCE)
            {
                if (serviceFrom.OutsourceSchedules.All(emps => emps.Status <= Constant.SERVICE_STATUS_NEW))
                {
                    return Constant.SERVICE_FORM_STATUS_NEW;
                }
                else if (serviceFrom.OutsourceSchedules.All(emps => emps.Status == Constant.SERVICE_STATUS_CONFIRM))
                {
                    return Constant.SERVICE_FORM_STATUS_CONFIRM;
                }
                else if (serviceFrom.OutsourceSchedules.All(emps => emps.Status <= Constant.SERVICE_STATUS_CONFIRM))
                {
                    return Constant.SERVICE_FORM_STATUS_PARTIAL_NEW;
                }
                else if (serviceFrom.OutsourceSchedules.All(emps => emps.Status == Constant.SERVICE_STATUS_COMPLETE))
                {
                    return Constant.SERVICE_FORM_STATUS_FINISH;
                }
                else if (serviceFrom.OutsourceSchedules.All(emps => emps.Status <= Constant.SERVICE_STATUS_COMPLETE))
                {
                    return Constant.SERVICE_FORM_STATUS_PARTIAL_FINISH;
                }
                else if (serviceFrom.OutsourceSchedules.All(emps => emps.Status == Constant.SERVICE_STATUS_CANCEL))
                {
                    return Constant.SERVICE_FORM_STATUS_CANCEL;
                }
                else if (serviceFrom.OutsourceSchedules.All(emps => emps.Status <= Constant.SERVICE_STATUS_CANCEL))
                {
                    return Constant.SERVICE_FORM_STATUS_PARTIAL_CANCEL;
                }
                else
                {
                    return string.Empty;
                }
            }
            else if (serviceType == Constant.SERVICE_OUTPUT)
            {
                if (serviceFrom.OutputSchedules.All(emps => emps.Status <= Constant.SERVICE_STATUS_NEW))
                {
                    return Constant.SERVICE_FORM_STATUS_NEW;
                }
                else if (serviceFrom.OutputSchedules.All(emps => emps.Status == Constant.SERVICE_STATUS_CONFIRM))
                {
                    return Constant.SERVICE_FORM_STATUS_CONFIRM;
                }
                else if (serviceFrom.OutputSchedules.All(emps => emps.Status <= Constant.SERVICE_STATUS_CONFIRM))
                {
                    return Constant.SERVICE_FORM_STATUS_PARTIAL_NEW;
                }
                else if (serviceFrom.OutputSchedules.All(emps => emps.Status == Constant.SERVICE_STATUS_COMPLETE))
                {
                    return Constant.SERVICE_FORM_STATUS_FINISH;
                }
                else if (serviceFrom.OutputSchedules.All(emps => emps.Status <= Constant.SERVICE_STATUS_COMPLETE))
                {
                    return Constant.SERVICE_FORM_STATUS_PARTIAL_FINISH;
                }
                else if (serviceFrom.OutputSchedules.All(emps => emps.Status == Constant.SERVICE_STATUS_CANCEL))
                {
                    return Constant.SERVICE_FORM_STATUS_CANCEL;
                }
                else if (serviceFrom.OutputSchedules.All(emps => emps.Status <= Constant.SERVICE_STATUS_CANCEL))
                {
                    return Constant.SERVICE_FORM_STATUS_PARTIAL_CANCEL;
                }
                else
                {
                    return string.Empty;
                }
            }
            else
            {
                return string.Empty;
            }

        }

        public virtual JsonResult Services_Update([DataSourceRequest] DataSourceRequest request, SchedulerViewModels service)
        {
            if (ModelState.IsValid)
            {
                if (ValidateModel(service,ModelState))
                {
                    //validate before update Status
                    if(ValidateStatus(service,ModelState))
                    {
                        if (string.IsNullOrEmpty(service.Title))
                        {
                            service.Title = "";
                        }

                        var entity = db.ServiceForms.FirstOrDefault(m => m.Id == service.Id);
                        var serviceStatus = db.ServiceStatus.FirstOrDefault(s => s.Id == service.selectedStatus);
                        entity.EventStart = service.Start;
                        entity.EventEnd = service.End;
                        entity.ServiceStatu = serviceStatus;

                        //Cancel All Related Task
                        if (service.selectedStatus == Constant.SERVICE_STATUS_CANCEL)
                        {
                            foreach (var cancelItem in entity.EmployeeSchedules)
                            {
                                cancelItem.Status = Constant.SERVICE_STATUS_CANCEL;
                            }

                            foreach (var cancelItem in entity.EquipmentSchedules)
                            {
                                cancelItem.Status = Constant.SERVICE_STATUS_CANCEL;
                            }

                            foreach (var cancelItem in entity.LocationSchedules)
                            {
                                cancelItem.Status = Constant.SERVICE_STATUS_CANCEL;
                            }

                            foreach (var cancelItem in entity.OutsourceSchedules)
                            {
                                cancelItem.Status = Constant.SERVICE_STATUS_CANCEL;
                            }

                            foreach (var cancelItem in entity.OutputSchedules)
                            {
                                cancelItem.Status = Constant.SERVICE_STATUS_CANCEL;
                            }
                        }
                        else if (service.selectedStatus == Constant.SERVICE_STATUS_CANCEL_IN7DAYS)
                        {
                            foreach (var cancelItem in entity.EmployeeSchedules)
                            {
                                cancelItem.Status = Constant.SERVICE_STATUS_CANCEL_IN7DAYS;
                            }

                            foreach (var cancelItem in entity.EquipmentSchedules)
                            {
                                cancelItem.Status = Constant.SERVICE_STATUS_CANCEL_IN7DAYS;
                            }

                            foreach (var cancelItem in entity.LocationSchedules)
                            {
                                cancelItem.Status = Constant.SERVICE_STATUS_CANCEL_IN7DAYS;
                            }

                            foreach (var cancelItem in entity.OutsourceSchedules)
                            {
                                cancelItem.Status = Constant.SERVICE_STATUS_CANCEL_IN7DAYS;
                            }

                            foreach (var cancelItem in entity.OutputSchedules)
                            {
                                cancelItem.Status = Constant.SERVICE_STATUS_CANCEL_IN7DAYS;
                            }
                        }

                        db.Entry(entity).State = EntityState.Modified;
                        db.SaveChanges();

                    }
                   
                }
            }

            return Json(new[] { service }.ToDataSourceResult(request, ModelState));
        }

        private bool ValidateStatus(SchedulerViewModels service, ModelStateDictionary modelState)
        {
            if (service.selectedStatus == Constant.SERVICE_STATUS_CONFIRM)
            {
                if (service.EquipmentStatus == Constant.SERVICE_FORM_STATUS_CONFIRM
                    && service.PhotoGraphStatus == Constant.SERVICE_FORM_STATUS_CONFIRM
                    && service.LocationStatus == Constant.SERVICE_FORM_STATUS_CONFIRM && service.OutsourceStatus == Constant.SERVICE_FORM_STATUS_CONFIRM)
                {
                    return true;
                }
                else
                {
                    modelState.AddModelError("ผิดพลาด", "ยังมีบางรายการที่มีสถานะการให้บริการยังไม่ยืนยันให้ครบ โปรดตรวจสอบ สถานะช่างถ่ายภาพ สถานะพนักงานอุปกรณ์ถ่ายภาพ สถานะของสถานที่ให้บริการ หรือ สถานะของผู้ให้บริการจัดจ้าง");
                    return false;
                }
            }
            else if (service.selectedStatus == Constant.SERVICE_STATUS_COMPLETE)
            { 
                if (service.EquipmentStatus == Constant.SERVICE_FORM_STATUS_FINISH
                    && service.PhotoGraphStatus == Constant.SERVICE_FORM_STATUS_FINISH
                    && service.LocationStatus == Constant.SERVICE_FORM_STATUS_FINISH 
                    && service.OutsourceStatus == Constant.SERVICE_FORM_STATUS_FINISH
                    && service.OutputStatus == Constant.SERVICE_FORM_STATUS_FINISH)
                {
                    return true;
                }
                else
                {
                     modelState.AddModelError("ผิดพลาด", "ยังมีบางรายการที่มีสถานะการให้บริการยังไม่เสร็จสิ้นการให้บริการ โปรดตรวจสอบ สถานะช่างถ่ายภาพ สถานะพนักงานอุปกรณ์ถ่ายภาพ สถานะของสถานที่ให้บริการ สถานะของผู้ให้บริการจัดจ้าง หรือ พนักงานฝ่ายมัลติมีเดีย");
                    return false;
                }
                
            }
            else if (service.selectedStatus == Constant.SERVICE_STATUS_CANCEL)
            {
                var now = DateTime.Now.AddDays(7);
                if (service.Start.CompareTo(now) >= 0)
                {
                    return true;
                }
                else
                {
                    modelState.AddModelError("คำเตือน", "ไม่สามารถยกเลิกแบบไม่เสียค่ามัดจำ ได้เนื่องจากระยะเวลาในการให้บริการจะถึงภายในน้อยกว่า 7 วันทำการ จากเวลาปัจจุบัน");
                    return false;
                }
            }
            else if (service.selectedStatus == Constant.SERVICE_STATUS_CANCEL_IN7DAYS)
            {
                var now = DateTime.Now.AddDays(7);
                if (service.Start.CompareTo(now) <= 0)
                {
                    return true;
                }
                else
                {
                    modelState.AddModelError("คำเตือน", "ไม่สามารถยกเลิกแบบหักค้ามัดจำ 50% ได้เนื่องจากระยะเวลาในการให้บริการมีมากกว่า 7 วันทำการ จากเวลาปัจจุบัน");
                    return false;
                }
            }
            return true;
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

        public ActionResult DetailsServiceForm(int? ServiceFormScheduleId)
        {
            //ViewBag.EquipmentSetId = equipmentId;

            //Generate Service Details
            //Getting Information from Service
            //ServiceFormId => schedule for equipment
            if (ServiceFormScheduleId != null)
            {
                //find ServiceForm Id from emp schedule id
                var serviceFormId = ServiceFormScheduleId;
                //var serviceFormId = db.ServiceForms.Where(sv => sv.Any(eqps => eqps.Id == ServiceFormScheduleId)).Select(s => s.Id).FirstOrDefault();
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
                    OutsourceId = ServiceFormScheduleId,
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

        public PartialViewResult DescriptionServiceForm(int? serviceFormId, List<EmployeeDetails> lstEmp)
        {
            var _reportItems = new List<ServiceFormWithAllRelatedServicesInfo>();
            if (serviceFormId != null)
            {
                var entityServiceForm = db.ServiceForms.Find(serviceFormId);
                var entityEmpshcedules = entityServiceForm.EmployeeSchedules;
                var entityEquipments = entityServiceForm.EquipmentSchedules;
                var entityLocations = entityServiceForm.LocationSchedules;
                var entityOutsources = entityServiceForm.OutsourceSchedules;
                var entityOutputs = entityServiceForm.OutputSchedules;

                foreach (var item in entityEmpshcedules)
                {
                    var _reportItem = new ServiceFormWithAllRelatedServicesInfo 
                    {
                        Name = item.Employee.EmployeeInfoes.FirstOrDefault().Title + " " + item.Employee.EmployeeInfoes.FirstOrDefault().Name + " " + item.Employee.EmployeeInfoes.FirstOrDefault().Surname + " (" + item.Employee.EmployeeInfoes.FirstOrDefault().Nickname+")",
                        ServiceTypeName = "บริการช่างถ่ายภาพ",
                        EventStart = item.StartTime,
                        EventEnd = item.EndTime,
                        Status = db.ServiceStatus.Find(item.Status).StatusName
                    };
                    _reportItems.Add(_reportItem);
                }

                foreach (var item in entityEquipments)
                {
                    var _reportItem = new ServiceFormWithAllRelatedServicesInfo
                    {
                        Name = db.EquipmentServices.Find(item.EquipmentServiceId).Name,
                        ServiceTypeName = "บริการอุปกรณ์ถ่ายภาพ",
                        EventStart = item.StartTime,
                        EventEnd = item.EndTime,
                        Status = db.ServiceStatus.Find(item.Status).StatusName
                    };
                    _reportItems.Add(_reportItem);
                }

                foreach (var item in entityLocations)
                {
                    var _reportItem = new ServiceFormWithAllRelatedServicesInfo
                    {
                        Name = db.LocationServices.Find(item.LocationServiceId).Name,
                        ServiceTypeName = "บริการสถานที่ถ่ายภาพ",
                        EventStart = item.StartTime,
                        EventEnd = item.EndTime,
                        Status = db.ServiceStatus.Find(item.Status).StatusName
                    };
                    _reportItems.Add(_reportItem);
                }

                foreach (var item in entityOutsources)
                {
                    var _reportItem = new ServiceFormWithAllRelatedServicesInfo
                    {
                        Name = db.OutsourceServices.Find(item.OutsourceServiceId).Name,
                        ServiceTypeName = "บริการจัดจ้าง",
                        EventStart = item.StartTime,
                        EventEnd = item.EndTime,
                        Status = db.ServiceStatus.Find(item.Status).StatusName
                    };
                    _reportItems.Add(_reportItem);
                }

                foreach (var item in entityOutputs)
                {
                    var _reportItem = new ServiceFormWithAllRelatedServicesInfo
                    {
                        Name = db.OutputServices.Find(item.OutputServiceId).Name + "จำนวน (" + item.OutputQuantity +")",
                        ServiceTypeName = "บริการชิ้นงานถ่ายภาพ",
                        EventStart = (DateTime)item.TargetDate,
                        EventEnd = item.HandOnDate,
                        Status = db.ServiceStatus.Find(item.Status).StatusName
                    };
                    _reportItems.Add(_reportItem);
                }
            }

            ViewBag.ServiceFormItems = _reportItems;
            //Select Outsource ID
            //ViewBag.PhotoGrapherList = new List<EmployeeDetails>(lstEmp);
            //if (serviceFormId != null)
            //{
            //    var serviceForm = db.EmployeeSchedules.Find(serviceFormId).ServiceForm;
            //    var photoServiceId = db.EmployeeSchedules.Find(serviceFormId).EmployeeServiceId;
            //    var photoGraphService = db.PhotographServices.Find(photoServiceId);
            //    PhotographInfo photoItem = new PhotographInfo
            //    {
            //        Numphotographer = photoGraphService.PhotographerNumber,
            //        NumCameraman = photoGraphService.CameraManNumber,
            //        GuestsNumber = serviceForm.GuestsNumber,
            //        Description = photoGraphService.Name
            //    };
            //    return PartialView(photoItem);
            //}

            return PartialView();
        }



    }

    
}


