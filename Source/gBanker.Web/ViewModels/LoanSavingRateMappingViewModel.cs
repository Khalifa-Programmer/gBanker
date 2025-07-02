using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace gBanker.Web.ViewModels
{
    public class LoanSavingRateMappingViewModel : BaseModel
    {
        [Required(ErrorMessage = "{0} is required")]
        [Display(Name = "Loan/Saving Rate")]
        public string LoanSavingRateType { get; set; }

        [Required(ErrorMessage = "{0} is required")]
        [Display(Name = "Product Name")]
        public string ProductName { get; set; }

        [Required(ErrorMessage = "{0} is required")]
        [Display(Name = "Rate")]
        public decimal Rate { get; set; }
    }
}