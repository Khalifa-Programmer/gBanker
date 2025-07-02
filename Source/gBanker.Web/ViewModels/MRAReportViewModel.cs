using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace gBanker.Web.ViewModels
{
    public class MRAReportViewModel
    {
        public int MRAReportID { get; set; }
        public DateTime? DateTo { get; set; }
        public DateTime? DateFrom { get; set; }
        public int Office { get; set; }
        public List<SelectListItem> MRAReportList { get; set; }
        public List<SelectListItem> OfficeList { get; set; }
    }
}