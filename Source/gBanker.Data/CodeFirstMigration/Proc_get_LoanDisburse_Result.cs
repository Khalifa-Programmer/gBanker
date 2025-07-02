using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gBanker.Data.CodeFirstMigration
{
    public partial class Proc_get_LoanDisburse_Result
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
        public int CenterID { get; set; }
        public string CenterCode { get; set; }
        public string CenterName { get; set; }
        public byte LoanTerm { get; set; }
        public short PurposeID { get; set; }
        public string PurposeName { get; set; }
        public decimal PrincipalLoan { get; set; }
        public System.DateTime ApproveDate { get; set; }
        public string TransType { get; set; }
        public Nullable<System.DateTime> DisburseDate { get; set; }
        public string DisburseDtMsg { get; set; }
        public int Duration { get; set; }
        public decimal LoanInstallment { get; set; }
        public decimal IntInstallment { get; set; }
        public decimal InterestRate { get; set; }
        public decimal IntCharge { get; set; }
        public string InstallmentStartDate { get; set; }
        public string InstallmentStartDtMsg { get; set; }
        public byte LoanStatus { get; set; }
        public short EmployeeId { get; set; }
        public byte MemberCategoryID { get; set; }
        public Nullable<System.DateTime> InstallmentDate { get; set; }
        public Nullable<System.DateTime> FirstInstallmentStartDate { get; set; }
        public string pnm_order_identifier { get; set; }
        public string SmsStatus { get; set; }
        public string BankName { get; set; }
        public string ChequeNo { get; set; }
        public DateTime? ChequeIssueDate { get; set; }
        public decimal PartialAmount { get; set; }

        public decimal CSFAmount { get; set; }
        public decimal DisburseAmountWithSC { get; set; }
        public decimal InstallmentTotal { get; set; }



    }
}
