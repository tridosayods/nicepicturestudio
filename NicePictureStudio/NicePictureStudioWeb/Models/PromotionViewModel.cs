using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NicePictureStudio.Models
{
    public class PromotionCalcViewModel
    {
        public int PhotoDiscount { get; set; }
        public int EqpDiscount { get; set; }
        public int LocDiscount { get; set; }
        public int OsrDiscount { get; set; }
        public int OptDiscount { get; set; }
    }

    public class PromotionResult
    {
        public string DiscountSummary { get; set; }
        public string lowestDiscount { get; set; }
    }
}