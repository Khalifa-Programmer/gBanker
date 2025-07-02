using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace gBanker.Web.ViewModels
{
    public class LoanLedgerViewModel :BaseModel
    {
        public Nullable<int> OfficeID { get; set; }
        [Display(Name = "Samity ID")]
        public int CenterID { get; set; }
        public IEnumerable<SelectListItem> centerListItems { get; set; }
        public IEnumerable<SelectListItem> officeListItems { get; set; }
    }
}