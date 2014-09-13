using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace NicePictureStudio.App_Data
{
    [MetadataType(typeof(EmployeeLoginMetadata))]
    public partial class EmployeeLogin
    { }

    public class EmployeeLoginMetadata
    {
        [Key]
        [Column(Order=1)]
        public object LoginProvider { get; set; }
        [Key]
        [Column(Order = 2)]
        public object ProviderKey { get; set; }
        [Key]
        [Column(Order = 3)]
        public object UserId { get; set; }
    }

    [MetadataType(typeof(EmployeeRoleMetadata))]
    public partial class EmployeeRole
    { }

    public class EmployeeRoleMetadata
    {
        [Key]
        [Column(Order = 1)]
        public object UserId { get; set; }
        [Key]
        [Column(Order = 2)]
        public object RoleId { get; set; }
    }

    [MetadataType(typeof(IdentityUserRoleMetadata))]
    public partial class IdentityUserRole
    { }

    public class IdentityUserRoleMetadata
    {
        [Key]
        [Column(Order = 1)]
        public object UserId { get; set; }
        [Key]
        [Column(Order = 2)]
        public object RoleId { get; set; }
    }

    [MetadataType(typeof(EmployeeMetadata))]
    public partial class Employee
    { }

    public class EmployeeMetadata
    {

        //[ForeignKey("Status")]
        //public virtual object EmployeeStatu { get; set; }
     
    }
}