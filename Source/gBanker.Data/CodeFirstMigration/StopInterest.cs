using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gBanker.Data.CodeFirstMigration
{
    [Table("Stop_Interest")]
    public class StopInterest
    {        
        [Key]
        public Int64 StopInterestID { get; set; }

        [Display(Name = "Office")]
        [Required(ErrorMessage = "{0} is Required")]
        public int OfficeID { get; set; }

        [Display(Name = "Center")]
        [Required(ErrorMessage = "{0} is Required")]
        public int CenterID { get; set; }

        [Display(Name = "Member")]
        [Required(ErrorMessage = "{0} is Required")]
        public Int64 MemberID { get; set; }

        [Display(Name = "Product I D")]
        [Required(ErrorMessage = "{0} is Required")]
        public Int16? ProductID { get; set; }

        [Display(Name = "Stop Interest Date")]
        public DateTime? StopInterestDate { get; set; }

        [Display(Name = "Remarks")]
        [StringLength(50, ErrorMessage = "Maximum length is {1}")]
        public string Remarks { get; set; }

        [Display(Name = "Org I D")]
        [Required(ErrorMessage = "{0} is Required")]
        public int OrgID { get; set; }

        [Display(Name = "Is Active")]
        public bool? IsActive { get; set; }

        [Display(Name = "In Active Date")]
        public DateTime? InActiveDate { get; set; }

        [Display(Name = "Create User")]
        [Required(ErrorMessage = "{0} is Required")]
        [StringLength(15, ErrorMessage = "Maximum length is {1}")]
        public string CreateUser { get; set; }

        [Display(Name = "Create Date")]
        [Required(ErrorMessage = "{0} is Required")]
        public DateTime CreateDate { get; set; }
        public long SummaryID { get; set; }
        public byte ProdType { get; set; }
    }
}
