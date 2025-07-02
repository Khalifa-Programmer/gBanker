using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace gBanker.Web.ViewModels
{
    public class LegalInfoViewModel
    {
        public long Id { get; set; }

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
        public int ProductID { get; set; }

        [Display(Name = "Case No")]
        [Required(ErrorMessage = "{0} is Required")]
        [StringLength(50, ErrorMessage = "Maximum length is {1}")]
        public string CaseNo { get; set; }

        [Display(Name = "Case Date")]
        [Required(ErrorMessage = "{0} is Required")]
        public DateTime CaseDate { get; set; }

        [Display(Name = "Remarks")]
        [StringLength(250, ErrorMessage = "Maximum length is {1}")]
        public string Remarks { get; set; }
        public string OfficeName { get; set; }
        public string CenterName { get; set; }
        public string MemberName { get; set; }
        public string ProductName { get; set; }
        public string CaseDateS { get; set; }
        public IEnumerable<SelectListItem> productListItems { get; set; }
        public IEnumerable<SelectListItem> AccountListItems { get; set; }
        public IEnumerable<SelectListItem> centerListItems { get; set; }
        public IEnumerable<SelectListItem> officeListItems { get; set; }        
        public IEnumerable<SelectListItem> memberListItems { get; set; }
       
    }
}