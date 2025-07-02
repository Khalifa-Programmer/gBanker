using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gBanker.Data.CodeFirstMigration
{
    public partial class Proc_Get_CUMMIS_Result
    {
        public int CumMisID { get; set; }
        public int OfficeID { get; set; }
        public string OfficeCode { get; set; }
        public int CenterID { get; set; }
        public string CenterCode { get; set; }
        public int ProductID { get; set; }
        public string ProductCode { get; set; }
        public string Gender { get; set; }
        public Nullable<decimal> NoOfLoanee { get; set; }
        public Nullable<decimal> UpToLoanDis { get; set; }
        public Nullable<decimal> UptoDisburseMent { get; set; }
        public Nullable<decimal> UpToRecovery { get; set; }
        public Nullable<decimal> UptoAdmission { get; set; }
        public Nullable<decimal> UpToDropOut { get; set; }
        public Nullable<decimal> UptoFullyRepaid { get; set; }
        public Nullable<decimal> UptoDeposit { get; set; }
        public Nullable<decimal> UptoInterest { get; set; }
        public Nullable<decimal> uptowithdrawal { get; set; }
        public Nullable<decimal> WriteOffLoan { get; set; }
        public Nullable<decimal> WriteOffInterest { get; set; }

        public string CumMisItemName { get; set; }
    }
}
