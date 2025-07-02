using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace gBanker.Web.ViewModels
{
    public class BalanceCompareViewModel: BaseModel
    {
        //Common Field Both of Loan & Savings
        public int RowSl { get; set; }
        public int OfficeID { get; set; }
        public string OfficeCode { get; set; }
        public string OfficeName { get; set; }
        public decimal BalanceA { get; set; }
        public decimal Diff_R_S { get; set; }
        public decimal Diff_R_A { get; set; }
        public string AccCode { get; set; }
        public string AccName { get; set; }
        public DateTime CompleteWorkDate { get; set; }


        //Loan Section
        public decimal LoanBalR{ get; set; }
        public decimal LoanBalS{ get; set; }
        


        //Saving Section
        public decimal BalanceR { get; set; }
        public decimal BalanceS { get; set; }
        public decimal Diff_S_A { get; set; }


        //For DropDown
        public List<SelectListItem> ComareList { get; set; }
    }
}