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
    public class OutputSchedulesController : Controller
    {
        private NicePictureStudioDBEntities db = new NicePictureStudioDBEntities();

        // GET: OutputSchedules
        public async Task<ActionResult> Index()
        {
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

        public PartialViewResult OutputScheduler()
        {
            return PartialView();
        }

        public virtual JsonResult Outputs_Read([DataSourceRequest] DataSourceRequest request)
        {
            IQueryable<OutputSchedulerViewModels> tasks = CreateOutputSchedules().Select(task => new OutputSchedulerViewModels()
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

        private List<OutputSchedulerViewModels> CreateOutputSchedules()
        {
            List<OutputSchedulerViewModels> _listSchecule = new List<OutputSchedulerViewModels>();
            var allLocation = (from outputSchedule in db.OutputSchedules
                               join output in db.OutputServices on outputSchedule.OutputServiceId equals output.Id
                               select new { outputSchedule = outputSchedule, output = output }).ToList();
            foreach (var item in allLocation)
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
                    if (string.IsNullOrEmpty(service.Title))
                    {
                        service.Title = "";
                    }
                    var entity = db.OutputSchedules.FirstOrDefault(m => m.Id == service.Id);
                    entity.TargetDate = service.Start;
                    entity.HandOnDate= service.End;
                    entity.ReviseDate = service.ReviseDate;
                    entity.ReviseCount = service.ReviseCount;
                    entity.Status = service.selectedStatus;
                    db.Entry(entity).State = EntityState.Modified;
                    db.SaveChanges();
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
    }
}
