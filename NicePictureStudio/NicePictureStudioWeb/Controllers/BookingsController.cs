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
using NicePictureStudio.Models;
using AutoMapper;
using Kendo.Mvc.UI;
using Kendo.Mvc.Extensions;
using NicePictureStudio.Utils;

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

        public ActionResult Bookings_read([DataSourceRequest] DataSourceRequest request)
        {
            var bookings = db.Bookings.Include(b => b.BookingStatu).Include(b => b.Promotion).Include(b => b.Service).ToList();
            IQueryable<BookingViewsModel> tasks = bookings.Select(task => new BookingViewsModel()
            {
                Id = task.Id,
                Name = task.Title + " "+  task.Name + " " + task.Surname,
                AppointmentDate = task.AppointmentDate,
                BookingCode = task.BookingCode,
                BookingStatus = task.BookingStatu.Name,
                Details = task.Details,
                PromotionName = task.Promotion == null ? string.Empty : task.Promotion.Name,
                ServiceName = task.Service == null ? string.Empty : task.Service.Customer.CustomerName,
            }
             ).AsQueryable();
            return Json(tasks.ToDataSourceResult(request));
        }

        public ActionResult BookingsNotConfirm_read([DataSourceRequest] DataSourceRequest request)
        {
            var bookings = db.Bookings.Include(b => b.BookingStatu).Include(b => b.Promotion).Include(b => b.Service).ToList();
            var currentDate = DateTime.Now;
            //#1 Booking that not operated yet , almost due or overdeu => red warning
            var bookingNotConfirm = bookings.Where(book => book.BookingStatu.Id < Constant.BOOKING_STATUS_OPERATED && (book.AppointmentDate - currentDate).TotalDays < 0).OrderBy(b=>b.AppointmentDate);
            var lstBookingNotConfirm = new List<BookingViewsModel>();
            foreach (var books in bookingNotConfirm)
            {
                var booking = new BookingViewsModel
                {
                    Id = books.Id,  
                    Name = books.Title + " " + books.Name + " " + books.Surname,
                    AppointmentDate = books.AppointmentDate,
                    BookingCode = books.BookingCode,
                    BookingStatus = "เลยกำหนดนัดหมาย",
                    Details = books.Details,
                    PromotionName = books.Promotion == null ? string.Empty : books.Promotion.Name,
                    ServiceName = books.Service == null ? string.Empty : books.Service.Customer.CustomerName
                };
                lstBookingNotConfirm.Add(booking);
            }

            var bookingTobeConfirm = bookings.Where(book => book.BookingStatu.Id < Constant.BOOKING_STATUS_OPERATED && (book.AppointmentDate - currentDate).TotalDays > 0 
                && (book.AppointmentDate - currentDate).TotalDays <= 3).OrderBy(b => b.AppointmentDate);
            var lstBookingTobeConfirm = new List<BookingViewsModel>();
            foreach (var books in bookingTobeConfirm)
            {
                var booking = new BookingViewsModel
                {
                    Id = books.Id,
                    Name = books.Title + " " + books.Name + " " + books.Surname,
                    AppointmentDate = books.AppointmentDate,
                    BookingCode = books.BookingCode,
                    BookingStatus = "ใกล้ถึงเวลานัดหมาย",
                    Details = books.Details,
                    PromotionName = books.Promotion == null ? string.Empty : books.Promotion.Name,
                    ServiceName = books.Service == null ? string.Empty : books.Service.Customer.CustomerName
                };
                lstBookingTobeConfirm.Add(booking);
            }

            //#2 Booking all in normal, asc
            var bookingAlls = bookings.Where(book =>(book.AppointmentDate - currentDate).TotalDays > 3).OrderBy(b => b.AppointmentDate).OrderBy(b=>b.AppointmentDate);
            var lstBookingNormalAlls = new List<BookingViewsModel>();
            foreach (var books in bookingAlls)
            {
                var booking = new BookingViewsModel
                {
                    Id = books.Id,
                    Name = books.Title + " " + books.Name + " " + books.Surname,
                    AppointmentDate = books.AppointmentDate,
                    BookingCode = books.BookingCode,
                    BookingStatus = books.BookingStatu.Name,
                    Details = books.Details,
                    PromotionName = books.Promotion == null ? string.Empty : books.Promotion.Name,
                    ServiceName = books.Service == null ? string.Empty : books.Service.Customer.CustomerName
                };
                lstBookingNormalAlls.Add(booking);
            }

            var allUnionListBooking = lstBookingNotConfirm.Union(lstBookingTobeConfirm).Union(lstBookingNormalAlls);
            IQueryable<BookingViewsModel> tasks = allUnionListBooking.Select(task => new BookingViewsModel()
            {
                //Id = task.Id,
                //Name = task.Title + " " + task.Name + " " + task.Surname,
                //AppointmentDate = task.AppointmentDate,
                //BookingCode = task.BookingCode,
                //BookingStatus = task.BookingStatu.Name,
                //Details = task.Details,
                //PromotionName = task.Promotion == null ? string.Empty : task.Promotion.Name,
                //ServiceName = task.Service == null ? string.Empty : task.Service.Customer.CustomerName,
                Id = task.Id,
                Name = task.Name,
                AppointmentDate = task.AppointmentDate,
                BookingCode = task.BookingCode,
                BookingStatus = task.BookingStatus,
                Details = task.Details,
               PromotionName = task.PromotionName ,
               ServiceName = task.ServiceName 
            }
             ).AsQueryable();
            return Json(tasks.ToDataSourceResult(request));
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
            //Mapper.CreateMap<Booking, BookingViewModel>();
            //BookingViewModel bookingView = Mapper.Map<BookingViewModel>(booking);
            return View(booking);
        }

        // GET: Bookings/Create
        public ActionResult Create()
        {
            //Getting Valid Promotion
            var validPromotion = db.Promotions.Where(pt => DateTime.Now <= pt.ExpireDate);
            ViewBag.BookingStatus = new SelectList(db.BookingStatus, "Id", "Name");
            ViewBag.PromotionId = new SelectList(validPromotion, "Id", "Name");
            ViewBag.ServiceId = new SelectList(db.Services, "Id", "BookingName");
            ViewBag.SpecialOrder = new MultiSelectList(db.BookingSpecialRequests, "Id", "Name");
            int _latestBookingId;
            if (db.Bookings.Count() > 0)
            {  _latestBookingId = db.Bookings.Max(p => p.Id); }
            else
            { 
                _latestBookingId =0;
            }
            ViewBag.BookingNumber = DateTime.Now.ToString("yyyy") + DateTime.Now.Month + DateTime.Now.Day+ _latestBookingId.ToString();
            //ViewBag.BookingNumber = Math.Abs(Convert.ToInt32(DateTime.Now.GetHashCode())).ToString().Substring(0,5);
            return View();
        }

        // POST: Bookings/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,Title,Name,Surname,BookingCode,AppointmentDate,SpecialOrder,Details,BookingStatu,PromotionId,ServiceId,ContactNumber,ContactEmail")] Booking booking, int PromotionId, int[] SpecialOrder)
        {
            int statusConfirm = Constant.BOOKING_STATUS_CONFIRM;
            if (ModelState.IsValid)
            {
                try
                {
                    //booking status always 1
                    
                    BookingStatu bookingStatus = await db.BookingStatus.FindAsync(statusConfirm);
                    if (SpecialOrder != null)
                    {
                        foreach (int reqId in SpecialOrder)
                        {
                            BookingSpecialRequest specialorder = db.BookingSpecialRequests.Find(reqId);
                            booking.BookingSpecialRequests.Add(specialorder);
                        }
                    }
                    
                    Promotion promotion = await db.Promotions.FindAsync(PromotionId);
                    //BookingSpecialRequest specialorder = await db.BookingSpecialRequests.FindAsync(SpecialOrder);
                    booking.BookingStatu = bookingStatus;
                    booking.Promotion = promotion;
                    //booking.BookingSpecialRequest = specialorder;
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

            ViewBag.BookingStatus = new SelectList(db.BookingStatus, "Id", "Name", booking.Id);
            ViewBag.PromotionId = new SelectList(db.Promotions, "Id", "Name", booking.Id);
            ViewBag.ServiceId = new SelectList(db.Services, "Id", "BookingName", booking.Id);
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
            var InvalidPromotion = db.Promotions.Where(pt => DateTime.Now > pt.ExpireDate);
            var targetPromotion = db.Promotions.Where(tg => tg.Id == booking.Promotion.Id);
            var ValidPromotion = (db.Promotions.Except<Promotion>(InvalidPromotion).AsEnumerable()).Union(targetPromotion);
            var SelectedSpecialOrder = booking.BookingSpecialRequests.AsEnumerable();
            ViewBag.SelectedSpecialOrder = new MultiSelectList(SelectedSpecialOrder, "Id", "Name");
            ViewBag.BookingStatus = new SelectList(db.BookingStatus, "Id", "Name", booking.BookingStatu.Id);
            ViewBag.PromotionId = new SelectList(ValidPromotion, "Id", "Name", booking.Promotion.Id);
            ViewBag.SpecialOrder = new MultiSelectList(db.BookingSpecialRequests, "Id", "Name", SelectedSpecialOrder); 
            return View(booking);
        }

        // POST: Bookings/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,Title,Name,Surname,BookingCode,AppointmentDate,SpecialOrder,Details,PromotionId,ServiceId,ContactNumber,ContactEmail")] Booking booking, int BookingStatus, int PromotionId, int[] SpecialOrder)
        {
            if (ModelState.IsValid)
            {
                
                Booking currentBooking = await db.Bookings.FindAsync(booking.Id);
                Promotion promotion = await db.Promotions.FindAsync(PromotionId);
                //BookingSpecialRequest specialorder = await db.BookingSpecialRequests.FindAsync(SpecialOrder);
                BookingStatu bookingStauts = await db.BookingStatus.FindAsync(BookingStatus);

                currentBooking.Title = booking.Title;
                currentBooking.Name = booking.Name;
                currentBooking.Surname = booking.Surname;
                currentBooking.AppointmentDate = booking.AppointmentDate;
                currentBooking.Details = booking.Details;
                currentBooking.ContactEmail = booking.ContactEmail;
                currentBooking.ContactNumber = booking.ContactNumber;
                
                currentBooking.BookingStatu = bookingStauts;
                currentBooking.Promotion = promotion;
                //Comparing recorded

                if (SpecialOrder != null)
                {
                    var existSpecialOrderList = currentBooking.BookingSpecialRequests;
                    List<BookingSpecialRequest> newList = new List<BookingSpecialRequest>();

                    foreach (int reqId in SpecialOrder)
                    {
                        BookingSpecialRequest specialorder = await db.BookingSpecialRequests.FindAsync(reqId);
                        newList.Add(specialorder);
                    }
                    var intersectBookingList = existSpecialOrderList.Intersect<BookingSpecialRequest>(newList.AsEnumerable());

                    var updateBookingList = intersectBookingList.Union(newList.AsEnumerable());
                    currentBooking.BookingSpecialRequests.Clear();

                    foreach (var newBooking in updateBookingList)
                    {
                        currentBooking.BookingSpecialRequests.Add(newBooking);
                    }
                }
                else
                {
                    currentBooking.BookingSpecialRequests.Clear();
                }
                
                //currentBooking.BookingSpecialRequest = specialorder;
                db.Entry(currentBooking).State = EntityState.Modified;
                
                //db.Bookings.Attach(booking);
                ////db.Entry(currentBooking).Reference(st => st.BookingStatu).Load();
                //booking.BookingStatu = bookingStauts;
                
                //booking.Promotion = promotion;
                //booking.BookingSpecialRequest = specialorder;
                //db.Entry(booking.BookingStatu).State = EntityState.Modified;
                //db.Entry(booking.Promotion).State = EntityState.Modified;
                //db.Entry(booking.BookingSpecialRequest).State = EntityState.Modified;
                
                //db.Entry(booking).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.BookingStatus = new SelectList(db.BookingStatus, "Id", "Name", booking.BookingStatu.Id);
            ViewBag.PromotionId = new SelectList(db.Promotions, "Id", "Name", booking.Promotion.Id);
            ViewBag.ServiceId = new SelectList(db.Services, "Id", "BookingName", booking.Service.Id);
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

        public virtual JsonResult Appointments_Read([DataSourceRequest] DataSourceRequest request)
        {
            IQueryable<SchedulerViewModels> tasks = CreateApppointmentSchedules().Select(task => new SchedulerViewModels()
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

        public virtual JsonResult Appointments_Update([DataSourceRequest] DataSourceRequest request, SchedulerViewModels service)
        {
            if (ModelState.IsValid)
            {
                if (ValidateModel(service, ModelState))
                {
                    if (string.IsNullOrEmpty(service.Title))
                    {
                        service.Title = "";
                    }
                    var entity = db.Bookings.FirstOrDefault(m => m.Id == service.Id);
                    var entityStatus = db.BookingStatus.FirstOrDefault(bs => bs.Id == service.selectedStatus);
                    entity.BookingStatu = entityStatus;
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

        private List<SchedulerViewModels> CreateApppointmentSchedules()
        {
            List<SchedulerViewModels> _listSchecule = new List<SchedulerViewModels>();
            var allAppointment = (from app in db.Bookings
                                  select app).ToList();
            foreach (var item in allAppointment)
            {
                if (item != null)
                {
                    SchedulerViewModels _scheduler = new SchedulerViewModels
                    {
                        Id = item.Id,
                        Title = item.Name,
                        Description = item.Details,
                        Start = item.AppointmentDate,
                        End = item.AppointmentDate.AddHours(3),
                        selectedStatus = item.BookingStatu.Id
                    };
                    _listSchecule.Add(_scheduler);
                }
            }
            return _listSchecule;
        }
    }
}
