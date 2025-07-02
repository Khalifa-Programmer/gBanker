using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gBanker.Data.CodeFirstMigration
{
    public class LoanSummaryCorrection
    {

        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public long MemberID { get; set; }

        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public long NewMemberID { get; set; }

        [Key]
        [Column(Order = 2)]
        [StringLength(20)]
        public string OldMemberCode { get; set; }

        [Key]
        [Column(Order = 3)]
        [StringLength(20)]
        public string MemberCode { get; set; }

        [StringLength(107)]
        public string OldName { get; set; }

        [StringLength(107)]
        public string MemberName { get; set; }

        [Key]
        [Column(Order = 4)]
        [StringLength(50)]
        public string OldCenterCode { get; set; }

        [Key]
        [Column(Order = 5)]
        [StringLength(50)]
        public string OldCenterName { get; set; }

        [Key]
        [Column(Order = 6)]
        [StringLength(50)]
        public string CenterCode { get; set; }

        [Key]
        [Column(Order = 7)]
        [StringLength(50)]
        public string CenterName { get; set; }

        [Key]
        [Column(Order = 8)]
        [StringLength(10)]
        public string OldProduct { get; set; }

        [Key]
        [Column(Order = 9)]
        [StringLength(150)]
        public string OLdProdName { get; set; }

        [Key]
        [Column(Order = 10)]
        [StringLength(10)]
        public string Product { get; set; }

        [Key]
        [Column(Order = 11)]
        [StringLength(150)]
        public string ProdName { get; set; }

        [Key]
        [Column(Order = 12, TypeName = "numeric")]
        public decimal OldPrin { get; set; }

        [Key]
        [Column(Order = 13, TypeName = "numeric")]
        public decimal UpdatedDIsbursedLoan { get; set; }

        [Column(TypeName = "date")]
        public DateTime? OldDisDate { get; set; }

        public DateTime? CorrectionDate { get; set; }

        [Key]
        [Column(Order = 14)]
        [StringLength(10)]
        public string EmployeeCode { get; set; }

        [Key]
        [Column(Order = 15)]
        [StringLength(50)]
        public string EmpName { get; set; }

        [Key]
        [Column(Order = 16)]
        [StringLength(35)]
        public string CreateUser { get; set; }

        [Key]
        [Column(Order = 17)]
        [StringLength(10)]
        public string OfficeCode { get; set; }

        [Key]
        [Column(Order = 18)]
        [StringLength(40)]
        public string OfficeName { get; set; }
    }
}
