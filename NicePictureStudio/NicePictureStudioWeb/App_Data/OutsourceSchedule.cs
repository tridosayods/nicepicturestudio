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
    
    public partial class OutsourceSchedule
    {
        public int Id { get; set; }
        public System.DateTime StartTime { get; set; }
        public System.DateTime EndTime { get; set; }
        public int OutsourceId { get; set; }
        public int OutsourceServiceId { get; set; }
    
        public virtual ServiceForm ServiceForm { get; set; }
    }
}
