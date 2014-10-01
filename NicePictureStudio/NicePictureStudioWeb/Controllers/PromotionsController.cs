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
using System.Globalization;

namespace NicePictureStudio
{
    public class PromotionsController : Controller
    {
        private NicePictureStudioDBEntities db = new NicePictureStudioDBEntities();

        // GET: Promotions
        public async Task<ActionResult> Index()
        {
            return View(await db.Promotions.ToListAsync());
        }

        // GET: Promotions/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Promotion promotion = await db.Promotions.FindAsync(id);
            if (promotion == null)
            {
                return HttpNotFound();
            }
            return View(promotion);
        }

        // GET: Promotions/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Promotions/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,Name,CreateDate,ExpireDate,PhotoGraphDiscount,EquipmentDiscount,LocationDiscount,OutputDiscount,OutsourceDiscount")] Promotion promotion)
        {
            if (ModelState.IsValid)
            {
                db.Promotions.Add(promotion);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(promotion);
        }

        // GET: Promotions/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Promotion promotion = await db.Promotions.FindAsync(id);
            if (promotion == null)
            {
                return HttpNotFound();
            }
            return View(promotion);
        }

        // POST: Promotions/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,Name,CreateDate,ExpireDate,PhotoGraphDiscount,EquipmentDiscount,LocationDiscount,OutputDiscount,OutsourceDiscount")] Promotion promotion)
        {
            if (ModelState.IsValid)
            {
                db.Entry(promotion).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(promotion);
        }

        // GET: Promotions/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Promotion promotion = await db.Promotions.FindAsync(id);
            if (promotion == null)
            {
                return HttpNotFound();
            }
            return View(promotion);
        }

        // POST: Promotions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Promotion promotion = await db.Promotions.FindAsync(id);
            db.Promotions.Remove(promotion);
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

        public PartialViewResult PreWeddingCalculationDetail()
        {
            return PartialView();
        }

        public PartialViewResult SimpleCalculatePromotion(int PhotoGraphDiscount=0,int EquipmentDiscount=0,int LocationDiscount=0,int OutputDiscount=0,int OutsourceDiscount=0)
        {
            /*Static cost , price*/
            decimal photoGraphPreWeddingCost = 3000+4000+1000+1500+2000+2000;
            decimal photoGraphEngagementCost = 3000 + 4000 + 0 + 1500 + 2000 + 2000;
            decimal photoGraphWeddingCost = 6000 + 4000 + 0 + 2500 + 2000 + 2000;

            decimal photoGraphPreWeddingPrice = (3000 + (3000*2)) + (4000+(4000*1)) + (1000+(1000*1)) + (1500+(1500*1)) + (2000+(2000*(decimal)0.5))+(2000+(2000*3));
            decimal photoGraphEngagementPrice = (3000 + (3000 * 2)) + (4000 + (4000 * 1)) + (0 + (0 * 1)) + (1500 + (1500 * 1)) + (2000 + (2000 * (decimal)0.5)) + (2000 + (2000 * 3));
            decimal photoGraphWeddingPrice = (6000 + (6000 * 2)) + (4000 + (4000 * 1)) + (0 + (0 * 1)) + (2500 + (2500 * 1)) + (2000 + (2000 * (decimal)0.5)) + (2000 + (2000 * 3));

            decimal percentPhotoGraph = (decimal)PhotoGraphDiscount / (decimal)100;
            decimal percentEquipment = (decimal)EquipmentDiscount / (decimal)100;
            decimal percentLocation = (decimal)LocationDiscount / (decimal)100;
            decimal percentOutsource = (decimal)OutputDiscount / (decimal)100;
            decimal percentOutput = (decimal)OutputDiscount / (decimal)100;

            decimal EstimatePreWeddingPrice = (3000 + (3000 * (percentPhotoGraph))) + (4000 + (4000 * percentPhotoGraph)) + (1000 + (1000 * percentEquipment)) + (1500 + (1500 * percentLocation)) + (2000 + (2000 * percentOutsource)) + (2000 + (2000 * percentOutput));
            decimal EstimateEngagementPrice = (3000 + (3000 * (percentPhotoGraph))) + (4000 + (4000 * percentPhotoGraph)) + (0 + (0 * percentEquipment)) + (1500 + (1500 * percentLocation)) + (2000 + (2000 * percentOutsource)) + (2000 + (2000 * percentOutput));
            decimal EstimateWeddingPrice = (6000 + (6000 * (percentPhotoGraph))) + (4000 + (4000 * percentPhotoGraph)) + (0 + (0 * percentEquipment)) + (2500 + (2500 * percentLocation)) + (2000 + (2000 * percentOutsource)) + (2000 + (2000 * percentOutput));
            
            decimal percentageStaticPrice = (((photoGraphPreWeddingPrice+photoGraphEngagementPrice+photoGraphWeddingPrice) - (photoGraphPreWeddingCost+photoGraphEngagementCost+photoGraphWeddingCost)) / (photoGraphPreWeddingCost+photoGraphEngagementCost+photoGraphWeddingCost));
            decimal percentageNewPrice = (((EstimatePreWeddingPrice + EstimateEngagementPrice + EstimateWeddingPrice) - (photoGraphPreWeddingCost + photoGraphEngagementCost + photoGraphWeddingCost)) / (photoGraphPreWeddingCost + photoGraphEngagementCost + photoGraphWeddingCost));

            PromotionResult result = new PromotionResult();
            result.DiscountSummary = (RoundUp(percentageNewPrice, 2)).ToString("P", CultureInfo.CurrentCulture);
            result.lowestDiscount = (RoundUp(percentageStaticPrice, 2)).ToString("P", CultureInfo.CurrentCulture);
            return PartialView(result);
        }

        private decimal RoundUp(decimal number, int places)
        {
            decimal factor = RoundFactor(places);
            number *= factor;
            number = Math.Ceiling(number);
            number /= factor;
            return number;
        }

        private decimal RoundDown(decimal number, int places)
        {
            decimal factor = RoundFactor(places);
            number *= factor;
            number = Math.Floor(number);
            number /= factor;
            return number;
        }

        internal decimal RoundFactor(int places)
        {
            decimal factor = 1m;

            if (places < 0)
            {
                places = -places;
                for (int i = 0; i < places; i++)
                    factor /= 10m;
            }

            else
            {
                for (int i = 0; i < places; i++)
                    factor *= 10m;
            }

            return factor;
        }
    }
}
