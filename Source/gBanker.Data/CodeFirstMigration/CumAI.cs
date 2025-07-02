using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gBanker.Data.CodeFirstMigration
{
    [Table("CumAIS")]
    public partial class CumAI
    {
        [Key]
        public int CumAisID { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime AISDate { get; set; }

        [Required]
        [StringLength(50)]
        public string VoucherNo { get; set; }

        [Required]
        [StringLength(50)]
        public string OfficeID { get; set; }

        [Required]
        [StringLength(50)]
        public string AccCode { get; set; }

        [StringLength(200)]
        public string Naration { get; set; }

        [StringLength(50)]
        public string ReconPurposeCode { get; set; }

        [StringLength(50)]
        public string Reference { get; set; }

        [Column(TypeName = "numeric")]
        public decimal Debit { get; set; }

        [Column(TypeName = "numeric")]
        public decimal Credit { get; set; }

        [StringLength(50)]
        public string VoucherType { get; set; }

        [Required]
        [StringLength(55)]
        public string CreateUser { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime CreateDate { get; set; }
    }
}
