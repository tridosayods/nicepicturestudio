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
    
    public partial class DimensionServiceType
    {
        public DimensionServiceType()
        {
            this.PerformanceFacts = new HashSet<PerformanceFacts>();
        }
    
        public int ServiceTypeId { get; set; }
        public string ServiceType { get; set; }
        public Nullable<decimal> Cost { get; set; }
    
        public virtual ICollection<PerformanceFacts> PerformanceFacts { get; set; }
    }
}
