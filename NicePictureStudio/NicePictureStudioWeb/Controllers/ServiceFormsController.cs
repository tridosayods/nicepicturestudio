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

namespace NicePictureStudio
{
    public class ServiceFormsController : Controller
    {
        private NicePictureStudioDBEntities db = new NicePictureStudioDBEntities();

        // GET: ServiceForms
        public async Task<ActionResult> Index()
        {
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

        public PartialViewResult ServiceFormScheduler()
        {
            return PartialView();
        }

        public virtual JsonResult Services_Read([DataSourceRequest] DataSourceRequest request)
        {
            IQueryable<SchedulerViewModels> tasks = CreateSchedulerServiceForm().Select(task => new SchedulerViewModels()
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

        private List<SchedulerViewModels> CreateSchedulerServiceForm()
        {
            List<SchedulerViewModels> _listSchecule = new List<SchedulerViewModels>();
            var allServiceForms = db.ServiceForms.ToList();
            foreach (var item in allServiceForms)
            {
                SchedulerViewModels _scheduler = new SchedulerViewModels { 
                    Id = item.Id,
                    Title = item.ServiceType.ServiceTypeName,
                    Description = item.Name,
                    Start = item.EventStart,
                    End = item.EventEnd,
                    selectedStatus = item.ServiceStatu.Id
                };
                _listSchecule.Add(_scheduler);
            }
            return _listSchecule;
        }

        public virtual JsonResult Services_Update([DataSourceRequest] DataSourceRequest request, SchedulerViewModels service)
        {
            if (ModelState.IsValid)
            {
                if (ValidateModel(service,ModelState))
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


