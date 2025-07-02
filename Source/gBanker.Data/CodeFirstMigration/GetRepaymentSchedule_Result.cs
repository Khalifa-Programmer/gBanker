using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gBanker.Data.CodeFirstMigration
{
    public partial class GetRepaymentSchedule_Result
    {
        public long RepaymentScheduleID { get; set; }
        public long LoanSummaryID { get; set; }
        public int OfficeID { get; set; }
        public long MemberID { get; set; }
        public short ProductID { get; set; }
        public int CenterID { get; set; }
        public string CenterCode { get; set; }
        public string CenterName { get; set; }
        public string FullName { get; set; }
        public byte MemberCategoryID { get; set; }
        public int LoanTerm { get; set; }
        public string RepaymentDate { get; set; }
        //public System.DateTime RepaymentDate { get; set; }
        public int InstallmentNo { get; set; }
        public string ProductCode { get; set; }
        public string ProductName { get; set; }
        public decimal LoanInstallment { get; set; }
        public decimal IntInstallment { get; set; }
        //public Nullable<System.DateTime> InstallmentDate { get; set; }
        public string InstallmentDate { get; set; }
        public decimal LoanPaid { get; set; }
        public decimal IntPaid { get; set; }
        public decimal IntCharge { get; set; }
        public string CreateUser { get; set; }
        public decimal? PrincipalLoan { get; set; }


        public decimal? LoanBalnce { get; set; }


        public decimal? InterestBalance { get; set; }
    }
}
