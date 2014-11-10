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
    public class GeneralReportsController : Controller
    {
        private NCStudioDWEntities db = new NCStudioDWEntities();
        
        // GET: GeneralReports
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult IncomeReport()
        {
            return View();
        }

        public ActionResult ApprisalReport()
        {
            return View();
        }

        public ActionResult CostReport()
        {
            return View();
        }

        public PartialViewResult GEReport01_Income_Cost()
        {
            var barChart = new List<BarChartViewModel>();
            var catagories = db.PerformanceFacts.GroupBy(p => p.DimensionTime.Year).Select(p => p.FirstOrDefault());
            foreach (var item in catagories)
            {
                var chartItem = new BarChartViewModel();
                chartItem.Catalogs = item.DimensionTime.Year;
                //chartItem.Data_1 = (decimal)item.DimensionTime.PerformanceFacts.Where(s => s.DimensionTime.Year == item.DimensionTime.Year).Sum(a => (a.SaleAmount));
                chartItem.Data_1 = (decimal)db.PerformanceFacts.Where(s => s.DimensionTime.Year == item.DimensionTime.Year).Sum(a => (a.SaleAmount));
                chartItem.Data_Group1_1 = (decimal)db.PerformanceFacts.Where(s => s.DimensionTime.Year == item.DimensionTime.Year).Sum(a => (a.SaleAmount - a.CostAmount));
                barChart.Add(chartItem);
            }
            return PartialView(barChart);
        }

        public PartialViewResult GEReport01_02_Income_Cost_Table()
        {
            return PartialView();
        }

        public PartialViewResult GEReport02_Income_Quarter()
        {
            var barChart = new List<BarChartViewModel>();
            var catagories = db.PerformanceFacts.GroupBy(p => new { p.DimensionTime.Year, p.DimensionTime.Month }).Select(p => p.FirstOrDefault()).OrderBy(a => a.DimensionTime.TimeId); 
            foreach (var item in catagories)
            {
                var chartItem = new BarChartViewModel();
                chartItem.Catalogs = item.DimensionTime.Year +"/"+ item.DimensionTime.Month;
                chartItem.Data_1 = (decimal)db.PerformanceFacts.Where(s => s.DimensionTime.Year == item.DimensionTime.Year 
                    && s.DimensionTime.Month == item.DimensionTime.Month ).Sum(a => (a.SaleAmount));
                barChart.Add(chartItem);
            }
            return PartialView(barChart);
        }

        public PartialViewResult GEReport03_Imcome_Promotion()
        {
            var barChart = new List<BarChartViewModel>();
            var catagories = db.PerformanceFacts.GroupBy(p => new { p.DimensionTime.Year }).Select(p => p.FirstOrDefault());
            foreach (var item in catagories)
            {
                var chartItem = new BarChartViewModel();
                chartItem.Catalogs = item.DimensionTime.Year;
                var subCatagories = db.PerformanceFacts.GroupBy(p => p.DimensionPromotion.PromotionType).Select(p => p.FirstOrDefault());

                foreach (var subitem in subCatagories)
                {
                    if (subitem.DimensionPromotion.PromotionType == Constant.CUBE_PROMOTION_TYPE_1)
                    {
                        chartItem.Data_1 = (decimal)db.PerformanceFacts.Where(s => s.DimensionTime.Year == item.DimensionTime.Year
                       && s.DimensionPromotion.PromotionType == Constant.CUBE_PROMOTION_TYPE_1).Sum(a => (a.SaleAmount - a.CostAmount));
                    }
                    else if (subitem.DimensionPromotion.PromotionType == Constant.CUBE_PROMOTION_TYPE_2)
                    {
                        chartItem.Data_Group1_1 = (decimal)db.PerformanceFacts.Where(s => s.DimensionTime.Year == item.DimensionTime.Year
                      && s.DimensionPromotion.PromotionType == Constant.CUBE_PROMOTION_TYPE_2).Sum(a => (a.SaleAmount -a.CostAmount));
                        
                    }
                    else if (subitem.DimensionPromotion.PromotionType == Constant.CUBE_PROMOTION_TYPE_3)
                    {
                        chartItem.Data_Group2_1 = (decimal)db.PerformanceFacts.Where(s => s.DimensionTime.Year == item.DimensionTime.Year
                     && s.DimensionPromotion.PromotionType == Constant.CUBE_PROMOTION_TYPE_3).Sum(a => (a.SaleAmount - a.CostAmount));

                    }
                   
                }
                barChart.Add(chartItem);
            }
            return PartialView(barChart);
        }

        public PartialViewResult GEReport04_Profit_Percent_Time()
        {
            var lineChart = new List<LineChartViewModel>();
            var catagories = db.PerformanceFacts.GroupBy(p => new { p.DimensionTime.Year, p.DimensionTime.Quarter }).Select(p => p.FirstOrDefault()).OrderBy(o=>o.TimeId);
            foreach (var item in catagories)
            {
                var chartItem = new LineChartViewModel();
                chartItem.Catalogs = item.DimensionTime.Year + "/ Quarter: " + item.DimensionTime.Quarter;
                chartItem.Data_1 = (decimal)db.PerformanceFacts.Where(s => s.DimensionTime.Year == item.DimensionTime.Year
                    && s.DimensionTime.Quarter == item.DimensionTime.Quarter).Average(a => (a.ProfitPercent));
                lineChart.Add(chartItem);
            }
            return PartialView(lineChart);
        }

        public PartialViewResult GEReport05_Promotion_Time()
        {
            var lineChart = new List<LineChartViewModel>();
            var catagories = db.PerformanceFacts.GroupBy(p => new { p.DimensionTime.Year, p.DimensionTime.Quarter }).Select(p => p.FirstOrDefault()).OrderBy(o => o.TimeId);
            foreach (var item in catagories)
            {
                var chartItem = new LineChartViewModel();
                chartItem.Catalogs = item.DimensionTime.Year + "/ Quarter: " + item.DimensionTime.Quarter;
                var subCatagories = db.PerformanceFacts.GroupBy(p => p.DimensionPromotion.PromotionType).Select(p => p.FirstOrDefault());

                foreach (var subitem in subCatagories)
                {
                    if (subitem.DimensionPromotion.PromotionType == Constant.CUBE_PROMOTION_TYPE_1)
                    {
                        chartItem.Data_1 = (decimal)db.PerformanceFacts.Where(s => s.DimensionTime.Year == item.DimensionTime.Year
                       && s.DimensionPromotion.PromotionType == Constant.CUBE_PROMOTION_TYPE_1
                       && s.DimensionTime.Quarter == item.DimensionTime.Quarter).Sum(a => (a.SaleAmount));
                    }
                    else if (subitem.DimensionPromotion.PromotionType == Constant.CUBE_PROMOTION_TYPE_2)
                    {
                        chartItem.Data_Group1_1 = (decimal)db.PerformanceFacts.Where(s => s.DimensionTime.Year == item.DimensionTime.Year
                      && s.DimensionPromotion.PromotionType == Constant.CUBE_PROMOTION_TYPE_2
                      && s.DimensionTime.Quarter == item.DimensionTime.Quarter).Sum(a => (a.SaleAmount));

                    }
                    else if (subitem.DimensionPromotion.PromotionType == Constant.CUBE_PROMOTION_TYPE_3)
                    {
                        chartItem.Data_Group2_1 = (decimal)db.PerformanceFacts.Where(s => s.DimensionTime.Year == item.DimensionTime.Year
                     && s.DimensionPromotion.PromotionType == Constant.CUBE_PROMOTION_TYPE_3
                    && s.DimensionTime.Quarter == item.DimensionTime.Quarter).Sum(a => (a.SaleAmount));
                    }

                }
                lineChart.Add(chartItem);
            }
            return PartialView(lineChart);
        }

        public PartialViewResult GEReport06_Profit_ServiceType()
        {
            var barChart = new List<BarChartViewModel>();
            var catagories = db.PerformanceFacts.GroupBy(p => new { p.DimensionTime.Year }).Select(p => p.FirstOrDefault());
            foreach (var item in catagories)
            {
                var chartItem = new BarChartViewModel();
                chartItem.Catalogs = item.DimensionTime.Year;
                var subCatagories = db.PerformanceFacts.GroupBy(p => p.DimensionServiceType.ServiceId).Select(p => p.FirstOrDefault());

                foreach (var subitem in subCatagories)
                {
                    if (subitem.DimensionServiceType.ServiceId == Constant.CUBE_SERVICE_TYPE_PREWEDDING)
                    {
                        chartItem.Data_1 = (decimal)db.PerformanceFacts.Where(s => s.DimensionTime.Year == item.DimensionTime.Year
                       && s.DimensionServiceType.ServiceId == Constant.CUBE_SERVICE_TYPE_PREWEDDING).Sum(a => (a.SaleAmount));
                        chartItem.Data_2 = (decimal)db.PerformanceFacts.Where(s => s.DimensionTime.Year == item.DimensionTime.Year
                      && s.DimensionServiceType.ServiceId == Constant.CUBE_SERVICE_TYPE_PREWEDDING).Sum(a => (a.SaleAmount - a.CostAmount));
                    }
                    else if (subitem.DimensionServiceType.ServiceId == Constant.CUBE_SERVICE_TYPE_ENGAGEMENT)
                    {
                        chartItem.Data_Group1_1 = (decimal)db.PerformanceFacts.Where(s => s.DimensionTime.Year == item.DimensionTime.Year
                      && s.DimensionServiceType.ServiceId == Constant.CUBE_SERVICE_TYPE_ENGAGEMENT).Sum(a => (a.SaleAmount));
                        chartItem.Data_Group1_2 = (decimal)db.PerformanceFacts.Where(s => s.DimensionTime.Year == item.DimensionTime.Year
                      && s.DimensionServiceType.ServiceId == Constant.CUBE_SERVICE_TYPE_ENGAGEMENT).Sum(a => (a.SaleAmount - a.CostAmount));

                    }
                    else if (subitem.DimensionServiceType.ServiceId == Constant.CUBE_SERVICE_TYPE_WEDDING)
                    {
                        chartItem.Data_Group2_1 = (decimal)db.PerformanceFacts.Where(s => s.DimensionTime.Year == item.DimensionTime.Year
                     && s.DimensionServiceType.ServiceId == Constant.CUBE_SERVICE_TYPE_WEDDING).Sum(a => (a.SaleAmount));
                        chartItem.Data_Group2_2 = (decimal)db.PerformanceFacts.Where(s => s.DimensionTime.Year == item.DimensionTime.Year
                    && s.DimensionServiceType.ServiceId == Constant.CUBE_SERVICE_TYPE_WEDDING).Sum(a => (a.SaleAmount - a.CostAmount));

                    }

                }
                barChart.Add(chartItem);
            }
            return PartialView(barChart);
        }

        public PartialViewResult GEReport07_Cost_Income()
        {
            var barChart = new List<BarChartViewModel>();
            var catagories = db.PerformanceFacts.GroupBy(p => p.DimensionTime.Year).Select(p => p.FirstOrDefault());
            foreach (var item in catagories)
            {
                var chartItem = new BarChartViewModel();
                chartItem.Catalogs = item.DimensionTime.Year;
                chartItem.Data_1 = (decimal)db.PerformanceFacts.Where(s => s.DimensionTime.Year == item.DimensionTime.Year).Sum(a => (a.SaleAmount));
                chartItem.Data_Group1_1 = (decimal)db.PerformanceFacts.Where(s => s.DimensionTime.Year == item.DimensionTime.Year).Sum(a => (a.CostAmount));
                barChart.Add(chartItem);
            }
            return PartialView(barChart);
        }

        public PartialViewResult GEReport08_Sale_Income()
        {
            var barChart = new List<BarChartViewModel>();
            var catagories = db.PerformanceFacts.GroupBy(p => new { p.DimensionTime.Year, p.DimensionPromotion.SaleId }).Select(p => p.FirstOrDefault());
            foreach (var item in catagories)
            {
                var subCategories = db.PerformanceFacts.GroupBy(p => p.DimensionServiceType.ServiceId).Select(p => p.FirstOrDefault());
                var chartItem = new BarChartViewModel();
                chartItem.Catalogs = item.DimensionTime.Year + "/" + item.DimensionPromotion.SalePerson;
                foreach (var subitem in subCategories)
                {
                    if (subitem.DimensionServiceType.ServiceId == Constant.CUBE_SERVICE_TYPE_PREWEDDING)
                    {
                        chartItem.Data_1 = Convert.ToDecimal(db.PerformanceFacts.Where(s => s.DimensionServiceType.ServiceId == subitem.DimensionServiceType.ServiceId
                            && s.DimensionTime.Year == item.DimensionTime.Year && s.DimensionPromotion.SaleId == item.DimensionPromotion.SaleId
                            ).Sum(a => a.SaleAmount));
                    }
                    else if (subitem.DimensionServiceType.ServiceId == Constant.CUBE_SERVICE_TYPE_ENGAGEMENT)
                    {
                        chartItem.Data_Group1_1 = Convert.ToDecimal(db.PerformanceFacts.Where(s => s.DimensionServiceType.ServiceId == subitem.DimensionServiceType.ServiceId
                            && s.DimensionTime.Year == item.DimensionTime.Year && s.DimensionPromotion.SaleId == item.DimensionPromotion.SaleId
                            ).Sum(a => a.SaleAmount));
                    }
                    else if (subitem.DimensionServiceType.ServiceId == Constant.CUBE_SERVICE_TYPE_WEDDING)
                    {
                        chartItem.Data_Group2_1 = Convert.ToDecimal(db.PerformanceFacts.Where(s => s.DimensionServiceType.ServiceId == subitem.DimensionServiceType.ServiceId
                             && s.DimensionTime.Year == item.DimensionTime.Year && s.DimensionPromotion.SaleId == item.DimensionPromotion.SaleId
                            ).Sum(a => a.SaleAmount));
                    }

                }
                barChart.Add(chartItem);
            }
            return PartialView(barChart);
        }

        public PartialViewResult GEReport09_Apprisal_ServiceType()
        {
            var barChart = new List<BarChartViewModel>();
            var catagories = db.PerformanceFacts.GroupBy(p => p.DimensionServiceType.ServiceId).Select(p => p.FirstOrDefault());
            foreach (var item in catagories)
            {
                var chartItem = new BarChartViewModel();
                chartItem.Catalogs = item.DimensionServiceType.ServiceType;
               //Apprisal Average for scrore 1-2
                chartItem.Data_1 = (decimal)db.PerformanceFacts.Where(s => s.DimensionServiceType.ServiceId == item.DimensionServiceType.ServiceId
                    && s.ApprisalScore == 1).Sum(a => a.SaleAmount);
                chartItem.Data_2 = (decimal)db.PerformanceFacts.Where(s => s.DimensionServiceType.ServiceId == item.DimensionServiceType.ServiceId
                    && (s.ApprisalScore == 2)).Sum(a => a.SaleAmount);
                chartItem.Data_Group1_1 = (decimal)db.PerformanceFacts.Where(s => s.DimensionServiceType.ServiceId == item.DimensionServiceType.ServiceId
                  && s.ApprisalScore == 3).Sum(a => a.SaleAmount);
                chartItem.Data_Group1_2 = (decimal)db.PerformanceFacts.Where(s => s.DimensionServiceType.ServiceId == item.DimensionServiceType.ServiceId
                    &&  s.ApprisalScore == 4).Sum(a => a.SaleAmount);
                chartItem.Data_Group2_1 = (decimal)db.PerformanceFacts.Where(s => s.DimensionServiceType.ServiceId == item.DimensionServiceType.ServiceId
                    && s.ApprisalScore == 5).Sum(a => a.SaleAmount);
                barChart.Add(chartItem);
            }
            return PartialView(barChart);
        }

        public PartialViewResult GEReport10_Apprisal_Photographer()
        {
            var barChart = new List<BarChartViewModel>();
            var catagories = db.PerformanceFacts.GroupBy(p => p.DimensionPhotoGrapher.PhotoGraphId).Select(p => p.FirstOrDefault());
            foreach (var item in catagories)
            {
                var chartItem = new BarChartViewModel();
                chartItem.Catalogs = item.DimensionPhotoGrapher.Name;
                //Apprisal Average for scrore 1-2
                chartItem.Data_1 = (decimal)db.PerformanceFacts.Where(s => s.DimensionPhotoGrapher.PhotoGraphId == item.DimensionPhotoGrapher.PhotoGraphId
                    && (s.ApprisalScore == 1)).Sum(a => a.SaleAmount);
                chartItem.Data_2 = (decimal)db.PerformanceFacts.Where(s => s.DimensionPhotoGrapher.PhotoGraphId == item.DimensionPhotoGrapher.PhotoGraphId
                  && s.ApprisalScore == 2).Sum(a => a.SaleAmount);
                chartItem.Data_Group1_1 = (decimal)db.PerformanceFacts.Where(s => s.DimensionPhotoGrapher.PhotoGraphId == item.DimensionPhotoGrapher.PhotoGraphId
                    && s.ApprisalScore == 3).Sum(a => a.SaleAmount);
                chartItem.Data_Group1_2 = (decimal)db.PerformanceFacts.Where(s => s.DimensionPhotoGrapher.PhotoGraphId == item.DimensionPhotoGrapher.PhotoGraphId
                    && s.ApprisalScore == 4).Sum(a => a.SaleAmount);
                chartItem.Data_Group2_1 = (decimal)db.PerformanceFacts.Where(s => s.DimensionPhotoGrapher.PhotoGraphId == item.DimensionPhotoGrapher.PhotoGraphId
                    && s.ApprisalScore == 5).Sum(a => a.SaleAmount);
                barChart.Add(chartItem);
            }
            return PartialView(barChart);
        }

        public PartialViewResult GEReport11_Apprisal_Overall()
        {
            var barChart = new List<BarChartViewModel>();
            var catagories = db.PerformanceFacts.GroupBy(p => p.DimensionServiceType.ServiceId).Select(p => p.FirstOrDefault());
            foreach (var item in catagories)
            {
                var chartItem = new BarChartViewModel();
                chartItem.Catalogs = item.DimensionServiceType.ServiceType;
                //Apprisal Average for scrore 1-2
                chartItem.Data_1 = (decimal)db.PerformanceFacts.Where(s => s.DimensionServiceType.ServiceId == item.DimensionServiceType.ServiceId
                    && s.ApprisalScore == 5).Count();
                chartItem.Data_2 = (decimal)db.PerformanceFacts.Where(s => s.DimensionServiceType.ServiceId == item.DimensionServiceType.ServiceId
                    && (s.ApprisalScore == 4)).Count();
                chartItem.Data_Group1_1 = (decimal)db.PerformanceFacts.Where(s => s.DimensionServiceType.ServiceId == item.DimensionServiceType.ServiceId
                  && s.ApprisalScore == 3).Count();
                chartItem.Data_Group1_2 = (decimal)db.PerformanceFacts.Where(s => s.DimensionServiceType.ServiceId == item.DimensionServiceType.ServiceId
                    && s.ApprisalScore == 2).Count();
                chartItem.Data_Group2_1 = (decimal)db.PerformanceFacts.Where(s => s.DimensionServiceType.ServiceId == item.DimensionServiceType.ServiceId
                    && s.ApprisalScore == 1).Count();
                barChart.Add(chartItem);
            }
            return PartialView(barChart);
        }

        public PartialViewResult GEReport12_Apprisal_Promotion()
        {
            var barChart = new List<BarChartViewModel>();
            var catagories = db.PerformanceFacts.GroupBy(p => p.DimensionPromotion.PromotionType).Select(p => p.FirstOrDefault());
            foreach (var item in catagories)
            {
                var chartItem = new BarChartViewModel();
                chartItem.Catalogs = item.DimensionPromotion.PromotionTypeName;
                //Apprisal Average for scrore 1-2
                chartItem.Data_1 = (decimal)db.PerformanceFacts.Where(s => s.DimensionPromotion.PromotionTypeName == item.DimensionPromotion.PromotionTypeName
                    && s.ApprisalScore == 5).Count();
                chartItem.Data_2 = (decimal)db.PerformanceFacts.Where(s => s.DimensionPromotion.PromotionTypeName == item.DimensionPromotion.PromotionTypeName
                    && (s.ApprisalScore == 4)).Count();
                chartItem.Data_Group1_1 = (decimal)db.PerformanceFacts.Where(s => s.DimensionPromotion.PromotionTypeName == item.DimensionPromotion.PromotionTypeName
                  && s.ApprisalScore == 3).Count();
                chartItem.Data_Group1_2 = (decimal)db.PerformanceFacts.Where(s => s.DimensionPromotion.PromotionTypeName == item.DimensionPromotion.PromotionTypeName
                    && s.ApprisalScore == 2).Count();
                chartItem.Data_Group2_1 = (decimal)db.PerformanceFacts.Where(s => s.DimensionPromotion.PromotionTypeName == item.DimensionPromotion.PromotionTypeName
                    && s.ApprisalScore == 1).Count();
                barChart.Add(chartItem);
            }
            return PartialView(barChart);
        }

        public PartialViewResult GEReport13_Apprisal_Year()
        {
            var barChart = new List<BarChartViewModel>();
            var catagories = db.PerformanceFacts.GroupBy(p => p.DimensionTime.Year).Select(p => p.FirstOrDefault());
            foreach (var item in catagories)
            {
                var chartItem = new BarChartViewModel();
                chartItem.Catalogs = item.DimensionTime.Year;
                //Apprisal Average for scrore 1-2
                chartItem.Data_1 = (decimal)db.PerformanceFacts.Where(s => s.DimensionTime.Year == item.DimensionTime.Year
                    && s.ApprisalScore == 5).Count();
                chartItem.Data_2 = (decimal)db.PerformanceFacts.Where(s => s.DimensionTime.Year == item.DimensionTime.Year
                    && (s.ApprisalScore == 4)).Count();
                chartItem.Data_Group1_1 = (decimal)db.PerformanceFacts.Where(s => s.DimensionTime.Year == item.DimensionTime.Year
                  && s.ApprisalScore == 3).Count();
                chartItem.Data_Group1_2 = (decimal)db.PerformanceFacts.Where(s => s.DimensionTime.Year == item.DimensionTime.Year
                    && s.ApprisalScore == 2).Count();
                chartItem.Data_Group2_1 = (decimal)db.PerformanceFacts.Where(s => s.DimensionTime.Year == item.DimensionTime.Year
                    && s.ApprisalScore == 1).Count();
                barChart.Add(chartItem);
            }
            return PartialView(barChart);
        }

        public PartialViewResult GEReport14_Cancel_ServiceType()
        {
            var barChart = new List<BarChartViewModel>();
            var catagories = db.PerformanceFacts.GroupBy(p => p.DimensionServiceType.ServiceId).Select(p => p.FirstOrDefault());
            foreach (var item in catagories)
            {
                var chartItem = new BarChartViewModel();
                chartItem.Catalogs = item.DimensionServiceType.ServiceType;
                chartItem.Data_1 = (decimal)db.PerformanceFacts.Where(s => s.DimensionServiceType.ServiceId == item.DimensionServiceType.ServiceId
                    && s.IsCancel == true).Count();
                barChart.Add(chartItem);
            }
            return PartialView(barChart);
        }

        public PartialViewResult GEReport15_Cost_Overall()
        {
            var barChart = new List<BarChartViewModel>();
            var catagories = db.PerformanceFacts.GroupBy(p => p.DimensionTime.Year).Select(p => p.FirstOrDefault());
            foreach (var item in catagories)
            {
                var chartItem = new BarChartViewModel();
                chartItem.Catalogs = item.DimensionTime.Year;
                chartItem.Data_1 = (decimal)db.PerformanceFacts.Where(s => s.DimensionTime.Year == item.DimensionTime.Year
                   ).Sum(a => a.CostAmount);
                barChart.Add(chartItem);
            }
            return PartialView(barChart);
        }

        public PartialViewResult GEReport16_Cost_Time()
        {
            var barChart = new List<BarChartViewModel>();
            var catagories = db.PerformanceFacts.GroupBy(p => new { p.DimensionTime.Year, p.DimensionTime.Month }).Select(p => p.FirstOrDefault()).OrderBy(a=>a.DimensionTime.TimeId);
            foreach (var item in catagories)
            {
                var chartItem = new BarChartViewModel();
                chartItem.Catalogs = item.DimensionTime.Year + "/" + item.DimensionTime.Month;
                chartItem.Data_1 = (decimal)db.PerformanceFacts.Where(s => s.DimensionTime.Year == item.DimensionTime.Year
                    && s.DimensionTime.Month == item.DimensionTime.Month).Sum(a => (a.CostAmount));
                barChart.Add(chartItem);
            }
            return PartialView(barChart);
        }

        public PartialViewResult GEReport17_Cost_Promotion()
        {
            var barChart = new List<BarChartViewModel>();
            var catagories = db.PerformanceFacts.GroupBy(p => new { p.DimensionTime.Year,p.DimensionTime.Quarter }).Select(p => p.FirstOrDefault()).OrderBy(a=>a.DimensionTime.TimeId);
            foreach (var item in catagories)
            {
                var chartItem = new BarChartViewModel();
                chartItem.Catalogs = item.DimensionTime.Year +"/Quarter: " + item.DimensionTime.Quarter ;
                var subCatagories = db.PerformanceFacts.GroupBy(p => p.DimensionPromotion.PromotionType).Select(p => p.FirstOrDefault());

                foreach (var subitem in subCatagories)
                {
                    try
                    {
                        if (subitem.DimensionPromotion.PromotionType == Constant.CUBE_PROMOTION_TYPE_1)
                        {
                            chartItem.Data_1 = (decimal)db.PerformanceFacts.Where(s => s.DimensionTime.Year == item.DimensionTime.Year
                           && s.DimensionPromotion.PromotionType == Constant.CUBE_PROMOTION_TYPE_1 && s.DimensionTime.Month == item.DimensionTime.Quarter).Sum(a => (a.CostAmount));
                        }
                        else if (subitem.DimensionPromotion.PromotionType == Constant.CUBE_PROMOTION_TYPE_2)
                        {
                            chartItem.Data_Group1_1 = (decimal)db.PerformanceFacts.Where(s => s.DimensionTime.Year == item.DimensionTime.Year
                          && s.DimensionPromotion.PromotionType == Constant.CUBE_PROMOTION_TYPE_2 && s.DimensionTime.Month == item.DimensionTime.Quarter).Sum(a => (a.CostAmount));

                        }
                        else if (subitem.DimensionPromotion.PromotionType == Constant.CUBE_PROMOTION_TYPE_3)
                        {
                            chartItem.Data_Group2_1 = (decimal)db.PerformanceFacts.Where(s => s.DimensionTime.Year == item.DimensionTime.Year
                         && s.DimensionPromotion.PromotionType == Constant.CUBE_PROMOTION_TYPE_3 && s.DimensionTime.Month == item.DimensionTime.Quarter).Sum(a => (a.CostAmount));

                        }
                    }
                    catch { }
                    

                }
                barChart.Add(chartItem);
            }
            return PartialView(barChart);
        }

        public PartialViewResult GEReport18_Cost_Profit()
        {
            var barChart = new List<BarChartViewModel>();
            var catagories = db.PerformanceFacts.GroupBy(p => p.DimensionTime.Year).Select(p => p.FirstOrDefault());
            foreach (var item in catagories)
            {
                var chartItem = new BarChartViewModel();
                chartItem.Catalogs = item.DimensionTime.Year;
                chartItem.Data_1 = (decimal)db.PerformanceFacts.Where(s => s.DimensionTime.Year == item.DimensionTime.Year).Sum(a => (a.CostAmount));
                chartItem.Data_Group1_1 = (decimal)db.PerformanceFacts.Where(s => s.DimensionTime.Year == item.DimensionTime.Year).Sum(a => (a.SaleAmount - a.CostAmount));
                barChart.Add(chartItem);
            }
            return PartialView(barChart);
        }

        public PartialViewResult GEReport19_Cost_Promotion_Summary()
        {
            var barChart = new List<BarChartViewModel>();
            var catagories = db.PerformanceFacts.GroupBy(p => new { p.DimensionTime.Year }).Select(p => p.FirstOrDefault());
            foreach (var item in catagories)
            {
                var chartItem = new BarChartViewModel();
                chartItem.Catalogs = item.DimensionTime.Year;
                var subCatagories = db.PerformanceFacts.GroupBy(p => p.DimensionPromotion.PromotionType).Select(p => p.FirstOrDefault());

                foreach (var subitem in subCatagories)
                {
                    if (subitem.DimensionPromotion.PromotionType == Constant.CUBE_PROMOTION_TYPE_1)
                    {
                        chartItem.Data_1 = (decimal)db.PerformanceFacts.Where(s => s.DimensionTime.Year == item.DimensionTime.Year
                       && s.DimensionPromotion.PromotionType == Constant.CUBE_PROMOTION_TYPE_1).Sum(a => (a.CostAmount));
                    }
                    else if (subitem.DimensionPromotion.PromotionType == Constant.CUBE_PROMOTION_TYPE_2)
                    {
                        chartItem.Data_Group1_1 = (decimal)db.PerformanceFacts.Where(s => s.DimensionTime.Year == item.DimensionTime.Year
                      && s.DimensionPromotion.PromotionType == Constant.CUBE_PROMOTION_TYPE_2).Sum(a => (a.CostAmount));

                    }
                    else if (subitem.DimensionPromotion.PromotionType == Constant.CUBE_PROMOTION_TYPE_3)
                    {
                        chartItem.Data_Group2_1 = (decimal)db.PerformanceFacts.Where(s => s.DimensionTime.Year == item.DimensionTime.Year
                     && s.DimensionPromotion.PromotionType == Constant.CUBE_PROMOTION_TYPE_3).Sum(a => (a.CostAmount));

                    }

                }
                barChart.Add(chartItem);
            }
            return PartialView(barChart); 
        }

        public PartialViewResult GEReport20_Cost_Outsource()
        {
            var barChart = new List<BarChartViewModel>();
            var catagories = db.PerformanceFacts.GroupBy(p => p.DimensionTime.Year).Select(p => p.FirstOrDefault());
            foreach (var item in catagories)
            {
                var chartItem = new BarChartViewModel();
                chartItem.Catalogs = item.DimensionTime.Year;
                chartItem.Data_1 = (decimal)db.PerformanceFacts.Where(s => s.DimensionTime.Year == item.DimensionTime.Year).Sum(a => (a.CostAmount - a.CostOutSourceAmount));
                chartItem.Data_Group1_1 = (decimal)db.PerformanceFacts.Where(s => s.DimensionTime.Year == item.DimensionTime.Year).Sum(a => (a.CostOutSourceAmount));
                barChart.Add(chartItem);
            }
            return PartialView(barChart);
        }
    }
}