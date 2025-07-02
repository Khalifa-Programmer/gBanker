using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gBanker.Data.CodeFirstMigration
{
    public partial class GetSetSLCTxtKeyPress_Result
    {
        public Nullable<long> loansummaryId { get; set; }
        public int OfficeID { get; set; }
        public int centerid { get; set; }
        public short productid { get; set; }
        public string ProductCode { get; set; }
        public string ProductName { get; set; }
        public long memberid { get; set; }
        public string membercode { get; set; }
        public string MemberName { get; set; }
        public byte LoanTerm { get; set; }
        public decimal PrincipalLoan { get; set; }
        public decimal LoanRepaid { get; set; }
        public Nullable<decimal> IntCharge { get; set; }
        public Nullable<decimal> CumIntCharge { get; set; }
        public Nullable<decimal> IntPaid { get; set; }
        public Nullable<decimal> LoanInstallment { get; set; }
        public Nullable<System.DateTime> InstallmentDate { get; set; }
        public Nullable<System.DateTime> trxdate { get; set; }
        public int DueLoanInstallment { get; set; }
        public int DueInterestInstallment { get; set; }
        public int TransType { get; set; }
        public byte MemberCategoryID { get; set; }
        public short EmployeeID { get; set; }
        public Nullable<byte> InvestorID { get; set; }
        public int OrgID { get; set; }
        public Nullable<short> PurposeID { get; set; }
        public int InstallmentNo { get; set; }
        public string InterestCalculationMethod { get; set; }
        public Nullable<decimal> TotalPaid { get; set; }
    }
}
