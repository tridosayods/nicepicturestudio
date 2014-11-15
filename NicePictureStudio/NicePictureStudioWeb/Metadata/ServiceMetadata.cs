using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace NicePictureStudio.App_Data
{
    [MetadataType(typeof(ServiceMetadata))]
    public partial class Service
    { }

    public class ServiceMetadata
    {
        [Required]
        public object GroomName { get; set; }
        [Required]
        public object BrideName { get; set; }

        //[Required]
        //public object Payment { get; set; }
        //[Required]
        //public object PayAmount { get; set; }
    }

    [MetadataType(typeof(ServiceMetadata))]
    public partial class ServiceFprm
    { }

    public class ServiceFormMetadata
    {
        [Required]
        public object GuestNumber { get; set; }
    }


    [MetadataType(typeof(LocationMetadata))]
    public partial class Location
    { }

    public class LocationMetadata
    {
        [Required]
        public object LocationName { get; set; }

        [Required]
        public object LocationAddress { get; set; }
    }

}