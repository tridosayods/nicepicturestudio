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
using NicePictureStudio.Models;

namespace NicePictureStudio
{
    public class ServicesController : Controller
    {
        private NicePictureStudioDBEntities db = new NicePictureStudioDBEntities();

        private PromotionViewModel _promotion;
        private ServiceFromKeeper _formKeeper;

        //Command
        public static readonly string PreWedding = "PreWedding";
        public readonly string Engagement = "Engagement";
        public readonly string Wedding = "Wedding";
        public readonly string HTMLTagForReplace = "FormSection";
        
        //Modal
        public readonly string HTMLModalPhotoGraph = "modalPhotoGraph";
        public readonly string HTMLModalEquipment = "modalEquipment";
        public readonly string HTMLModalLocation = "modalLocation";
        public readonly string HTMLModalOutSource = "modalOutsource";
        public readonly string HTMLModalOutput = "modalOutput";
        
        //Button
        public readonly string HTMLTagButtonPhotoGraph = "btnPhotoGraph";
        public readonly string HTMLTagButtonEquipment = "btnEquipment";
        public readonly string HTMLTagButtonLocation = "btnLocation";
        public readonly string HTMLTagButtonOutSource = "btnOutsource";
        public readonly string HTMLTagButtonOutput = "btnOutput";
        //Container
        public readonly string HTMLContainerButtonPhotoGraph = "ctnPhotoGraph";
        public readonly string HTMLContainerButtonEquipment = "ctnEquipment";
        public readonly string HTMLContainerButtonLocation = "ctnLocation";
        public readonly string HTMLContainerButtonOutSource = "ctnOutsource";
        public readonly string HTMLContainerButtonOutput = "ctnOutput";

        // GET: Services
        public async Task<ActionResult> Index()
        {
            var services = db.Services.Include(s => s.Customer);
            return View(await services.ToListAsync());
        }

        // GET: Services/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Service service = await db.Services.FindAsync(id);
            if (service == null)
            {
                return HttpNotFound();
            }
            return View(service);
        }

        // GET: Services/Create
        public ActionResult Create()
        {
            ViewBag.CustomerId = new SelectList(db.Customers, "CustomerId", "CustomerName");
            ViewBag.BookingId = new SelectList(db.Bookings, "Id", "Name");
            //Binding a promotion from scracth or create new promotion
            ViewBag.BookingList = new SelectList(db.Bookings, "Id", "BookingName");
            return View();
        }


        // POST: Services/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,BookingName,GroomName,BrideName,SpecialRequest,Payment,PayAmount,CustomerId,CRMFormId")] Service service)
        {
            if (ModelState.IsValid)
            {
                db.Services.Add(service);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.CustomerId = new SelectList(db.Customers, "CustomerId", "CustomerName", service.CustomerId);
            return View(service);
        }

        public async Task<PartialViewResult> CreateService(int bookingId)
        {

            Booking booking = await db.Bookings.FindAsync(bookingId);
            if (booking != null)
            {
                //Getting Promotion & Booking Information
                _promotion = new PromotionViewModel(booking.Promotion.ExpireDate,booking.Promotion.PhotoGraphDiscount,
                    booking.Promotion.EquipmentDiscount,
                    booking.Promotion.LocationDiscount, booking.Promotion.OutputDiscount, 
                    booking.Promotion.OutsourceDiscount);
                //Preparing for creating Service
                //CreateService
                // One promotion affet to any package -> need to keep price as static
                // getting promotion and then extract trype of service -> create item.

            }
            return PartialView();
        }

        // GET: Services/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Service service = await db.Services.FindAsync(id);
            if (service == null)
            {
                return HttpNotFound();
            }
            ViewBag.CustomerId = new SelectList(db.Customers, "CustomerId", "CustomerName", service.CustomerId);
            return View(service);
        }

        // POST: Services/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,BookingName,GroomName,BrideName,SpecialRequest,Payment,PayAmount,CustomerId,CRMFormId")] Service service)
        {
            if (ModelState.IsValid)
            {
                db.Entry(service).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.CustomerId = new SelectList(db.Customers, "CustomerId", "CustomerName", service.CustomerId);
            return View(service);
        }

        // GET: Services/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Service service = await db.Services.FindAsync(id);
            if (service == null)
            {
                return HttpNotFound();
            }
            return View(service);
        }

        // POST: Services/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Service service = await db.Services.FindAsync(id);
            db.Services.Remove(service);
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

        #region Create Customer Section
        /***************************Create Customer Section*******************************************************/

        public PartialViewResult CreateCustomerFromService()
        {
            return PartialView();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<PartialViewResult> CreateCustomerFromService([Bind(Include = "CustomerId,CustomerName,PhoneNumber,Address,AnniversaryDate")] Customer customer)
        {
            if (ModelState.IsValid)
            {
                //db.Customers.Add(customer);
                //await db.SaveChangesAsync();
                Customer _customer = customer;
                return PartialView(@"/Views/Services/DetailsCustomerFromService.cshtml", _customer);
            }

            return PartialView();
        }

        public async Task<PartialViewResult> DetailsCustomerFromService(int? id)
        {
            if (id != null)
            {
                Customer customer = await db.Customers.FindAsync(id);
                if (customer == null)
                {
                    return PartialView();
                }
                else
                {
                    return PartialView(customer);
                }
            }
            else
            {
                return PartialView();
            }
        }



        public async Task<PartialViewResult> EditCustomerFromService(int? id)
        {
            if (id != null)
            {
                Customer customer = await db.Customers.FindAsync(id);
                if (customer == null)
                {
                    return PartialView();
                }
                else
                {
                    return PartialView(customer);
                }
            }
            return PartialView();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<PartialViewResult> EditCustomerFromService([Bind(Include = "CustomerId,CustomerName,PhoneNumber,Address,AnniversaryDate")] Customer customer)
        {
            if (ModelState.IsValid)
            {
                db.Entry(customer).State = EntityState.Modified;
                await db.SaveChangesAsync();
                Customer _customer = customer;
                return PartialView(@"/Views/Services/DetailsCustomerFromService.cshtml", _customer);
            }
            return PartialView();
        }


        /*Get*/
        public async Task<PartialViewResult> ViewCustomerFromService(int customerid)
        {
            Customer model = await db.Customers.FindAsync(customerid);
            return PartialView(model);
        }


        /***************************Create Customer Section*******************************************************/
        #endregion


        #region Create Service Form Section
        /***************************Create Service Form Section*******************************************************/

        public async Task<PartialViewResult> GetServiceFormById(int serviceFormId)
        {
            ServiceForm model = await db.ServiceForms.FindAsync(serviceFormId);
            return PartialView(model);
        }

          public PartialViewResult CreateServiceFormFromService(string Command)
        {
            //assign value for replacing #id in view
            ViewBag.ServiceTypeItem = Command;
            ViewBag.ServiceTypeTag = Command + HTMLTagForReplace;
            ViewBag.HTMLTagButtonPhotoGraph = HTMLTagButtonPhotoGraph + Command;
            ViewBag.HTMLTagButtonEquipment = HTMLTagButtonEquipment + Command;
            ViewBag.HTMLTagButtonLocation = HTMLTagButtonLocation + Command;
            ViewBag.HTMLTagButtonOutSource = HTMLTagButtonOutSource + Command;
            ViewBag.HTMLTagButtonOutput = HTMLTagButtonOutput + Command;
            ViewBag.HTMLContainerButtonPhotoGraph = HTMLContainerButtonPhotoGraph + Command;
            ViewBag.HTMLContainerButtonEquipment = HTMLContainerButtonEquipment + Command;
            ViewBag.HTMLContainerButtonLocation = HTMLContainerButtonLocation + Command;
            ViewBag.HTMLContainerButtonOutSource = HTMLContainerButtonOutSource + Command;
            ViewBag.HTMLContainerButtonOutput = HTMLContainerButtonOutput + Command;
            ViewBag.HTMLModalPhotoGraph = HTMLModalPhotoGraph + Command;
            ViewBag.HTMLModalEquipment = HTMLModalEquipment + Command;
            ViewBag.HTMLModalLocation = HTMLModalLocation + Command;
            ViewBag.HTMLModalOutSource = HTMLModalOutSource + Command;
            ViewBag.HTMLModalOutput = HTMLModalOutput + Command;
            return PartialView();
        }

          [HttpPost]
          [ValidateAntiForgeryToken]
          public async Task<PartialViewResult> CreateServiceFormFromService([Bind(Include = "Id,Name,ServiceType,Status,EventStart,EventEnd,GuestsNumber")] ServiceForm serviceForm, string Command)
          {
              if (ModelState.IsValid)
              {
                  ServiceType serviceType = db.ServiceTypes.Where(s => string.Compare(s.ServiceTypeName, Command, true) == 0).FirstOrDefault();
                  if (serviceType != null)
                  {
                      serviceForm.ServiceType = serviceType.Id;
                      //db.ServiceForms.Add(serviceForm);
                      //await db.SaveChangesAsync();
                  }
                  else { return PartialView(); }

                  //assign value for replacing #id in view
                  ServiceForm _serviceForm = serviceForm;
                  return PartialView(@"/Views/ServiceForms/DetailsServiceFormFromService.cshtml", serviceForm);
              }
              return PartialView();
          }


          public async Task<PartialViewResult> DetailsServiceFormFromService(int? id)
          {
              if (id != null)
              {
                  ServiceForm serviceForm = await db.ServiceForms.FindAsync(id);
                  if (serviceForm == null)
                  {
                      return PartialView();
                  }
                  else
                  {
                      //assign value for replacing #id in view
                      ViewBag.ServiceTypeItem = serviceForm.ServiceType1.ServiceTypeName;
                      return PartialView(serviceForm);
                  }
              }
              else
              {
                  return PartialView();
              }
          }

          public async Task<PartialViewResult> EditServiceFormFromService(int? id)
          {
              if (id != null)
              {
                  ServiceForm serviceForm = await db.ServiceForms.FindAsync(id);
                  if (serviceForm == null)
                  {
                      return PartialView();
                  }
                  else
                  {
                      return PartialView(serviceForm);
                  }
              }
              return PartialView();
          }

          [HttpPost]
          [ValidateAntiForgeryToken]
          public async Task<PartialViewResult> EditServiceFormFromService([Bind(Include = "Id,Name,ServiceType,Status,EventStart,EventEnd,GuestsNumber")] ServiceForm serviceForm)
          {
              if (ModelState.IsValid)
              {
                  db.Entry(serviceForm).State = EntityState.Modified;
                  await db.SaveChangesAsync();
                  ServiceForm _serviceForm = serviceForm;
                  return PartialView(@"/Views/Services/DetailsServiceFormFromService.cshtml", serviceForm);
              }
              return PartialView();
          }

        /***************************Create Service Form Section*******************************************************/
        #endregion

        #region CreateModal Window section

        [HttpGet]  
        public async Task<PartialViewResult> CreatePhotoGraphServiceByModal(int? id)
          {
            PhotographService photoGraphService;
            ViewData["PhotoGraphList"] =  new SelectList(db.PhotographServices, "Id", "Name");
            if (id != null)
            { photoGraphService = await db.PhotographServices.FindAsync(id); }
            else
            { photoGraphService = await db.PhotographServices.FirstAsync(); }
            
            ViewData["Code"] = photoGraphService.Id;
            return PartialView(photoGraphService); 
          }

        //[HttpPost]
        //public async Task<PartialViewResult> CreatePhotoGraphServiceByModal(int? id)
        //{
        //    ViewData["PhotoGraphList"] = new SelectList(db.PhotographServices, "Id", "Name", "Description");
        //    PhotographService photoGraphService = await db.PhotographServices.FindAsync(id);
        //    ViewData["Code"] = photoGraphService.Id;
        //    return PartialView(photoGraphService);
        //}

        #endregion
    }
}
