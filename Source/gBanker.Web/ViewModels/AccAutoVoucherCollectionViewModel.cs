using gBanker.Web.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace gBanker.Web.ViewModels
{
    public class AccAutoVoucherCollectionViewModel
    {
        [GlobalizedDisplayName("Office")]
        public int? OfficeId { get; set; }
        public DateTime? BusinessDate { get; set; }
        public IEnumerable<SelectListItem> officeListItems { get; set; }
    }
}