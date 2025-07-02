using gBanker.Web.Filters;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace gBanker.Web.ViewModels
{
    public class RepaymentScheduleViewModel
    {
        public long RepaymentScheduleID { get; set; }

        public long LoanSummaryID { get; set; }

        public int OfficeID { get; set; }

        public long MemberID { get; set; }

        public short ProductID { get; set; }
        [Display(Name = "Samity ID")]
        public int CenterID { get; set; }
        public string CenterCode { get; set; }
        public byte MemberCategoryID { get; set; }

        public int LoanTerm { get; set; }

       
        //public DateTime RepaymentDate { get; set; }
        public string RepaymentDate { get; set; }
        public int InstallmentNo { get; set; }

       
        public decimal LoanInstallment { get; set; }

  
        public decimal IntInstallment { get; set; }

       
        public decimal? IntCharge { get; set; }

      
        public decimal? PrincipalLoan { get; set; }

     
        public decimal? LoanBalnce { get; set; }

     
        public decimal? InterestBalance { get; set; }

        public bool? IsActive { get; set; }

    
        public DateTime? InActiveDate { get; set; }

       
        public string CreateUser { get; set; }

      
        public DateTime CreateDate { get; set; }

        public IEnumerable<SelectListItem> memberListItems { get; set; }
        public IEnumerable<SelectListItem> officeListItems { get; set; }
        public IEnumerable<SelectListItem> productListItems { get; set; }


    }
}