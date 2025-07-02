using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace gBanker.Web.RestApi.Models.Entity
{
    public class LoanTermValidationResult
    {
        public int ErrorCode { get; set; }
        public string ErrorName { get; set; }
        public int LoantermUP { get; set; }
        public int OfficeID { get; set; }
        public long MemberID { get; set; }
    }
}