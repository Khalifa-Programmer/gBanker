using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gBanker.Data.CodeFirstMigration
{
    public partial class GetCreditScore_Result
    {
        public string MemberCode { get; set; }
        public string OfficeCode { get; set; }
        public string ProductCode { get; set; }
        public string CenterCode { get; set; }
        public byte LoanTerm { get; set; }
        public string MemberID { get; set; }
        public string OfficeName { get; set; }
        public string EmpName { get; set; }
        public Nullable<System.DateTime> JoingDate { get; set; }
        public string MemberName { get; set; }
        public Nullable<System.DateTime> DOB { get; set; }
        public string RefereeName { get; set; }
        public string MemberAddress { get; set; }
        public string SSN { get; set; }
        public decimal PrincipalLoan { get; set; }
        public Nullable<System.DateTime> DisburseDate { get; set; }
        public decimal LoanPaid { get; set; }
        public decimal LoanBalance { get; set; }
        public decimal InterestPaid { get; set; }
        public decimal LoanInstallment { get; set; }
        public decimal IntInstallment { get; set; }
        public string LoanItem { get; set; }
        public string PurposeName { get; set; }
        public Nullable<System.DateTime> LastInstallmentDate { get; set; }
        public Nullable<decimal> LoanPaid_ThisMonth { get; set; }
        public Nullable<decimal> IntPaid_ThisMonth { get; set; }
        public short LoanDuration { get; set; }
        public short WeekPassed { get; set; }
        public short DropInstallment { get; set; }
        public Nullable<System.DateTime> AccountCloseDate { get; set; }
        public Nullable<System.DateTime> LastPaymentDate { get; set; }
        public Nullable<decimal> LastLoanPaid { get; set; }
        public Nullable<decimal> LastIntPaid { get; set; }
        public Nullable<System.DateTime> ReportDate { get; set; }
        public string MemberFirstName { get; set; }
        public string MemberMiddleName { get; set; }
        public string MemberLastName { get; set; }
        public string StreetAddress1 { get; set; }
        public string StreetAddress2 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string ZipCode { get; set; }
        public string TypeofID { get; set; }
        public string IDComments { get; set; }
        public string Race { get; set; }
        public string Ethnicity { get; set; }
    }
}
