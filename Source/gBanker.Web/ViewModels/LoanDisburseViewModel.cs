using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace gBanker.Web.ViewModels
{
    public class LoanDisburseViewModel : BaseModel
    {

        public long LoanSummaryID { get; set; }
        public int OfficeID { get; set; }
        public string OfficeCode { get; set; }
        public string OfficeName { get; set; }
        public long MemberID { get; set; }
        public string MemberCode { get; set; }
        public string FirstName { get; set; }
        public string PhoneNo { get; set; }
        public short ProductID { get; set; }
        public string ProductCode { get; set; }
        public string ProductName { get; set; }
        [Display(Name = "Samity ID")]
        public int CenterID { get; set; }
        [Display(Name = "Samity Code")]
        public string CenterCode { get; set; }
        [Display(Name = "Samity Name")]
        public string CenterName { get; set; }
        public byte LoanTerm { get; set; }
        public short PurposeID { get; set; }
        public string PurposeName { get; set; }
        public decimal PrincipalLoan { get; set; }
        public System.DateTime ApproveDate { get; set; }
        public Nullable<System.DateTime> DisburseDate { get; set; }
        public string DisburseDtMsg { get; set; }
        public int Duration { get; set; }
        public decimal LoanRepaid { get; set; }
        public decimal IntCharge { get; set; }
        public decimal IntPaid { get; set; }
        public decimal CSFAmount { get; set; }
        public decimal LoanInstallment { get; set; }
        public decimal IntInstallment { get; set; }
        public decimal InterestRate { get; set; }
        public string InstallmentStartDate { get; set; }
        public string InstallmentStartDtMsg { get; set; }
        public short EmployeeId { get; set; }
        public byte MemberCategoryID { get; set; }
        public Nullable<System.DateTime> InstallmentDate { get; set; }
        public Nullable<System.DateTime> FirstInstallmentStartDate { get; set; }
        public int InstallmentNo { get; set; }

        public int DropInstallment { get; set; }
        public int Holidays { get; set; }

        public string TransType { get; set; }
        public short ContinuousDrop { get; set; }
        public decimal Balance { get; set; }
        public decimal Advance { get; set; }
        public decimal DueRecovery { get; set; }
        public decimal DisburseAmountWithSC { get; set; } // NEW ADD KHALID
        public decimal InstallmentTotal { get; set; } // NEW ADD KHALID

        public string BankName { get; set; }
        public string ChequeNo { get; set; }
        public DateTime? ChequeIssueDate { get; set; }
        public decimal ExcessPay { get; set; }
        public byte InvestorID { get; set; }
        public string pnm_order_identifier { get; set; }
        public decimal PartialAmount { get; set; }
        public string SmsStatus { get; set; }
        public IEnumerable<SelectListItem> GetAccountCodeList { get; set; }
        public string LoanStatus
        {
            get;
            set;

        }
        public string DisburseStatus
        {
            get
            {
                if (string.IsNullOrEmpty(LoanStatus))
                    return "";
                else if (LoanStatus == "2")
                    return "Disbursed";
                else if (LoanStatus == "1")
                    return "";
                else

                    return "";

            }
        }

    }
}