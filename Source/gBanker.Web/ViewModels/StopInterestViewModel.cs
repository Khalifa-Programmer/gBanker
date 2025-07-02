using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace gBanker.Web.ViewModels
{
    public class StopInterestViewModel
    {
        public long StopInterestID { get; set; }

        [Display(Name = "Office")]
        [Required(ErrorMessage = "{0} is Required")]
        public int OfficeID { get; set; }

        [Display(Name = "Center")]
        [Required(ErrorMessage = "{0} is Required")]
        public int CenterID { get; set; }

        [Display(Name = "Member")]
        [Required(ErrorMessage = "{0} is Required")]
        public Int64 MemberID { get; set; }

        [Display(Name = "Product")]
        [Required(ErrorMessage = "{0} is Required")]
        public Int16? ProductID { get; set; }

        [Display(Name = "Stop Interest Date")]
        [Required(ErrorMessage = "{0} is Required")]
        public DateTime? StopInterestDate { get; set; }
        public string StopInterestDateView { get; set; }

        [Display(Name = "Remarks")]
        [StringLength(50, ErrorMessage = "Maximum length is {1}")]
        public string Remarks { get; set; }
        public long SummaryID { get; set; }

        [Display(Name = "Product Type")]
        [Required(ErrorMessage = "{0} is Required")]
        public byte ProdType { get; set; }
        public string ProductName { get; set; }
        public string OfficeName { get; set; }
        public string CenterName { get; set; }
        public string MemberName { get; set; }
        public IEnumerable<SelectListItem> memberListItems { get; set; }
        public IEnumerable<SelectListItem> centerListItems { get; set; }
        public IEnumerable<SelectListItem> officeListItems { get; set; }
        public IEnumerable<SelectListItem> productListItems { get; set; }
        public IEnumerable<SelectListItem> ProductTypeList { get; set; }

    }
}