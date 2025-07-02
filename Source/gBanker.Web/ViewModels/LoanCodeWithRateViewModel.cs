using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace gBanker.Web.ViewModels
{
    public class LoanCodeWithRateViewModel : BaseModel
    {
        [Display(Name = "Service Charge Rate")]
        [Required(ErrorMessage = "{0} is Required")]
        public string ServiceChargeRate { get; set; }

        [Display(Name = "Loan Date")]
        [Required(ErrorMessage = "{0} is Required")]
        public string LoanDate { get; set; }

        [Display(Name = "Loan Code")]
        [Required(ErrorMessage = "{0} is Required")]
        public string LoanCode { get; set; }

        [Display(Name = "Service Amount")]
        [Required(ErrorMessage = "{0} is Required")]
        public decimal ServiceAmount { get; set; }
        //public List<LoanCodeXRateViewModel> LoanCodeWithRates { get; set; }


        public IEnumerable<SelectListItem> LoanCodeList { get; set; }
    }

    public class LoanCodeXRateViewModel
    {
        public string LoanCodeName { get; set; }
        public float? ServiceCharge { get; set; }

    }
}