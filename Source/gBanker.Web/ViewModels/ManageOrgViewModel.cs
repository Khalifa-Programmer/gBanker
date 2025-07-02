using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace gBanker.Web.ViewModels
{
    public class ManageOrgViewModel : BaseModel
    {

        [Required(ErrorMessage = "{0} is required")]
        [Display(Name = "Organaization Code")]
        public string OrganaizationCode { get; set; }

        [Required(ErrorMessage = "{0} is required")]
        [Display(Name = "Organaization Name")]
        public string OrganaizationName { get; set; }

        [Required(ErrorMessage = "{0} is required")]
        [Display(Name = "Organaization Address")]
        public string OrganaizationAddress { get; set; }

        [Required(ErrorMessage = "{0} is required")]
        [Display(Name = "PKSF PO CODE")]
        public string PKSFPOCode { get; set; }

        public IEnumerable<SelectListItem> POCodeList { get; set; }
    }
}