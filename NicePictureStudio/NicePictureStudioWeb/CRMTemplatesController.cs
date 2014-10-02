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
    public class CRMTemplatesController : Controller
    {
        private NicePictureStudioDBEntities db = new NicePictureStudioDBEntities();

        // GET: CRMTemplates
        public async Task<ActionResult> Index()
        {
            var cRMTemplates = db.CRMTemplates.Include(c => c.CRMServiceCategory).Include(c => c.CRMServiceType);
            return View(await cRMTemplates.ToListAsync());
        }

        // GET: CRMTemplates/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CRMTemplate cRMTemplate = await db.CRMTemplates.FindAsync(id);
            if (cRMTemplate == null)
            {
                return HttpNotFound();
            }
            return View(cRMTemplate);
        }

        // GET: CRMTemplates/Create
        public ActionResult Create()
        {
            ViewBag.ServiceCategory = new SelectList(db.CRMServiceCategories, "Id", "Name");
            ViewBag.ServiceType = new SelectList(db.CRMServiceTypes, "Id", "Name");
            return View();
        }

        // POST: CRMTemplates/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,Question,ServiceCategory,ServiceType")] CRMTemplate cRMTemplate)
        {
            if (ModelState.IsValid)
            {
                db.CRMTemplates.Add(cRMTemplate);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.ServiceCategory = new SelectList(db.CRMServiceCategories, "Id", "Name", cRMTemplate.ServiceCategory);
            ViewBag.ServiceType = new SelectList(db.CRMServiceTypes, "Id", "Name", cRMTemplate.ServiceType);
            return View(cRMTemplate);
        }

        // GET: CRMTemplates/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CRMTemplate cRMTemplate = await db.CRMTemplates.FindAsync(id);
            if (cRMTemplate == null)
            {
                return HttpNotFound();
            }
            ViewBag.ServiceCategory = new SelectList(db.CRMServiceCategories, "Id", "Name", cRMTemplate.ServiceCategory);
            ViewBag.ServiceType = new SelectList(db.CRMServiceTypes, "Id", "Name", cRMTemplate.ServiceType);
            return View(cRMTemplate);
        }

        // POST: CRMTemplates/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,Question,ServiceCategory,ServiceType")] CRMTemplate cRMTemplate)
        {
            if (ModelState.IsValid)
            {
                db.Entry(cRMTemplate).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.ServiceCategory = new SelectList(db.CRMServiceCategories, "Id", "Name", cRMTemplate.ServiceCategory);
            ViewBag.ServiceType = new SelectList(db.CRMServiceTypes, "Id", "Name", cRMTemplate.ServiceType);
            return View(cRMTemplate);
        }

        // GET: CRMTemplates/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CRMTemplate cRMTemplate = await db.CRMTemplates.FindAsync(id);
            if (cRMTemplate == null)
            {
                return HttpNotFound();
            }
            return View(cRMTemplate);
        }

        // POST: CRMTemplates/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            CRMTemplate cRMTemplate = await db.CRMTemplates.FindAsync(id);
            db.CRMTemplates.Remove(cRMTemplate);
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

        public ActionResult NoCRMPageFound()
        {
            return View();
        }

        public ActionResult CRMApprisal(int? id)
        {
            if (id > 0)
            {
                ViewBag.QuestionList = db.CRMTemplates.ToList();
                Service service = db.Services.Where(srv => srv.CRMFormId == id).FirstOrDefault();
                Customer customer = new Customer();
                if (service != null)
                {
                    customer = service.Customer;
                }
                return View(customer);
            }
            else
            {
                return RedirectToAction("NoCRMPageFound");
            }
        }

        [HttpPost]
        public ActionResult CRMApprisal(List<int> RadioBtnSelcet)
        {
            return RedirectToAction("Index");
        }
    }
}
