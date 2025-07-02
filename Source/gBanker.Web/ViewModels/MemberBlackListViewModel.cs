using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace gBanker.Web.ViewModels
{
    public class MemberBlackListViewModel: BaseModel
    {
        public Int64 MemberBlackListID { get; set; }

        public int? OfficeID { get; set; }

        [NotMapped]
        public string Center { get; set; }
        public Int64? MemberID { get; set; }

        [Display(Name = "Member Code")]
        [StringLength(50, ErrorMessage = "Maximum length is {1}")]
        public string MemberCode { get; set; }
        [NotMapped]
        public string MemberName { get; set; }

        [Display(Name = "National ID")]
        [StringLength(25, ErrorMessage = "Maximum length is {1}")]
        public string NationalID { get; set; }

        [Display(Name = "Phone No")]
        [StringLength(35, ErrorMessage = "Maximum length is {1}")]
        public string PhoneNo { get; set; }

        [Display(Name = "Smart Card")]
        [StringLength(20, ErrorMessage = "Maximum length is {1}")]
        public string SmartCard { get; set; }

        [Display(Name = "Other Id No")]
        [StringLength(20, ErrorMessage = "Maximum length is {1}")]
        public string OtherIdNo { get; set; }


        [Display(Name = "Samity Name")]
        [NotMapped]
        public int CenterID { get; set; }
        [NotMapped]
        public string CenterCode { get; set; }
        [NotMapped]
        public string CenterName { get; set; }
        public IEnumerable<SelectListItem> centerListItems { get; set; }

    }
}