using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace gBanker.Web.ViewModels
{
    public class DayInitialViewModel :BaseModel
    {
        public int? OfficeId { get; set; }
        public DateTime? BusinessDate { get; set; }
        public IEnumerable<SelectListItem> officeListItems { get; set; }

        public Int64 ProcessInfoID { get; set; }
        public Int64 rowSl { get; set; }
        public string OfficeName { get; set; }
        public string InitialDate { get; set; }

    }
}