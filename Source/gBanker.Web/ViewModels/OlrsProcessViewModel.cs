using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace gBanker.Web.ViewModels
{
    public class OlrsProcessViewModel : BaseModel
    {
        [Required(ErrorMessage = "{0} is required")]
        [Display(Name = "Process Type")]
        public string ProcessType { get; set; }

        [Required(ErrorMessage = "{0} is required")]
        [Display(Name = "Process Date  [mm/yyyy]")]
        public string ProcessMonth { get; set; }

        [Display(Name = "Sync Type")]
        public string SyncToPKSFType { get; set; }  
        
        [Required(ErrorMessage = "{0} is required")]
        [Display(Name = "Sync Date [mm/yyyy]")]
        public string SyncMonth { get; set; }

        [Display(Name = "Not Sync Type")]
        public string NotSyncToPKSFType { get; set; }

        [Required(ErrorMessage = "{0} is required")]
        [Display(Name = "Not Sync Date [mm/yyyy]")]
        public string NotSyncMonth { get; set; }
    }
}