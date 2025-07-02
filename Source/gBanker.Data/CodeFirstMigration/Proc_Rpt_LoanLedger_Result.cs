using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gBanker.Data.CodeFirstMigration
{
    public class Proc_Rpt_LoanLedger_Result
    {
        public int OfficeID { get; set; }
        public int CenterID { get; set; }
        public short ProductID { get; set; }
        public long MemberID { get; set; }
        public int LoanTerm { get; set; }
        public System.DateTime InstallmentDate { get; set; }
        public decimal PrincipalLoan { get; set; }
        public Nullable<decimal> LoanInstallment { get; set; }
        public decimal LoanDue { get; set; }
        public decimal IntCharge { get; set; }
        public Nullable<decimal> InterestPaid { get; set; }
        public decimal IntDue { get; set; }
        public byte TrxType { get; set; }
        public string CenterName { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string ProductName { get; set; }
        public Nullable<System.DateTime> DisburseDate { get; set; }
    }
}
