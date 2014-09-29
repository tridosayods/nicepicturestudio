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
    public class EmployeeSchedulesController : Controller
    {
        private NicePictureStudioDBEntities db = new NicePictureStudioDBEntities();

        // GET: EmployeeSchedules
        public async Task<ActionResult> Index()
        {
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

        public PartialViewResult EmployeesScheduler()
        {
            return PartialView();
        }

        public virtual JsonResult Employees_Read([DataSourceRequest] DataSourceRequest request)
        {
            IQueryable<SchedulerViewModels> tasks = CreateEmployeeSchedules().Select(task => new SchedulerViewModels()
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

        private List<SchedulerViewModels> CreateEmployeeSchedules()
        {
            List<SchedulerViewModels> _listSchecule = new List<SchedulerViewModels>();
            var allServiceForms = db.Employees.Where(emp => (emp.Position == "") || (emp.Position == "")).ToList();
            var allemployee = (from empSchedule in db.EmployeeSchedules
                              join emp in db.Employees on empSchedule.Employee.Id equals emp.Id
                              where (emp.Position == "PhotoGraph" || emp.Position == "CameraMan")
                               select empSchedule).ToList();
            foreach (var item in allemployee)
            {
                if (item.ServiceForm != null)
                {
                    SchedulerViewModels _scheduler = new SchedulerViewModels
                    {
                        Id = item.Id,
                        Title = item.Employee.Name,
                        Description = item.Employee.Position,
                        Start = item.StartTime,
                        End = item.EndTime,
                        selectedStatus = item.ServiceForm.ServiceStatu.Id
                    };
                    _listSchecule.Add(_scheduler);
                }
            }
            return _listSchecule;
        }
    }
}
