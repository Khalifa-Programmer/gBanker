using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace gBanker.Web.ViewModels
{
    public class MamlaListViewModel
    { 

        public int OfficeID { get; set; }
        public int RowSl { get; set; }
        public string  ZoneCode { get; set; }
        public string OfficeCode { get; set; }
        public string OfficeName { get; set; }
        public string CenterCode { get; set; }
        public string CenterName { get; set; }
        public Int64 Branchwise_sl { get; set; }
        public string Memberwise_sl { get; set; }
        public string BranchCode { get; set; }
        public string MemberCode { get; set; }
        public string Name { get; set; }
        public string ErrorColumnName { get; set; }
        public string ErrorCode { get; set; }
        public string ErrorDescription { get; set; }
        public int CenterID { get; set; }

        public IEnumerable<SelectListItem> officeListItems { get; set; }
        public IEnumerable<SelectListItem> centerListItems { get; set; }
        public IEnumerable<SelectListItem> productListItems { get; set; }

    }
}