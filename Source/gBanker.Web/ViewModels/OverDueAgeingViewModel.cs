using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace gBanker.Web.ViewModels
{
    public class OverDueAgeingViewModel
    {
        public int? OrgID { get; set; }
        public int? OfficeID { get; set; }
        public int? OfficeIDTO { get; set; }
        public int? CenterID { get; set; }
        public int? CenterIDTo { get; set; }
        public int? StaffID { get; set; }
        public int? StaffIDTo { get; set; }
        public int? productID { get; set; }
        public int? ProductIDTo { get; set; }
        public DateTime? DateFrom { get; set; }
        public String DateTo { get; set; }
        public String Qtype { get; set; }
    }
}