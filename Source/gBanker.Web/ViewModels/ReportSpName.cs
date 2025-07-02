using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace gBanker.Web.ViewModels
{
    public class ReportSpName
    {
        public int Id { get; set; }
        public string LabelName { get; set; }
        public string SpName { get; set; }
        public bool? IsActive { get; set; }
        public string OfficeCode { get; set; }
        public string OfficeName { get; set; }

    }// END Class
}// END Namespace