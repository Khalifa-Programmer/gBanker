using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace gBanker.Web.ViewModels
{
    public class LoaneeTransferViewModel : BaseModel
    {
        public long MemberID { get; set; }
        
        [Display(Name = "Member Code")]
        public string MemberCode { get; set; }
        
        [Display(Name = "Member Name")]
        public string MemberName { get; set; }        
        public int OfficeID { get; set; }
        
        [Display(Name = "Office Name")]
        public string OfficeName { get; set; }
        [Display(Name = "Samity ID")]
        public int CenterID { get; set; }

        [Display(Name = "Samity Name")]
        public int CenterName { get; set; }

        [Display(Name = "Group Name")]
        public string GroupName { get; set; }
        public int GroupID { get; set; }
        public string RefereeName { get; set; }
        public long LoanSummaryID { get; set; }
        public short ProductID { get; set; }
        public int NewOfficeID { get; set; }
        public int NewCenterID { get; set; }
        public int NewGroupID { get; set; }
        public string NewMemberCode { get; set; }
        public IEnumerable<SelectListItem> OfficeList { get; set; }
        public IEnumerable<SelectListItem> CenterList { get; set; }
        public IEnumerable<SelectListItem> GroupList { get; set; }
        public IEnumerable<SelectListItem> ProductList { get; set; }
        public IEnumerable<SelectListItem> MemberList { get; set; }

    }
}