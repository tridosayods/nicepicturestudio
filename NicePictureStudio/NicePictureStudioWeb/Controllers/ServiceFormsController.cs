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
               // return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ServiceForm serviceForm = await db.ServiceForms.FindAsync(id);
            if (serviceForm == null)
            {
                //return HttpNotFound();
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
        public async Task<ActionResult> Create([Bind(Include = "Id,Name,ServiceType,Status,EventStart,EventEnd,GuestsNumber")] ServiceForm serviceForm)
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
        public async Task<ActionResult> Edit([Bind(Include = "Id,Name,ServiceType,Status,EventStart,EventEnd,GuestsNumber")] ServiceForm serviceForm)
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


    }
}
