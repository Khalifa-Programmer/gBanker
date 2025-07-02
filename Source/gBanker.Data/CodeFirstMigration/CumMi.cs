using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gBanker.Data.CodeFirstMigration
{
     [Table("CumMIS")]
    public partial class CumMi
    {
        [Key]
        public int CumMisID { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime MisDate { get; set; }

        public int OfficeID { get; set; }

        public int CenterID { get; set; }

        public int ProductID { get; set; }

        public int InvestorID { get; set; }

        [Required]
        [StringLength(2)]
        public string Gender { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? NoOfLoanee { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? UpToLoanDis { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? UptoDisburseMent { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? UpToRecovery { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? UptoAdmission { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? UpToDropOut { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? UptoFullyRepaid { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? UptoDeposit { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? UptoInterest { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? uptowithdrawal { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? WriteOffLoan { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? WriteOffInterest { get; set; }

        public int? CumMisItemID { get; set; }
    }
}
