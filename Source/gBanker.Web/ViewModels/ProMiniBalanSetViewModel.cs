using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace gBanker.Web.ViewModels
{
    public class ProMiniBalanSetViewModel : BaseModel
    {
        public long MinimumBalanceID { get; set; }

        [Display(Name = "Product")]        
        public int? ProductId { get; set; }

        [Display(Name = "Minimum Balance")]
        [Required(ErrorMessage = "Minimum Balance is required.")]
        public decimal MinimumBalance { get; set; }

        [Display(Name = "Effective Date From")]
        [Required(ErrorMessage = "Effective Date From is required.")]
        public string EffectiveDateFrom { get; set; }

        [Display(Name = "Effective Date To")]
        [Required(ErrorMessage = "Effective Date To is required.")]
        public string EffectiveDateTo { get; set; }

        public bool IsDefault { get; set; }

        public IEnumerable<SelectListItem> productListItems { get; set; }

        public long? rowSl { get; set; }

        public string CreatedOn { get; set; }
    }
}