//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace NicePictureStudio.App_Data
{
    using System;
    using System.Collections.Generic;
    
    public partial class DimensionPromotion
    {
        public DimensionPromotion()
        {
            this.PerformanceFacts = new HashSet<PerformanceFacts>();
        }
    
        public int PromotionId { get; set; }
        public string SalePerson { get; set; }
        public Nullable<int> PromotionType { get; set; }
        public Nullable<int> Profit { get; set; }
        public string PromotionTypeName { get; set; }
    
        public virtual ICollection<PerformanceFacts> PerformanceFacts { get; set; }
    }
}
