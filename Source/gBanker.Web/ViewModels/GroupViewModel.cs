using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace gBanker.Web.ViewModels
{
    public class GroupViewModel : BaseModel
    {
        public short GroupID { get; set; }

        [Required]
        [StringLength(50)]
        [Display(Name = "Group Code")]
        public string GroupCode { get; set; }

        public int OfficeID { get; set; }
          [Display(Name = "Office Code")]
        public string OfficeCode { get; set; }
          [Display(Name = "Office Name")]
        public string OfficeName { get; set; }
          [Display(Name = "Formation Date")]
        [Column(TypeName = "date")]
        public DateTime FormationDate { get; set; }
         [Display(Name = "Group Status")]
        public byte GroupStatus { get; set; }

        public bool? IsActive { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime? InActiveDate { get; set; }

        public IEnumerable<SelectListItem> StatusMode { get; set; }

    }
}