using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace gBanker.Web.ViewModels
{
    public class MatureLTSViewModel//:BaseModel
    {
        public long MatureLTSID { get; set; }
       
        public long SavingSummaryID { get; set; }
        
      
        public decimal? Calcnterest { get; set; }
        public decimal? Deposit { get; set; }

        public decimal? WithDrawal { get; set; }
        public decimal? Interest { get; set; }
        public bool Transffered { get; set; }
        public DateTime? TransDate { get; set; }

        public int ProductID { get; set; }
        public int OfficeID { get; set; }
        public decimal? CurrentInterest { get; set; }
        public decimal? InterestRate { get; set; }
        public decimal? WithdrawalRate { get; set; }
        public DateTime? OpeningDate { get; set; }
        public decimal? SavingInstallment { get; set; }
        public int Duration { get; set; }
        
        [Required]
        [StringLength(35)]
        public string CreateUser { get; set; }


        public DateTime? CreateDate { get; set; }
        public int rowSl { get; set; }

        public decimal? MembersTotalDeposit { get; set; }

        public int? InstallmentPaid { get; set; }

        public int? ValidForMSDS { get; set; }
    }// END Class

    public class SavingsLedgerViewModel//:BaseModel
    {
        public string ZoneCode { get; set; }
        public string ZoneName { get; set; }
        public string AreaCode { get; set; }
        public string AreaName { get; set; }
        public int OfficeID { get; set; }

        public string BranchCode { get; set; }
        public string BranchName { get; set; }

        public int CenterID { get; set; }

        public string CenterCode { get; set; }
        public string CenterName { get; set; }
        public int EmployeeID { get; set; }
       
        public string StaffID { get; set; }
        public string StaffName { get; set; }

        public long MemberID { get; set; }

        public string LoaneeNo { get; set; }

        public string LoaneeName { get; set; }

        public int ProductID { get; set; }

        public string ProductCode { get; set; }
        public string ItemName { get; set; }
        public DateTime? InstallmentDate { get; set; }

      
        public decimal? Savings { get; set; }
        public decimal? Withdrawal { get; set; }

        public decimal? Balance { get; set; }
        public decimal? MonthlyInterest { get; set; }

        public decimal? running_total { get; set; }

        public DateTime? ReportStartDate { get; set; }
        public DateTime? ReportEndDate { get; set; }
        public DateTime? AccountOpeningDate { get; set; }

        public int TranType { get; set; }
        public decimal? Penalty { get; set; }
        public string NoOfAccount { get; set; }
        public string TrxType { get; set; }
        public string AccountNo { get; set; }

       


    }// END Class
}// END NameSpace