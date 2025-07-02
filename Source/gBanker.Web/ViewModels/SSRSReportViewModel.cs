using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace gBanker.Web.ViewModels
{
    public class SSRSReportViewModel
    {
        public long rowSl { get; set; }
        public int OfficeLevel { get; set; }
        public string HOCode { get; set; }
        public string HOName { get; set; }
        public int OfficeID { get; set; }
        public string OfficeCode { get; set; }
        public string OfficeName { get; set; }
        public int EmployeeID { get; set; }
        public string OfficeLevelName { get; set; }

    }
}