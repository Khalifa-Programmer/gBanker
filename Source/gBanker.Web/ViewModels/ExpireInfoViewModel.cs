using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace gBanker.Web.ViewModels
{
    public class ExpireInfoViewModel :BaseModel
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public long ExpireInfoID { get; set; }

        public int OfficeID { get; set; }
        public string OfficeCode { get; set; }
        [Display(Name = "Samity ID")]
        public int CenterID { get; set; }
        [Display(Name = "Samity Code")]
        public string CenterCode { get; set; }
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long MemberID { get; set; }
        public string MemberCode { get; set; }
        public string MemberName { get; set; }
        [StringLength(50)]
        public string ExpiryName { get; set; }
        [StringLength(50)]
        
        [Required]
        public string Relation { get; set; }
        [Column(TypeName = "date")]
        public DateTime? ExpireDate { get; set; }
        [Required]
        [StringLength(50)]
        [Display(Name = "Cause Of Death")]
        public string Remarks { get; set; }

        public int OrgID { get; set; }
        [Display(Name = "Type Of Death")]
        public int ExpireStatus { get; set; }
        public bool? IsActive { get; set; }

        public int? ProductID { get; set; }

        [Display(Name = "Principle Balance")]
        public decimal PrincipleBalance { get; set; }
        [Display(Name = "Service Charge")]
        public decimal ServiceCharge { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime? InActiveDate { get; set; }

        public string ApplicantName { get; set; }
        public string MobileNumber { get; set; }

        public IEnumerable<SelectListItem> ExpireList { get; set; }
        public IEnumerable<SelectListItem> productListItems { get; set; }
        public IEnumerable<SelectListItem> investorListItems { get; set; }
        public IEnumerable<SelectListItem> centerListItems { get; set; }
        public IEnumerable<SelectListItem> officeListItems { get; set; }
        public IEnumerable<SelectListItem> membercategoryListItems { get; set; }
        public IEnumerable<SelectListItem> purposeListItems { get; set; }
        public IEnumerable<SelectListItem> memberListItems { get; set; }

    }
}