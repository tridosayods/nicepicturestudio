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

namespace NicePictureStudio
{
    public class EquipmentSchedulesController : Controller
    {
        private NicePictureStudioDBEntities db = new NicePictureStudioDBEntities();

        // GET: EquipmentSchedules
        [OutputCache(Duration = 0)]
        public async Task<ActionResult> Index()
        {
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

        public PartialViewResult EquipmentScheduler()
        {
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

        public virtual JsonResult Equipments_Read([DataSourceRequest] DataSourceRequest request)
        {
            IQueryable<EquipmentSchedulerViewModel> tasks = CreateEquipmentSchedules().Select(task => new EquipmentSchedulerViewModel()
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
                Quantity = task.Quantity
            }
            ).AsQueryable();
            return Json(tasks.ToDataSourceResult(request));
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

        private List<EquipmentSchedulerViewModel> CreateEquipmentSchedules()
        {
            int EquipmentStatusVacant = 1;
            List<EquipmentSchedulerViewModel> _listSchecule = new List<EquipmentSchedulerViewModel>();
            var allEquipments = (from equipmentSchedule in db.EquipmentSchedules
                                 join eqp in db.Equipments on equipmentSchedule.EquipmentId equals eqp.EquipmentId
                                 where (eqp.EquipmentStatu.Id == EquipmentStatusVacant)
                                 select new {eqpSchedule = equipmentSchedule, eqpItem = eqp}).ToList();
           

            foreach (var item in allEquipments)
            {
                if (item.eqpSchedule.ServiceForm != null)
                {
                    var entity = db.EquipmentSchedules.FirstOrDefault(m => m.Id == item.eqpSchedule.Id);
                    var matchEntityAtTheSameTime = (from schedule in db.EquipmentSchedules
                                                    where (schedule.StartTime >= entity.StartTime && schedule.StartTime <= entity.EndTime) && (schedule.EquipmentId == entity.EquipmentId)
                                                    select (schedule)).ToList();

                    EquipmentSchedulerViewModel _scheduler = new EquipmentSchedulerViewModel
                    {
                        Id = item.eqpSchedule.Id,
                        Title = item.eqpItem.EquipmentName,
                        Description = item.eqpItem.EquipmentDetail,
                        Start = item.eqpSchedule.StartTime,
                        End = item.eqpSchedule.EndTime,
                        selectedStatus = item.eqpSchedule.Status,
                        Quantity = item.eqpItem.Quantity,
                        RemainItem = item.eqpItem.Quantity - matchEntityAtTheSameTime.Count
                    };
                   
                    _listSchecule.Add(_scheduler);
                }
            }
            return _listSchecule;
        }
    }
}
