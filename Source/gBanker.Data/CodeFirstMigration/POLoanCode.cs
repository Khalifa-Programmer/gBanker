using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace gBanker.Data.CodeFirstMigration
{
    [Table("pksf.POLoanCode")]  
    public partial class POLoanCode
    {
        [Key]        
        public int Id { get; set; }

        [Display(Name = "po name")]
        [Required(ErrorMessage = "{0} is Required")]
        [StringLength(30, ErrorMessage = "Maximum length is {1}")]
        public string LoanCode { get; set; }

        [Display(Name = "Associated Loan Code")]
        public string AssociatedLoanCode { get; set; }

        [Display(Name = "Functionalities And Features")]
        [Required(ErrorMessage = "{0} is Required")]
        [StringLength(500, ErrorMessage = "Maximum length is {1}")]
        public string FunctionalitiesAndFeatures { get; set; }


        [Display(Name = "Associated AccCode FA")]        
        [StringLength(500, ErrorMessage = "Maximum length is {1}")]
        public string AssociatedAccCodeFA { get; set; }

        [Display(Name = "Associated AccCode SCP")]        
        [StringLength(500, ErrorMessage = "Maximum length is {1}")]
        public string AssociatedAccCodeSCP { get; set; }

        [Display(Name = "Is Active")]
        [Required(ErrorMessage = "{0} is Required")]
        public bool IsActive { get; set; }

        [Display(Name = "Create User")]
        public int? CreateUser { get; set; }

        [Display(Name = "Create Date")]
        public DateTime? CreateDate { get; set; }

        [Display(Name = "Update User")]
        public int? UpdateUser { get; set; }

        [Display(Name = "Update Date")]
        public DateTime? UpdateDate { get; set; }
    }
}
