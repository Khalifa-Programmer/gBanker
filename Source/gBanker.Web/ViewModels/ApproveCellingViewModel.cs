using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace gBanker.Web.ViewModels
{
    public class ApproveCellingViewModel
    {
        public long ApproveCellingID { get; set; }

        public int RoleID { get; set; }

        [Display(Name="User Role")]
        public List<int> RoleIDs { get; set; }

        [Column(TypeName = "numeric")]
        [Display(Name = "Range (Min)")]
        public decimal? MinRange { get; set; }

        [Column(TypeName = "numeric")]
        [Display(Name = "Range (Max)")]
        public decimal? MaxRange { get; set; }

        [Display(Name = "Product Type")]
        public int? ProdType { get; set; }

        [StringLength(150)]
        public string RoleName { get; set; }
        public IEnumerable<SelectListItem> roleList { get; set; }
        public IEnumerable<SelectListItem> producttype { get; set; }
        public IEnumerable<SelectListItem> productlist { get; set; }
        ///  public IEnumerable<SelectListItem> roleName { get; set; }

        [Display(Name = "Product")]
        public int? ProductId { get; set; }
        public string ProductName { get; set; }
    }
}