using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace gBanker.Web.ViewModels
{
    //New
    public class MemberWisePassbookBalanceEntryViewModel : BaseModel
    {
        public Int64 MemberWisePassbookBalanceEntryId { get; set; }

        [Display(Name = "Office Id")]
        [Required(ErrorMessage = "{0} is Required")]
        public int OfficeId { get; set; }

        [Display(Name = "Center Id")]
        [Required(ErrorMessage = "{0} is Required")]
        public int CenterId { get; set; }

        [Display(Name = "Member Id")]
        [Required(ErrorMessage = "{0} is Required")]
        public int MemberId { get; set; }

        [Display(Name = "Product Id")]
        [Required(ErrorMessage = "{0} is Required")]
        public int ProductId { get; set; }

        [Display(Name = "Amount")]
        [Required(ErrorMessage = "{0} is Required")]
        public decimal Amount { get; set; }

        public IEnumerable<SelectListItem> ProductList { get; set; }
        public IEnumerable<SelectListItem> centerListItems { get; set; }
        public IEnumerable<SelectListItem> officeListItems { get; set; }
        public IEnumerable<SelectListItem> memberListItems { get; set; }

        [NotMapped]
        [Display(Name = "Member")]
        public string MemberName { get; set; }

    }
}