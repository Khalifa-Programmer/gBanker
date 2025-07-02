using System.Collections.Generic;
using System.Web.Mvc;

namespace gBanker.Core.Filters
{
    public class PRA_LN_DIST_WISE_DISB_ReportSearchFilter : BaseSearchFilter
    {
        public string MFI_District_Code { get; set; }
        public string MFI_Thana_Code { get; set; }

        public IEnumerable<SelectListItem> MFI_DistrictList { get; set; }
        public IEnumerable<SelectListItem> MFI_ThanaList { get; set; }
    }
}
