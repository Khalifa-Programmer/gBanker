using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace gBanker.Web.RestApi.Models.Entity
{
    public class MiscEntryResult
    {
        public int OfficeID { get; set; }
        public int CenterID { get; set; }
        public short ProductID { get; set; }
        public DateTime TrxDate { get; set; }
        public long MemberID { get; set; }
        public long MiscellaneousId { get; set; }
    }
}