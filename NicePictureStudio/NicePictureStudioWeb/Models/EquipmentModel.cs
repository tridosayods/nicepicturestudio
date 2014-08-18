using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace NicePictureStudio.Models
{
    public class Equipment
    {
        public int EquipmentId { get; set; }
        public string EquipmentName { get; set; }
        public string Type { get; set; }
        public string ModelName { get; set; }
        public string EquipmentDetail { get; set; }
        public string status { get; set; }
    }

}