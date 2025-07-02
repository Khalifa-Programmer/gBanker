using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace gBanker.Web.ViewModels
{
    public class HalfYearlySavingInterestViewModel :BaseModel
    {
        public int? OfficeId { get; set; }

        public DateTime? BusinessDate { get; set; }
        public IEnumerable<SelectListItem> officeListItems { get; set; }
    }
}