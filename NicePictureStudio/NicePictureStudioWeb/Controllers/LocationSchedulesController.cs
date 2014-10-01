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
    public class LocationSchedulesController : Controller
    {
        private NicePictureStudioDBEntities db = new NicePictureStudioDBEntities();

        // GET: LocationSchedules
        public async Task<ActionResult> Index()
        {
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

        public PartialViewResult LocationScheduler()
        {
            return PartialView();
        }

        public virtual JsonResult Locations_Read([DataSourceRequest] DataSourceRequest request)
        {
            IQueryable<SchedulerViewModels> tasks = CreateLocationSchedules().Select(task => new SchedulerViewModels()
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

        private List<SchedulerViewModels> CreateLocationSchedules()
        {
            int statuLocationClose = 2;
            List<SchedulerViewModels> _listSchecule = new List<SchedulerViewModels>();
            var allLocation = (from locatonSchedule in db.LocationSchedules
                               join loc in db.Locations on locatonSchedule.LocationId equals loc.LocationId
                               where (loc.LocationStatu.Id != statuLocationClose)
                               select new { locSchedule = locatonSchedule, location = loc }).ToList();
            foreach (var item in allLocation)
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
