using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace gBanker.Web.ViewModels
{
    public class FamilyGraceViewModel : BaseModel
    {
        public int FamilyGraceID { get; set; }

        public int? OrgID { get; set; }
        public string OfficeCode { get; set; }
        public string MemberCode { get; set; }
       [Display(Name = "Samity Code")]
        public string CenterCode { get; set; }
        public int? OfficeID { get; set; }
         [Display(Name = "Samity")]
        public int? CenterID { get; set; }

        public long? MemberID { get; set; }
      
        public Nullable<System.DateTime> GraceStartDate { get; set; }
    
        public Nullable<System.DateTime> GraceEndDate { get; set; }

     
        public string Description { get; set; }
        //public bool? IsActive { get; set; }

      
        //public DateTime? InActiveDate { get; set; }

      
        public IEnumerable<SelectListItem> memberListItems { get; set; }
        public IEnumerable<SelectListItem> centerListItems { get; set; }
        public IEnumerable<SelectListItem> officeListItems { get; set; }
    }
}