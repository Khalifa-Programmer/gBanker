using gBanker.Data.CodeFirstMigration;
using gBanker.Data.CodeFirstMigration.Db;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace gBanker.Web.ViewModels
{
    public class BasicDataAddViewModel
    {
        [Display(Name = "Indicator Code")]
        [Required(ErrorMessage = "{0} is Required")]
        public string IndicatorCode { get; set; }

        [Display(Name = "M/F Flag")]
        [Required(ErrorMessage = "{0} is Required")]
        public string M_F_flag { get; set; }


        [Display(Name = "BD_PKSF_FUND")]
        [Required(ErrorMessage = "{0} is Required")]
        public decimal bd_pksf_fund { get; set; }

        [Display(Name = "BD_NON_PKSF_FUND")]
        [Required(ErrorMessage = "{0} is Required")]
        public decimal bd_non_pksf_fund { get; set; }

        [Display(Name = "Date")]
        [Required(ErrorMessage = "{0} is Required")]
        public string MNYR { get; set; }

        //additional
        public IEnumerable<SelectListItem> IndicatorList { get; set; }
    }
}