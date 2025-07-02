using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace gBanker.Web.RestApi.Models.Entity
{
    public class Purpose
    {
        public int PurposeID { get; set; }
        public string PurposeCode { get; set; }
        public string PurposeName { get; set; }
        public int OrgID { get; set; }
        public bool IsActive { get; set; }
        public DateTime IsActiveDate { get; set; }
        public string CreateUser { get; set; }
        public DateTime CreateDate { get; set; }
        public string MainSector { get; set; }
        public string MainSectorName { get; set; }
        public string SubSector { get; set; }
        public string SubSectorName { get; set; }
        public string MainLoanSector { get; set; }
    }
}