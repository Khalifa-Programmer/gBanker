using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace gBanker.Web.RestApi.Models.Entity
{
    public class Center
    {
        public int CenterID { get; set; }
        public string CenterCode { get; set; }
        public int OfficeID { get; set; }
        public string CenterName { get; set; }
    }
}