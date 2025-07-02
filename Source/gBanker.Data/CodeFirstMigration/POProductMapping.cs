using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gBanker.Data.CodeFirstMigration
{
    [Table("pksf.POProductMapping")]
    public partial class POProductMapping
    {
        [Key]
        [Display(Name = "Id")]
        [Required(ErrorMessage = "{0} is Required")]
        public int Id { get; set; }

        [Display(Name = "PO Code")]
        [Required(ErrorMessage = "{0} is Required")]
        [StringLength(50, ErrorMessage = "Maximum length is {1}")]
        public string POCode { get; set; }

        [Display(Name = "PO Id")]
        [Required(ErrorMessage = "{0} is Required")]
        public int POId { get; set; }

        [Display(Name = "Loan Code")]
        [Required(ErrorMessage = "{0} is Required")]
        [StringLength(50, ErrorMessage = "Maximum length is {1}")]
        public string LoanCode { get; set; }

        [Display(Name = "Product Code")]
        [Required(ErrorMessage = "{0} is Required")]
        [StringLength(50, ErrorMessage = "Maximum length is {1}")]
        public string ProductCode { get; set; }

        [Display(Name = "Product Name")]
        [Required(ErrorMessage = "{0} is Required")]
        [StringLength(100, ErrorMessage = "Maximum length is {1}")]
        public string ProductName { get; set; }

        [Display(Name = "Loan Service Charge Rate")]
        [Required(ErrorMessage = "{0} is Required")]
        public decimal LoanServiceChargeRate { get; set; }

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

        [NotMapped]
        public string AssociatedLoanCode { get; set; }
    }
}
