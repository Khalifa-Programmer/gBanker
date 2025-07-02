using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace gBanker.Web.ViewModels
{
    public class WorkingAreaLogViewModel : BaseModel
    {
       
        public long WorkingAreaLogID { get; set; }

        public int? OfficeID { get; set; }

        [StringLength(10)]
        [Display(Name = "Program")]
        public string MainProductCode { get; set; }
      
        public string Program { get; set; }

        [StringLength(200)]
        public string WorkingArea { get; set; }

        [Column(TypeName = "numeric")]
        [Display(Name = "No. of Upzilla")]
        public decimal? Upzilla { get; set; }

        [Column(TypeName = "numeric")]
        [Display(Name = "No. of Municipality")]
        public decimal? Municipality { get; set; }

        [Column(TypeName = "numeric")]
        [Display(Name = "No. of village")]
        public decimal? Village { get; set; }

        [Column(TypeName = "numeric")]
        [Display(Name = "SelfEmploymentMale")]
        public decimal? SelfEnterprenuerMale { get; set; }

        [Column(TypeName = "numeric")]
        [Display(Name = "SelfEmploymentFeMale")]
        public decimal? SelfEnterprenuerFeMale { get; set; }

        [Column(TypeName = "numeric")]
        [Display(Name = "PaidEmploymentMale")]
        public decimal? PaidEnterPrenuerOwnFamilyMale { get; set; }

        [Column(TypeName = "numeric")]
        [Display(Name = "PaidEmploymentFeMale")]
        public decimal? PaidEnterPrenuerOwnFamilyFeMale { get; set; }

        [Column(TypeName = "numeric")]
        [Display(Name = "PaidEmploymentOutSideMale")]
        public decimal? PaidEnterPrenuerOutSideMale { get; set; }

        [Column(TypeName = "numeric")]
        [Display(Name = "PaidEmploymentOutSideFeMale")]
        public decimal? PaidEnterPrenuerOutSideFeMale { get; set; }
         [Display(Name = "Entry Date")]
        public DateTime? EntryDate { get; set; }
        public IEnumerable<SelectListItem> OfficeList { get; set; }
        public IEnumerable<SelectListItem> productListItems { get; set; }
    }
}