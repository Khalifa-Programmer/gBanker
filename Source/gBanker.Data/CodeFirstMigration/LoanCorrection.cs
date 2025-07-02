using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gBanker.Data.CodeFirstMigration
{
    [Table("LoanCorrection")]
    public partial class LoanCorrection
    {
        [Key]
        [Column(Order = 0)]
        public long LoanCorrectionID { get; set; }

        [Key]
        [Column(Order = 1, TypeName = "date")]
        public DateTime TrxDate { get; set; }

        [Key]
        [Column(Order = 2)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public long LoanSummaryID { get; set; }

        [Key]
        [Column(Order = 3)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int OfficeID { get; set; }

        [Key]
        [Column(Order = 4)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public long MemberID { get; set; }

        [Key]
        [Column(Order = 5)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public short ProductID { get; set; }

        [Key]
        [Column(Order = 6)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int CenterID { get; set; }

        [Key]
        [Column(Order = 7)]
        public byte MemberCategoryID { get; set; }

        [Key]
        [Column(Order = 8)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int LoanTerm { get; set; }

        [Key]
        [Column(Order = 9, TypeName = "date")]
        public DateTime InstallmentDate { get; set; }

        [Key]
        [Column(Order = 10, TypeName = "numeric")]
        public decimal PrincipalLoan { get; set; }

        [Key]
        [Column(Order = 11, TypeName = "numeric")]
        public decimal LoanDue { get; set; }

        [Key]
        [Column(Order = 12, TypeName = "numeric")]
        public decimal LoanPaid { get; set; }

        [Key]
        [Column(Order = 13, TypeName = "numeric")]
        public decimal IntCharge { get; set; }

        [Key]
        [Column(Order = 14, TypeName = "numeric")]
        public decimal IntDue { get; set; }

        [Key]
        [Column(Order = 15, TypeName = "numeric")]
        public decimal IntPaid { get; set; }

        [Key]
        [Column(Order = 16, TypeName = "numeric")]
        public decimal Advance { get; set; }

        [Key]
        [Column(Order = 17, TypeName = "numeric")]
        public decimal DueRecovery { get; set; }

        [Key]
        [Column(Order = 18)]
        public byte TrxType { get; set; }

        [Key]
        [Column(Order = 19)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public short InstallmentNo { get; set; }

        [Key]
        [Column(Order = 20)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public short EmployeeID { get; set; }

        public byte? InvestorID { get; set; }

        public bool? IsActive { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime? InActiveDate { get; set; }

        [Key]
        [Column(Order = 21)]
        [StringLength(35)]
        public string CreateUser { get; set; }

        [Key]
        [Column(Order = 22, TypeName = "smalldatetime")]
        public DateTime CreateDate { get; set; }
    }
}
