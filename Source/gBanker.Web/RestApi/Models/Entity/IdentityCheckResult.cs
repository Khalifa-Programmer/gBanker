using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace gBanker.Web.RestApi.Models.Entity
{
    public class IdentityCheckResult
    {
        public Int64 MemberID { get; set; }
        public string MemberCode { get; set; }
    }
}