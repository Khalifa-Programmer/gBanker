using gBanker.Data.CodeFirstMigration;
using gBanker.Data.CodeFirstMigration.Db;
using gBanker.Data.DBDetailModels.OLRSHubs.CommonModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace gBanker.Web.ViewModels
{
    public class ProgramDataAddViewModel
    {
        public ProgramDataAddViewModel()
        {
            this.ProgramDataLoanCodeWiseValueList = new List<ProgramDataLoanCodeWiseValue>();
        }

        [Display(Name = "Indicator Code")]
        [Required(ErrorMessage = "{0} is Required")]
        public string IndicatorCode { get; set; }

        [Display(Name = "MNYR")]
        [Required(ErrorMessage = "{0} is Required")]
        public string MNYR { get; set; }

        [Display(Name = "Male/Female Flag")]
        [Required(ErrorMessage = "{0} is Required")]
        public string M_F_Flag { get; set; }

        public List<ProgramDataLoanCodeWiseValue> ProgramDataLoanCodeWiseValueList { get; set; }

        //additional
        public IEnumerable<SelectListItem> IndicatorList { get; set; }
        public IEnumerable<POLoanCodeWithProductModel> LoanCodeWiseProductList { get; set; }
    }

    public class ProgramDataLoanCodeWiseValue
    {
        public string LoanCode { get; set; }
        public decimal Amount { get; set; }
    }
}