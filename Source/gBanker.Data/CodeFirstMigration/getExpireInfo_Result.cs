using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gBanker.Data.CodeFirstMigration
{
    public partial class getExpireInfo_Result
    {
        public long ExpireInfoID { get; set; }
        public int OfficeID { get; set; }
        public string OfficeCode { get; set; }
        public int CenterID { get; set; }
        public string CenterCode { get; set; }
        public long MemberID { get; set; }
        public string MemberCode { get; set; }
        public string MemberName { get; set; }
        public string ExpiryName { get; set; }
        public string ExpireDate { get; set; }
        public string Remarks { get; set; }
        public string Relation { get; set; }
    }
}
