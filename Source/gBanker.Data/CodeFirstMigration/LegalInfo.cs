using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gBanker.Data.CodeFirstMigration
{
    [Table("LegalInfo")]
    public class LegalInfo
    {
        [Key]
        [Display(Name = "Id")]
        [Required(ErrorMessage = "{0} is Required")]
        public long Id { get; set; }

        [Display(Name = "Office I D")]
        [Required(ErrorMessage = "{0} is Required")]
        public int OfficeID { get; set; }

        [Display(Name = "Center I D")]
        [Required(ErrorMessage = "{0} is Required")]
        public int CenterID { get; set; }

        [Display(Name = "Member I D")]
        [Required(ErrorMessage = "{0} is Required")]
        public Int64 MemberID { get; set; }

        [Display(Name = "Product I D")]
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

        [Display(Name = "Is Active")]
        [Required(ErrorMessage = "{0} is Required")]
        public bool IsActive { get; set; }

        [Display(Name = "Create User")]
        [StringLength(50, ErrorMessage = "Maximum length is {1}")]
        public string CreateUser { get; set; }

        [Display(Name = "Create Date")]
        public DateTime? CreateDate { get; set; }

        [Display(Name = "Update User")]
        [StringLength(50, ErrorMessage = "Maximum length is {1}")]
        public string UpdateUser { get; set; }

        [Display(Name = "Update Date")]
        public DateTime? UpdateDate { get; set; }
    }
}
