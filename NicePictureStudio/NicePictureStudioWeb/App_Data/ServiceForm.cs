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
    
    public partial class ServiceForm
    {
        public ServiceForm()
        {
            this.EquipmentSchedules = new HashSet<EquipmentSchedule>();
            this.LocationSchedules = new HashSet<LocationSchedule>();
            this.OutputSchedules = new HashSet<OutputSchedule>();
            this.OutsourceSchedules = new HashSet<OutsourceSchedule>();
            this.EmployeeSchedules = new HashSet<EmployeeSchedule>();
        }
    
        public int Id { get; set; }
        public string Name { get; set; }
        public System.DateTime EventStart { get; set; }
        public System.DateTime EventEnd { get; set; }
        public Nullable<int> GuestsNumber { get; set; }
        public Nullable<decimal> ServiceCost { get; set; }
        public Nullable<decimal> ServicePrice { get; set; }
        public Nullable<decimal> ServiceNetPrice { get; set; }
    
        public virtual ServiceStatu ServiceStatu { get; set; }
        public virtual ServiceType ServiceType { get; set; }
        public virtual ICollection<EquipmentSchedule> EquipmentSchedules { get; set; }
        public virtual ICollection<LocationSchedule> LocationSchedules { get; set; }
        public virtual ICollection<OutputSchedule> OutputSchedules { get; set; }
        public virtual ICollection<OutsourceSchedule> OutsourceSchedules { get; set; }
        public virtual Service Service { get; set; }
        public virtual ICollection<EmployeeSchedule> EmployeeSchedules { get; set; }
    }
}
