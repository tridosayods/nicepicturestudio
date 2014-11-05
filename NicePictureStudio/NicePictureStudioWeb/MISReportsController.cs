using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NicePictureStudio.App_Data;
using Kendo.Mvc.Extensions;
using NicePictureStudio.Utils;
using NicePictureStudio.Models;
using System.Data;
using System.Data.Entity;
using Kendo.Mvc.UI;

namespace NicePictureStudio
{
    public class MISReportsController : Controller
    {
        private NCStudioDWEntities db = new NCStudioDWEntities();
        // GET: MISReports
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Cube_Read([DataSourceRequest]DataSourceRequest request)
        {
            var dwdata = db.PerformanceFacts;
            IQueryable<MISReportingModel> tasks = dwdata.Select(task => new MISReportingModel()
            {
                ApprisalScore = task.ApprisalScore == null ? 0 : task.ApprisalScore,
                ProfitPercent = task.ProfitPercent,
                SaleAmount = task.SaleAmount,
                CostAmount = task.CostAmount,
                Year = task.DimensionTime.Year,
                Quarter = task.DimensionTime.Quarter,
                Month = task.DimensionTime.Month,
                ServiceType = task.DimensionServiceType.ServiceType,
                ServiceTYpeCost = task.DimensionServiceType.Cost,
                PhotoGrapherName = task.DimensionPhotoGrapher.Name,
                Skill = task.DimensionPhotoGrapher.Skill,
                SalePerson = task.DimensionPromotion.SalePerson,
                PromotionType = task.DimensionPromotion.PromotionType,
                Profit = task.DimensionPromotion.Profit,
                BestScore = task.DimensionSatisfaction.BestScore,
                WorseScore = task.DimensionSatisfaction.WorseScore
            }
             ).AsQueryable();
            return Json(tasks.ToDataSourceResult(request));
        }

        public PartialViewResult MISReport01_Profit_Promotion()
        {
            var barChart = new List<BarChartViewModel>();
            var catagories = db.PerformanceFacts.GroupBy(p=>p.DimensionPromotion.PromotionType).Select(p=>p.FirstOrDefault());
            foreach (var item in catagories)
            {
                var chartItem = new BarChartViewModel();
                chartItem.Catalogs = item.DimensionPromotion.PromotionTypeName;
                chartItem.Data_1 = (decimal)item.DimensionServiceType.PerformanceFacts.Where(s => s.DimensionPromotion.PromotionType == item.DimensionPromotion.PromotionType).Average(a => a.ProfitPercent);
                barChart.Add(chartItem);
            }

            return PartialView(barChart);
        }

        public PartialViewResult MISReport02_Appraisal_Max_Min_ServiceType()
        {
            var barChart = new List<BarChartViewModel>();
            var catagories = db.PerformanceFacts.GroupBy(p => p.DimensionServiceType.ServiceTypeId).Select(p => p.FirstOrDefault());
            foreach (var item in catagories)
            {
                var chartItem = new BarChartViewModel();
                chartItem.Catalogs = item.DimensionServiceType.ServiceType;
                chartItem.Data_1 = (decimal)item.DimensionServiceType.PerformanceFacts.Where(s => s.DimensionServiceType.ServiceTypeId == item.DimensionServiceType.ServiceTypeId).Average(a => a.ApprisalScore);
                barChart.Add(chartItem);
            }
            return PartialView(barChart);
        }

        public PartialViewResult MISReport03_Appraisal_Max_Min_Skill()
        {
            var barChart = new List<BarChartViewModel>();
           // var catagories = db.PerformanceFacts.GroupBy(p => p.DimensionPhotoGrapher.SkillId).Select(p => p.FirstOrDefault());
            var catagories = db.PerformanceFacts.GroupBy(p => p.DimensionTime.Year).Select(p => p.FirstOrDefault());
            foreach (var item in catagories)
            {
                var subCatagories = db.PerformanceFacts.GroupBy(p => p.DimensionPhotoGrapher.SkillId).Select(p => p.FirstOrDefault());
                 var chartItem = new BarChartViewModel();
                 chartItem.Catalogs = item.DimensionTime.Year;
                foreach (var subitem in subCatagories)
                {
                   
                   
                    if (subitem.DimensionPhotoGrapher.SkillId == Constant.CUBE_SERVICE_CATEGORY_PHOTOGRAPH)
                    {
                        chartItem.Data_1 = (decimal)subitem.DimensionPhotoGrapher.PerformanceFacts.Where(s => s.DimensionPhotoGrapher.SkillId == subitem.DimensionPhotoGrapher.SkillId).Max(a => a.ApprisalScore);
                        chartItem.Data_2 = (decimal)subitem.DimensionPhotoGrapher.PerformanceFacts.Where(s => s.DimensionPhotoGrapher.SkillId == subitem.DimensionPhotoGrapher.SkillId).Min(a => a.ApprisalScore);
                    }
                    else if (subitem.DimensionPhotoGrapher.SkillId == Constant.CUBE_SERVICE_CATEGORY_CAMERAMAN)
                    {
                        chartItem.Data_Group1_1 = (decimal)subitem.DimensionPhotoGrapher.PerformanceFacts.Where(s => s.DimensionPhotoGrapher.SkillId == subitem.DimensionPhotoGrapher.SkillId).Max(a => a.ApprisalScore);
                        chartItem.Data_Group1_2 = (decimal)subitem.DimensionPhotoGrapher.PerformanceFacts.Where(s => s.DimensionPhotoGrapher.SkillId == subitem.DimensionPhotoGrapher.SkillId).Min(a => a.ApprisalScore);
                    }
                    else if (subitem.DimensionPhotoGrapher.SkillId == Constant.CUBE_SERVICE_CATEGORY_MEDIA)
                    {
                        chartItem.Data_Group2_1 = (decimal)subitem.DimensionPhotoGrapher.PerformanceFacts.Where(s => s.DimensionPhotoGrapher.SkillId == subitem.DimensionPhotoGrapher.SkillId).Max(a => a.ApprisalScore);
                        chartItem.Data_Group2_2 = (decimal)subitem.DimensionPhotoGrapher.PerformanceFacts.Where(s => s.DimensionPhotoGrapher.SkillId == subitem.DimensionPhotoGrapher.SkillId).Min(a => a.ApprisalScore);
                    }
                   
                }
                 barChart.Add(chartItem);
                           
            }
            return PartialView(barChart);
        }
    }
}