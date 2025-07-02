using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace gBanker.Web.RestApi.Models.Entity
{
    public class Investor
    {
        public byte InvestorID { get; set; }
        public string InvestorCode { get; set; }
        public string InvestorName { get; set; }
        public int OrgID { get; set; }
        public bool IsActive { get; set; }
        public DateTime IsActiveDate { get; set; }
        public  string CreateUser { get; set; }
        public DateTime CreateDate { get; set; }

    }
}