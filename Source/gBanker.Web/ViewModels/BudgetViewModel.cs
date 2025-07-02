//using System;
//using System.Collections.Generic;
//using System.ComponentModel.DataAnnotations;
//using System.Linq;
//using System.Web;
//using System.Web.Mvc;

//namespace gBanker.Web.ViewModels
//{
//    public class BudgetViewModel: BaseModel
//    {
//        public int BudgetID { get; set; }

//        public int OrgID { get; set; }

//        public int OfficeID { get; set; }

//        [Display(Name = "Date")]
//        [DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy}", ApplyFormatInEditMode = true)]
//        public DateTime? TrxDate { get; set; }

//        [Display(Name = "Budget Year")]
//        public int? BudgetYear { get; set; }

//        [Display(Name = "Account Code")]
//        public int AccID { get; set; }
//        public string AccName { get; set; }

//        [Display(Name = "Amount")]
//        public decimal BudgetAmount { get; set; }

//        [Display(Name = "Budget Type")]
//        public int? BudgetType { get; set; }
//        public int? OfficeLevel { get; set; }
//        public int SlNo { get; set; }
//        public IEnumerable<SelectListItem> AccChartList { get; set; }
//    }
//}
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace gBanker.Web.ViewModels
{
    public class BudgetViewModel : BaseModel
    {
        public int BudgetID { get; set; }

        public int OrgID { get; set; }

        public int OfficeID { get; set; }

        [Display(Name = "Date")]
        [DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? TrxDate { get; set; }

        [Display(Name = "Budget Year")]
        public int? BudgetYear { get; set; }

        [Display(Name = "Account Code")]
        public int AccID { get; set; }
        public string AccName { get; set; }

        [Display(Name = "Amount")]
        public decimal BudgetAmount { get; set; }

        [Display(Name = "Budget Type")]
        public int? BudgetType { get; set; }
        public int? OfficeLevel { get; set; }
        public int SlNo { get; set; }
        public bool IsFinancial { get; set; }
        public IEnumerable<SelectListItem> AccChartList { get; set; }
    }
}