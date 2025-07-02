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
    public class FinancialDataAddViewModel
    {
        [Display(Name = "Indicator Code")]
        [Required(ErrorMessage = "{0} is Required")]
        public string IndicatorCode { get; set; }

        [Display(Name = "Amount")]
        [Required(ErrorMessage = "{0} is Required")]
        public decimal Amount { get; set; }  

        [Display(Name = "Date")]
        [Required(ErrorMessage = "{0} is Required")]
        public string MNYR { get; set; }

        //additional
        public IEnumerable<SelectListItem> IndicatorList { get; set; }
    }
}