using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gBanker.Data.CodeFirstMigration
{
    public class getMemberPassBookStock_Result
    {
        public long MemberPassBookStockID { get; set; }
        public Nullable<int> OfficeID { get; set; }
        public Nullable<long> LotNo { get; set; }
        public Nullable<decimal> Qty { get; set; }
        public Nullable<long> StartingNo { get; set; }
        public Nullable<long> LastIssue { get; set; }
        public int OrgID { get; set; }
        public string OfficeCode { get; set; }
        public string OfficeName { get; set; }
    }
}
