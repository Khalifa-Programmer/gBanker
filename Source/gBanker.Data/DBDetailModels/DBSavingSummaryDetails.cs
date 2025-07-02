using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gBanker.Data.DBDetailModels
{
    public class DBSavingSummaryDetails
    {
        public long SavingSummaryID { get; set; }
        public int OfficeID { get; set; }
        public string OfficeCode { get; set; }
        public long MemberID { get; set; }
        public string MemberCode { get; set; }
        public string MemberName { get; set; }
        public short ProductID { get; set; }
        public string ProductCode { get; set; }
        public int CenterID { get; set; }
        public string CenterCode { get; set; }
        public int NoOfAccount { get; set; }
        public System.DateTime TransactionDate { get; set; }
        public decimal Deposit { get; set; }
        public decimal Withdrawal { get; set; }
        public decimal Balance { get; set; }
        public decimal InterestRate { get; set; }
        public decimal SavingInstallment { get; set; }
        public decimal CumInterest { get; set; }
        public decimal MonthlyInterest { get; set; }
        public decimal Penalty { get; set; }
        public System.DateTime OpeningDate { get; set; }
        public Nullable<System.DateTime> MaturedDate { get; set; }
        public Nullable<System.DateTime> ClosingDate { get; set; }
        public byte TransType { get; set; }
        public byte SavingStatus { get; set; }
        public short EmployeeId { get; set; }
        public string EmployeeCode { get; set; }
        public byte MemberCategoryID { get; set; }
        public int Ref_Employee { get; set; }
        public bool IsActive { get; set; }
        public Nullable<System.DateTime> InActiveDate { get; set; }
    }
}
