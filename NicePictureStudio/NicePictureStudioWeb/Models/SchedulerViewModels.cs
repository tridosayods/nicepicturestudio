using Kendo.Mvc.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NicePictureStudio.Models
{
    public class SchedulerViewModels : ISchedulerEvent
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public string Description { get; set; }
        public int selectedStatus { get; set; }
        public IEnumerable<SelectList> Status { get; set; }
        [HiddenInput]
        public bool IsAllDay { get; set; }
        [HiddenInput]
        public string Recurrence { get; set; }
        [HiddenInput]
        public string RecurrenceRule { get; set; }
        [HiddenInput]
        public string RecurrenceException { get; set; }
        [HiddenInput]
        public string StartTimezone { get; set; }
        [HiddenInput]
        public string EndTimezone { get; set; }

    }

    public class ServiceStatusViewModel
    {
        public int Id {get;set;}
        public string Name {get;set;}
        public string Description  {get;set;}
    }
 
}