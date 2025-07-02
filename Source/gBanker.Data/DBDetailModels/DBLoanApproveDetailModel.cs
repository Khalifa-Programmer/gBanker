using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gBanker.Data.DBDetailModels
{
   public class DBLoanApproveDetailModel
    {
        public long LoanSummaryID { get; set; }
        public int OfficeID { get; set; }
        public string OfficeCode { get; set; }
        public string    OfficeName { get; set; }
        public string LoanNo { get; set; }
        public long MemberID { get; set; }
        public string MemberCode { get; set; }
        public string MemberName { get; set; }
        public int GroupID { get; set; }
        public short ProductID { get; set; }
        public string ProductCode { get; set; }
        public string ProductName { get; set; }
        public int CenterID { get; set; }
        public string CenterCode { get; set; }
        public string CenterName { get; set; }
        public byte MemberCategoryID { get; set; }
        public byte LoanTerm { get; set; }
        public short PurposeID { get; set; }
        public string PurposeCode { get; set; }
        public decimal PrincipalLoan { get; set; }
        public System.DateTime ApproveDate { get; set; }
        public Nullable<System.DateTime> DisburseDate { get; set; }
        public int Duration { get; set; }
        public decimal LoanInstallment { get; set; }
        public decimal IntInstallment { get; set; }
        public decimal InterestRate { get; set; }
        public Nullable<System.DateTime> InstallmentStartDate { get; set; }
        public System.DateTime? InstallmentDate { get; set; }
        public byte TransType { get; set; }
        public byte LoanStatus { get; set; }
       
        public short EmployeeId { get; set; }
        public string EmployeeCode { get; set; }
        public string EmployeeName { get; set; }

       
      
        public decimal LoanRepaid { get; set; }
        public decimal IntCharge { get; set; }
        public decimal IntPaid { get; set; }
       
     
        public int InstallmentNo { get; set; }
        public int DropInstallment { get; set; }
        public int Holidays { get; set; }
  
       
        public short ContinuousDrop { get; set; }
     
        public decimal Balance { get; set; }
        public decimal Advance { get; set; }
        public decimal DueRecovery { get; set; }


        public string SecurityBankName { get; set; }
       
        public string SecurityBankBranchName { get; set; }
       
        public string SecurityBankCheckNo { get; set; }
        public Nullable<System.DateTime> LoanCloseDate { get; set; }
        public Nullable<System.DateTime> OverdueDate { get; set; }
    
        public decimal ExcessPay { get; set; }
        public Nullable<decimal> CurLoan { get; set; }
        public Nullable<decimal> PreLoan { get; set; }
        public Nullable<decimal> CumLoanDue { get; set; }
        public Nullable<decimal> WriteOffLoan { get; set; }
        public Nullable<decimal> WriteOffInterest { get; set; }
       

    }
}
