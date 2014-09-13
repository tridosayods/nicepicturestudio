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
    public class OutsourceContactsController : Controller
    {
        private NicePictureStudioDBEntities db = new NicePictureStudioDBEntities();

        // GET: OutsourceContacts
        public async Task<ActionResult> Index()
        {
            var outsourceContacts = db.OutsourceContacts.Include(o => o.OutsourceServiceType).Include(o => o.OutsourceStatu);
            return View(await outsourceContacts.ToListAsync());
        }

        // GET: OutsourceContacts/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            OutsourceContact outsourceContact = await db.OutsourceContacts.FindAsync(id);
            if (outsourceContact == null)
            {
                return HttpNotFound();
            }
            return View(outsourceContact);
        }

        // GET: OutsourceContacts/Create
        public ActionResult Create()
        {
            ViewBag.OutsourceType = new SelectList(db.OutsourceServiceTypes, "Id", "TypeName");
            ViewBag.status = new SelectList(db.OutsourceStatus, "Id", "StatusName");
            return View();
        }

        // POST: OutsourceContacts/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "OutsourceContactId,OutsourceName,OutsourceType,Address,PhoneNumber,OpenTime,CloseTime,Detail,status")] OutsourceContact outsourceContact)
        {
            if (ModelState.IsValid)
            {
                db.OutsourceContacts.Add(outsourceContact);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.OutsourceType = new SelectList(db.OutsourceServiceTypes, "Id", "TypeName", outsourceContact.OutsourceServiceType);
            ViewBag.status = new SelectList(db.OutsourceStatus, "Id", "StatusName", outsourceContact.OutsourceStatu);
            return View(outsourceContact);
        }

        // GET: OutsourceContacts/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            OutsourceContact outsourceContact = await db.OutsourceContacts.FindAsync(id);
            if (outsourceContact == null)
            {
                return HttpNotFound();
            }
            ViewBag.OutsourceType = new SelectList(db.OutsourceServiceTypes, "Id", "TypeName", outsourceContact.OutsourceServiceType);
            ViewBag.status = new SelectList(db.OutsourceServices, "Id", "StatusName", outsourceContact.OutsourceStatu);
            return View(outsourceContact);
        }

        // POST: OutsourceContacts/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "OutsourceContactId,OutsourceName,OutsourceType,Address,PhoneNumber,OpenTime,CloseTime,Detail,status")] OutsourceContact outsourceContact)
        {
            if (ModelState.IsValid)
            {
                db.Entry(outsourceContact).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.OutsourceType = new SelectList(db.OutsourceServiceTypes, "Id", "TypeName", outsourceContact.OutsourceServiceType);
            ViewBag.status = new SelectList(db.OutsourceServices, "Id", "StatusName", outsourceContact.OutsourceStatu);
            return View(outsourceContact);
        }

        // GET: OutsourceContacts/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            OutsourceContact outsourceContact = await db.OutsourceContacts.FindAsync(id);
            if (outsourceContact == null)
            {
                return HttpNotFound();
            }
            return View(outsourceContact);
        }

        // POST: OutsourceContacts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            OutsourceContact outsourceContact = await db.OutsourceContacts.FindAsync(id);
            db.OutsourceContacts.Remove(outsourceContact);
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
