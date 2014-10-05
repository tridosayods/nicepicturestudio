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
using NicePictureStudio.Utils;
using System.Web.Http.Cors;

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

        public ActionResult FinishingApprisal()
        {
            return View();
        }

        public ActionResult CRMApprisal(int? id)
        {
            if (id > 0)
            {
                //Check if id has already existed in db
                bool IsScoreExisted = db.CRMScrores.Select(scr => scr.CRMForm.Id == id).FirstOrDefault();
                if (IsScoreExisted)
                {
                    return RedirectToAction("FinishingApprisal");
                }
                else
                {
                    ViewBag.QuestionList = db.CRMTemplates.ToList();
                    var CRMTeplates = db.CRMTemplates.ToList();
                    Service service = db.Services.Where(srv => srv.CRMFormId == id).FirstOrDefault();
                    Customer customer = new Customer();
                    if (service != null)
                    {
                        customer = service.Customer;
                    }
                    CRMModel CRMQuestion = new CRMModel();
                    CRMQuestion.Id = Convert.ToInt32(id);
                    CRMQuestion.CustomerName = customer.CustomerName;
                    foreach (var item in CRMTeplates)
                    {
                        Question question = new Question();
                        question.QuestionText = item.Question;
                        question.ID = item.Id;
                        question.Answers.Add(new Answer { ID = 1, AnswerText = "1" });
                        question.Answers.Add(new Answer { ID = 2, AnswerText = "2" });
                        question.Answers.Add(new Answer { ID = 3, AnswerText = "3" });
                        question.Answers.Add(new Answer { ID = 4, AnswerText = "4" });
                        question.Answers.Add(new Answer { ID = 5, AnswerText = "5" });
                        CRMQuestion.Questions.Add(question);
                    }
                    return View(CRMQuestion);
                }
                
            }
            else
            {
                return RedirectToAction("NoCRMPageFound");
            }
        }

        [HttpPost]
        public ActionResult CRMApprisal(CRMModel model, string TxtArea)
        {
            //Save to db
            CRMForm crmForm = new CRMForm();
            crmForm.Id = model.Id;
            crmForm.Feedback = TxtArea;
            crmForm.Timestamp = DateTime.Now;
            db.Entry(crmForm).State = EntityState.Modified;

            foreach (var item in model.Questions)
            {
                CRMScrore crmScore = new CRMScrore();
                crmScore.CRMForm = crmForm;
                crmScore.CRMTemplate = db.CRMTemplates.Find(item.ID);
                if (item.SelectedAnswer != null)
                    crmScore.Score = Convert.ToInt32(item.SelectedAnswer);
                else
                    crmScore.Score = 0;
                db.CRMScrores.Add(crmScore);
            }

            db.SaveChanges();

            return RedirectToAction("FinishingApprisal");
        }

        public ActionResult CRMCustomerRelation()
        {
            var CustomerList = db.Customers.Where(cus => cus.Services.Any(srv => srv.ServiceStatu.Id == Constant.SERVICE_STATUS_COMPLETE));
            List<CustomerRelation> customerRelations = new List<CustomerRelation>();
            foreach (Customer customer in CustomerList)
            {
                CustomerRelation _customer = new CustomerRelation 
                { 
                    CustomerId = customer.CustomerId,
                    CustomerName = customer.CustomerName,
                    CustomerAddress = customer.Address,
                    CustomerEmail = customer.Email,
                    CustomerPhoneNumber = customer.PhoneNumber,
                    AnniversaryDate = customer.AnniversaryDate,
                    ReferenceEmail = customer.ReferenceEmail,
                    ReferenceName = customer.ReferencePerson,
                    ReferencePhoneNumber = customer.ReferencePhoneNumber
                };
                customerRelations.Add(_customer);
            }
            return View(customerRelations.AsQueryable());
        }

        public async Task<ActionResult> CRMCustomerRelationDetails(int? id)
        {
            var customer = await db.Customers.FindAsync(id);
            CustomerRelation _customer = new CustomerRelation
            {
                CustomerId = customer.CustomerId,
                CustomerName = customer.CustomerName,
                CustomerAddress = customer.Address,
                CustomerEmail = customer.Email,
                CustomerPhoneNumber = customer.PhoneNumber,
                AnniversaryDate = customer.AnniversaryDate,
                ReferenceEmail = customer.ReferenceEmail,
                ReferenceName = customer.ReferencePerson,
                ReferencePhoneNumber = customer.ReferencePhoneNumber
            };
            return View(_customer);
        }

        [EnableCors(origins: "http://bdqabkk-pf26/olap/msmdpump.dll", headers: "*", methods: "*")]
        public ActionResult CRMReportCustomerRelation()
        {
            return View();
        }
    }
}
