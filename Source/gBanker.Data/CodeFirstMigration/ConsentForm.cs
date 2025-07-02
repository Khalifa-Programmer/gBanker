using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gBanker.Data.CodeFirstMigration
{
    [Table("ConsentForm")]
    public partial class ConsentForm
    {
        [Key]
        [Display(Name = "Id")]
        [Required(ErrorMessage = "{0} is Required")]
        public Int64 Id { get; set; }

        [Display(Name = "Office I D")]
        public int? OfficeID { get; set; }

        [Display(Name = "Center I D")]
        public int? CenterID { get; set; }

        [Display(Name = "Member I D")]
        public Int64? MemberID { get; set; }

        [Display(Name = "Product I D")]
        public int? ProductID { get; set; }

        [Display(Name = "No Account")]
        public int? NoAccount { get; set; }

        [Display(Name = "Tran Date")]
        public DateTime? TranDate { get; set; }

        [Display(Name = "Org I D")]
        public int? OrgID { get; set; }

        [Display(Name = "Saving Summary I D")]
        public Int64? SavingSummaryID { get; set; }

        [Display(Name = "Stop Status")]
        public int? StopStatus { get; set; }

        [Display(Name = "Stop Or Claimable")]
        public int? StopOrClaimable { get; set; }

        [Display(Name = "Create User")]
        [StringLength(35, ErrorMessage = "Maximum length is {1}")]

        //public bool? IsActive { get; set; }

        public string CreateUser { get; set; }

        [Display(Name = "Create Date")]
        public DateTime? CreateDate { get; set; }
    }
}
