using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gBanker.Data.DBDetailModels
{
    public class DBAccVoucherEntry
    {
        public long TrxMasterID { get; set; }
        public int OfficeID { get; set; }
        public DateTime TrxDate { get; set; }
        public string VoucherNo { get; set; }
        public string VoucherDesc { get; set; }
        public string VoucherType { get; set; }
        public string Reference { get; set; }
        public bool? IsPosted { get; set; }
        public bool? IsActive { get; set; }

        public long TrxDetailsID { get; set; }
        public int? AccID { get; set; }
        public decimal? Credit { get; set; }
        public decimal? Debit { get; set; }
        public string Narration { get; set; }
    }
}
