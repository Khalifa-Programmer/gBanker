using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace gBanker.Web.ViewModels
{
    public class DashboardViewModel
    {
        public int TotalOfficeCount { get; set; }
        public int TotalOrganizationMemberCount { get; set; }
        public long DashBoardID { get; set; }

        public int? OfficeID { get; set; }
        public string OfficeName { get; set; }

        public int? OrgID { get; set; }

        [Column(TypeName = "date")]
        public DateTime? DayEndProcessDate { get; set; }

        public long? TotalMember { get; set; }

        public long? TotalBorrower { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? TotalTodaysMember { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? TotalTodaysBorrower { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? Ratio { get; set; }

        public long? DormantMember { get; set; }

        public int? OverDueLoanee { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? OverDueAmount { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? DueAmount { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? PaidAmount { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? Otr { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? Income { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? Expense { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? ProfitOrLoss { get; set; }

        [StringLength(10)]
        public string ItemCode { get; set; }

        [StringLength(50)]
        public string ItemName { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? Members { get; set; }

        [StringLength(50)]
        public string BarYear { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? BarDisbursements { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? BarLoanRepaid { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? BarBadLoans { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? BarOverDueAmount { get; set; }
        [Column(TypeName = "numeric")]
        public decimal? BarSavings { get; set; }

        public decimal SummaryValue { get; set; }
    }    
}