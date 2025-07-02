using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gBanker.Data.CodeFirstMigration
{
    public partial class Proc_Get_AccountDetails_Result
    {

        public long TrxMasterID { get; set; }
        public string VoucherNo { get; set; }
        public System.DateTime TrxDate { get; set; }
        public string VoucherType { get; set; }
        public string VoucherDesc { get; set; }
        public string Reference { get; set; }
        public Nullable<decimal> TotDebit { get; set; }
        public Nullable<decimal> TotCredit { get; set; }
        public Nullable<bool> IsAutoVoucher { get; set; }
        public int OfficeID { get; set; }
        public int OfficeLevel { get; set; }
        public bool? IsPosted { get; set; }
        public bool? IsYearlyClosing { get; set; }
        public bool? IsRectify { get; set; }
    }
}
