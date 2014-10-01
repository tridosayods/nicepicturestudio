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
    public class OutsourceSchedulesController : Controller
    {
        private NicePictureStudioDBEntities db = new NicePictureStudioDBEntities();

        // GET: OutsourceSchedules
        public async Task<ActionResult> Index()
        {
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

        public PartialViewResult OutsourceScheduler()
        {
            return PartialView();
        }

        public virtual JsonResult Outsources_Read([DataSourceRequest] DataSourceRequest request)
        {
            IQueryable<SchedulerViewModels> tasks = CreateOutsouceSchedules().Select(task => new SchedulerViewModels()
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

        private List<SchedulerViewModels> CreateOutsouceSchedules()
        {
            List<SchedulerViewModels> _listSchecule = new List<SchedulerViewModels>();
            var allLocation = (from outsourceSchedule in db.OutsourceSchedules
                               join outsource in db.OutsourceContacts on outsourceSchedule.OutsourceId equals outsource.OutsourceContactId
                               select new { outsourceSchedule = outsourceSchedule, outsource = outsource }).ToList();
            foreach (var item in allLocation)
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
    }
}
