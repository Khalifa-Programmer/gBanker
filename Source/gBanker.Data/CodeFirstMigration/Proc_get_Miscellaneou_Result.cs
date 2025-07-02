using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gBanker.Data.CodeFirstMigration
{
    public partial class Proc_get_Miscellaneou_Result
    {
        public Nullable<long> MiscellaneousID { get; set; }
        public Nullable<int> OfficeID { get; set; }
        public string OfficeCode { get; set; }
        public Nullable<int> CenterID { get; set; }
        public string CenterCode { get; set; }
        public Nullable<short> ProductID { get; set; }
        public string ProductCode { get; set; }
        public string MemberName { get; set; }
        public Nullable<decimal> Amount { get; set; }
        public Nullable<System.DateTime> TrxDate { get; set; }
    }
}
