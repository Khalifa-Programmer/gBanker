using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace gBanker.Web.ViewModels
{
    public class VoiceDataProcessViewModel
    {
        public int OfficeId { get; set; }
        public int TrxType { get; set; }
        public string DateValue { get; set; }
        public string[] OfficeIds { get; set; }

        public IEnumerable<SelectListItem> OfficeList { get; set; }
        public IEnumerable<SelectListItem> TrxTypeList { get; set; }
    }
}