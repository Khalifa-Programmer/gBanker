using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace gBanker.Web.ViewModels
{
    public class BranchViewModel : BaseModel
    {
        [Required(ErrorMessage = "Branch ID is required")]
        [Display(Name = "Branch ID")]
        public short BranchID { get; set; }

        [Required(ErrorMessage = "Branch Code is required")]
        [Display(Name = "Branch Code")]
        public string BranchCode { get; set; }

        [Required(ErrorMessage = "Branch Name is required")]
        [Display(Name = "Branch Name")]
        public string BranchName { get; set; }

        [Display(Name = "Branch Name Bangla")]
        public string BranchNameBng { get; set; }

        [Display(Name = "Area")]
        public Nullable<int> AreaID { get; set; }

        [Display(Name = "Operation Start Date")]
        public System.DateTime OperationStartDate { get; set; }

         [Display(Name = "Geo Location")]
        public Nullable<int> GeoLocationID { get; set; }

        [Display(Name = "Branch Address")]
        public string BranchAddress { get; set; }

        [Display(Name = "Post Code")]
        public string PostCode { get; set; }

        [Display(Name = "Email Address")]
        public string Email { get; set; }

        [Display(Name = "Phone")]
        public string Phone { get; set; }

        [Display(Name = "Investor")]
        public Nullable<byte> InvestorID { get; set; }

        //public Nullable<bool> IsActive { get; set; }
        //public Nullable<System.DateTime> InActiveDate { get; set; }
        //public string CreateUser { get; set; }
        //public System.DateTime CreateDate { get; set; }

          
         //public IEnumerable<SelectListItem> PInvestorListItems { get; set; }
         //public IEnumerable<SelectListItem> PFrequencyListItems { get; set; }
         //public IEnumerable<SelectListItem> PCalcuationMethodListItems { get; set; }
    }
}