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
using System.Data.Entity.Validation;
using System.Diagnostics;

namespace NicePictureStudio
{
    public class BookingsController : Controller
    {
        private NicePictureStudioDBEntities db = new NicePictureStudioDBEntities();

        // GET: Bookings
        public async Task<ActionResult> Index()
        {
            var bookings = db.Bookings.Include(b => b.BookingStatu).Include(b => b.Promotion).Include(b => b.Service);
            return View(await bookings.ToListAsync());
        }

        // GET: Bookings/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Booking booking = await db.Bookings.FindAsync(id);
            if (booking == null)
            {
                return HttpNotFound();
            }
            return View(booking);
        }

        // GET: Bookings/Create
        public ActionResult Create()
        {
            ViewBag.BookingStatus = new SelectList(db.BookingStatus, "Id", "Name");
            ViewBag.PromotionId = new SelectList(db.Promotions, "Id", "Name");
            ViewBag.ServiceId = new SelectList(db.Services, "Id", "BookingName");
            ViewBag.BookingNumber = DateTime.Now.GetHashCode();
            return View();
        }

        // POST: Bookings/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,Name,BookingCode,AppointmentDate,SpecialOrder,Details,BookingStatus,PromotionId,ServiceId")] Booking booking)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    db.Bookings.Add(booking);
                    await db.SaveChangesAsync();
                    return RedirectToAction("Index");
                }
                catch (DbEntityValidationException dbEx)
                {
                    foreach (var validationErrors in dbEx.EntityValidationErrors)
                    {
                        foreach (var validationError in validationErrors.ValidationErrors)
                        {
                            Trace.TraceInformation("Class: {0}, Property: {1}, Error: {2}",
                                validationErrors.Entry.Entity.GetType().FullName,
                                validationError.PropertyName,
                                validationError.ErrorMessage);
                        }
                    }

                    throw new Exception(dbEx.Message);
                }
               
            }

            ViewBag.BookingStatus = new SelectList(db.BookingStatus, "Id", "Name", booking.BookingStatus);
            ViewBag.PromotionId = new SelectList(db.Promotions, "Id", "Name", booking.PromotionId);
            ViewBag.ServiceId = new SelectList(db.Services, "Id", "BookingName", booking.ServiceId);
            return View(booking);
        }

        // GET: Bookings/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Booking booking = await db.Bookings.FindAsync(id);
            if (booking == null)
            {
                return HttpNotFound();
            }
            ViewBag.BookingStatus = new SelectList(db.BookingStatus, "Id", "Name", booking.BookingStatus);
            ViewBag.PromotionId = new SelectList(db.Promotions, "Id", "Name", booking.PromotionId);
            ViewBag.ServiceId = new SelectList(db.Services, "Id", "BookingName", booking.ServiceId);
            return View(booking);
        }

        // POST: Bookings/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,Name,BookingCode,AppointmentDate,SpecialOrder,Details,BookingStatus,PromotionId,ServiceId")] Booking booking)
        {
            if (ModelState.IsValid)
            {
                db.Entry(booking).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.BookingStatus = new SelectList(db.BookingStatus, "Id", "Name", booking.BookingStatus);
            ViewBag.PromotionId = new SelectList(db.Promotions, "Id", "Name", booking.PromotionId);
            ViewBag.ServiceId = new SelectList(db.Services, "Id", "BookingName", booking.ServiceId);
            return View(booking);
        }

        // GET: Bookings/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Booking booking = await db.Bookings.FindAsync(id);
            if (booking == null)
            {
                return HttpNotFound();
            }
            return View(booking);
        }

        // POST: Bookings/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Booking booking = await db.Bookings.FindAsync(id);
            db.Bookings.Remove(booking);
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
