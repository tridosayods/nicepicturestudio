using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NicePictureStudio.Models
{
    public class MISReportingModel
    {
        //Dimension Time
        public string Year { get; set; }
        public string Quarter { get; set; }
        public string Month { get; set; }

        //Dimension ServiceType
        public string ServiceType { get; set; }
        public decimal? ServiceTYpeCost { get; set; }

        //Dimension Photographer
        public string PhotoGrapherName { get; set; }
        public string Skill { get; set; }

        //Dimension Promotion
        public string SalePerson { get; set; }
        public int? PromotionType { get; set; }
        public decimal? Profit { get; set; }

        //Dimension Satisfaction
        public int? BestScore { get; set; }
        public int? WorseScore { get; set; }

        //Measure
        public int? ApprisalScore { get; set; }
        public decimal? ProfitPercent { get; set; }
        public int? SaleAmount { get; set; }
        public int? CostAmount { get; set; }
    }

    public class BarChartViewModel
    {
        //Data Group
        public decimal Data_1 { get; set; }
        public decimal Data_2 { get; set; }

        public decimal Data_Group1_1 { get; set; }
        public decimal Data_Group1_2 { get; set; }

        public decimal Data_Group2_1 { get; set; }
        public decimal Data_Group2_2 { get; set; }

        //Catalogs
        public string Catalogs { get; set; }
    }
}