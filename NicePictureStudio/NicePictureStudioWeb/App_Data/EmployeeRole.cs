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
    
    public partial class EmployeeRole
    {
        public string UserId { get; set; }
        public string RoleId { get; set; }
    
        public virtual Employee Employee { get; set; }
        public virtual Role Role { get; set; }
    }
}
