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
            var catagories = db.PerformanceFacts.GroupBy(p => p.DimensionServiceType.ServiceId).Select(p => p.FirstOrDefault());
            foreach (var item in catagories)
            {
                var chartItem = new BarChartViewModel();
                chartItem.Catalogs = item.DimensionServiceType.ServiceType;
                chartItem.Data_1 = (decimal)item.DimensionServiceType.PerformanceFacts.Where(s => s.DimensionServiceType.ServiceId == item.DimensionServiceType.ServiceId).Max(a => a.ApprisalScore);
                chartItem.Data_Group1_1 = (decimal)item.DimensionServiceType.PerformanceFacts.Where(s => s.DimensionServiceType.ServiceId == item.DimensionServiceType.ServiceId).Min(a => a.ApprisalScore);
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

        public PartialViewResult MISReport04_Profit_Year()
        {
            var barChart = new List<BarChartViewModel>();
            var catagories = db.PerformanceFacts.GroupBy(p => p.DimensionTime.Year).Select(p => p.FirstOrDefault());
            foreach (var item in catagories)
            {
                var chartItem = new BarChartViewModel();
                chartItem.Catalogs = item.DimensionTime.Year;
                chartItem.Data_1 = (decimal)item.DimensionTime.PerformanceFacts.Where(s => s.DimensionTime.Year == item.DimensionTime.Year).Sum(a => (a.SaleAmount - a.CostAmount));
                barChart.Add(chartItem);
            }
            return PartialView(barChart);
        }

        public PartialViewResult MISReport05_Profit_Trend()
        {
            var lineChart = new List<LineChartViewModel>();
            var catagories = db.PerformanceFacts.GroupBy(p => new{ p.DimensionTime.Month,p.DimensionTime.Year}).Select(p => p.FirstOrDefault());
            foreach (var item in catagories)
            {
                var chartItem = new LineChartViewModel();
                chartItem.Catalogs = item.DimensionTime.Year + "/" + item.DimensionTime.Month;
                chartItem.Data_1 = (decimal)item.DimensionTime.PerformanceFacts.Where(s => (s.DimensionTime.Month == item.DimensionTime.Month)
                    &&(s.DimensionTime.Year == item.DimensionTime.Year)).Sum(a => (a.SaleAmount - a.CostAmount));
                lineChart.Add(chartItem);
            }
            return PartialView(lineChart);
        }

        

        public PartialViewResult MISReport06_Apprisal_Skill()
        {
            var lineChart = new List<LineChartViewModel>();
            var catagories = db.PerformanceFacts.GroupBy(p => new { p.DimensionTime.Year, p.DimensionTime.Quarter }).Select(p => p.FirstOrDefault());
            foreach (var item in catagories)
            {
                var subCategories = db.PerformanceFacts.GroupBy(p => p.DimensionPhotoGrapher.SkillId).Select(p => p.FirstOrDefault());
                var chartItem = new LineChartViewModel();
                chartItem.Catalogs = item.DimensionTime.Year + "/ Quarter" + item.DimensionTime.Quarter;
                foreach (var subitem in subCategories)
                {
                    if (subitem.DimensionPhotoGrapher.SkillId == Constant.CUBE_SERVICE_CATEGORY_PHOTOGRAPH)
                    {
                        chartItem.Data_1 = Convert.ToDecimal(subitem.DimensionPhotoGrapher.PerformanceFacts.Where(s => s.DimensionPhotoGrapher.SkillId == subitem.DimensionPhotoGrapher.SkillId
                            && s.DimensionTime.Year == item.DimensionTime.Year && s.DimensionTime.Quarter == item.DimensionTime.Quarter
                            ).Average(a => a.ApprisalScore));
                    }
                    else if (subitem.DimensionPhotoGrapher.SkillId == Constant.CUBE_SERVICE_CATEGORY_CAMERAMAN)
                    {
                        chartItem.Data_Group1_1 = Convert.ToDecimal(subitem.DimensionPhotoGrapher.PerformanceFacts.Where(s => s.DimensionPhotoGrapher.SkillId == subitem.DimensionPhotoGrapher.SkillId
                            && s.DimensionTime.Year == item.DimensionTime.Year && s.DimensionTime.Quarter == item.DimensionTime.Quarter
                            ).Average(a => a.ApprisalScore));
                    }
                    else if (subitem.DimensionPhotoGrapher.SkillId == Constant.CUBE_SERVICE_CATEGORY_MEDIA)
                    {
                        chartItem.Data_Group2_1 = Convert.ToDecimal(subitem.DimensionPhotoGrapher.PerformanceFacts.Where(s => s.DimensionPhotoGrapher.SkillId == subitem.DimensionPhotoGrapher.SkillId
                            ).Average(a => a.ApprisalScore));
                    }

                }
                lineChart.Add(chartItem);
            }
            return PartialView(lineChart);
        }

        public PartialViewResult MISReport07_Appraisal_ServiceType()
        {
            var lineChart = new List<LineChartViewModel>();
            var catagories = db.PerformanceFacts.GroupBy(p => new { p.DimensionTime.Year, p.DimensionTime.Quarter }).Select(p => p.FirstOrDefault());
            foreach (var item in catagories)
            {
                var subCategories = db.PerformanceFacts.GroupBy(p => p.DimensionServiceType.ServiceId).Select(p => p.FirstOrDefault());
                var chartItem = new LineChartViewModel();
                chartItem.Catalogs = item.DimensionTime.Year + "/ Quarter" + item.DimensionTime.Quarter;
                foreach (var subitem in subCategories)
                {
                    if (subitem.DimensionServiceType.ServiceId == Constant.CUBE_SERVICE_TYPE_PREWEDDING)
                    {
                        chartItem.Data_1 = Convert.ToDecimal(subitem.DimensionServiceType.PerformanceFacts.Where(s => s.DimensionServiceType.ServiceId == subitem.DimensionServiceType.ServiceId
                            && s.DimensionTime.Year == item.DimensionTime.Year && s.DimensionTime.Quarter == item.DimensionTime.Quarter
                            ).Average(a => a.ApprisalScore));
                    }
                    else if (subitem.DimensionServiceType.ServiceId == Constant.CUBE_SERVICE_TYPE_ENGAGEMENT)
                    {
                        chartItem.Data_Group1_1 = Convert.ToDecimal(subitem.DimensionServiceType.PerformanceFacts.Where(s => s.DimensionServiceType.ServiceId == subitem.DimensionServiceType.ServiceId
                            && s.DimensionTime.Year == item.DimensionTime.Year && s.DimensionTime.Quarter == item.DimensionTime.Quarter
                            ).Average(a => a.ApprisalScore));
                    }
                    else if (subitem.DimensionServiceType.ServiceId == Constant.CUBE_SERVICE_TYPE_WEDDING)
                    {
                        chartItem.Data_Group2_1 = Convert.ToDecimal(subitem.DimensionServiceType.PerformanceFacts.Where(s => s.DimensionServiceType.ServiceId == subitem.DimensionServiceType.ServiceId
                            ).Average(a => a.ApprisalScore));
                    }

                }
                lineChart.Add(chartItem);
            }
            return PartialView(lineChart);
        }

        public PartialViewResult MISReport08_Profit_ServiceType()
        {
            var barChart = new List<BarChartViewModel>();
            var catagories = db.PerformanceFacts.GroupBy(p => p.DimensionServiceType.ServiceId).Select(p => p.FirstOrDefault());
            foreach (var item in catagories)
            {
                var chartItem = new BarChartViewModel();
                chartItem.Catalogs = item.DimensionServiceType.ServiceType;
                chartItem.Data_1 = (decimal)item.DimensionServiceType.PerformanceFacts.Where(s => s.DimensionServiceType.ServiceId == item.DimensionServiceType.ServiceId).Sum(a=> a.SaleAmount - a.CostAmount);
                chartItem.Data_Group1_1 = (decimal)item.DimensionServiceType.PerformanceFacts.Where(s => s.DimensionServiceType.ServiceId == item.DimensionServiceType.ServiceId).Sum(a => a.CostAmount);
                barChart.Add(chartItem);
            }
            return PartialView(barChart);
        }

    }
}