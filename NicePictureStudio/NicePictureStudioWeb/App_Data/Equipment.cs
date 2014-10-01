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
    
    public partial class Equipment
    {
        public Equipment()
        {
            this.EquipmentServices = new HashSet<EquipmentService>();
        }
    
        public int EquipmentId { get; set; }
        public string EquipmentName { get; set; }
        public string ModelName { get; set; }
        public string EquipmentDetail { get; set; }
        public int Quantity { get; set; }
    
        public virtual EquipmentStatu EquipmentStatu { get; set; }
        public virtual EquipmentType EquipmentType { get; set; }
        public virtual ICollection<EquipmentService> EquipmentServices { get; set; }
    }
}
