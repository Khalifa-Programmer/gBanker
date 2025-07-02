using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace gBanker.Web.ViewModels
{
    public class SchedulerViewModel : BaseModel
    {
        public long SchedulerID { get; set; }

        [Required]
        [StringLength(100)]
        [Display(Name = "Scheduler Name")]
        public string SchedulerName { get; set; }

        [StringLength(200)]
        public string Description { get; set; }

        [Display(Name = "Start Time")]
        public DateTime? StartTime { get; set; }

        public DateTime? LastRun { get; set; }

        public int? RunEvery { get; set; }
        [Display(Name = "Hour")]
        public int hours { get; set; }
        [Display(Name = "Minute")]
        public int mins { get; set; }
        public string mode { get; set; }

        [StringLength(5)]
        public string Frequency { get; set; }
        public IEnumerable<SelectListItem> FrequencyList { get; set; }
        public IEnumerable<SelectListItem> HourList { get; set; }
        public IEnumerable<SelectListItem> MinsList { get; set; }
       
    }
}