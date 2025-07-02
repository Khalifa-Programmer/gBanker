using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gBanker.Data.CodeFirstMigration
{
    public partial class proc_get_LoanCorrection_Result
    {
        public long LoanCorrectionTrxID { get; set; }
        public System.DateTime TrxDate { get; set; }
        public long LoanSummaryID { get; set; }
        public int OfficeID { get; set; }
        public string OfficeCode { get; set; }
        public long MemberID { get; set; }
        public string MemberCode { get; set; }
        public string MemberName { get; set; }
        public short ProductID { get; set; }
        public string ProductCode { get; set; }
        public string ProductName { get; set; }
        public string InterestCalculationMethod { get; set; }
        public int CenterID { get; set; }
        public string CenterCode { get; set; }
        public byte MemberCategoryID { get; set; }
        public int LoanTerm { get; set; }
        public short PurposeID { get; set; }
        public System.DateTime InstallmentDate { get; set; }
        public decimal PrincipalLoan { get; set; }
        public decimal LoanRepaid { get; set; }
        public decimal LoanDue { get; set; }
        public decimal LoanPaid { get; set; }
        public decimal CumIntCharge { get; set; }
        public decimal IntCharge { get; set; }
        public decimal IntDue { get; set; }
        public decimal IntPaid { get; set; }
        public decimal Advance { get; set; }
        public decimal DueRecovery { get; set; }
        public byte TrxType { get; set; }
        public short InstallmentNo { get; set; }
        public short EmployeeID { get; set; }
        public Nullable<decimal> TotalPaid { get; set; }
        public Nullable<bool> IsActive { get; set; }
        public Nullable<System.DateTime> InActiveDate { get; set; }
        public string CreateUser { get; set; }
        public System.DateTime CreateDate { get; set; }
    }
}
