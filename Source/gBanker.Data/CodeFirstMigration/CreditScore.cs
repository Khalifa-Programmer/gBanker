using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gBanker.Data.CodeFirstMigration
{
    [Table("CreditScore")]
    public partial class CreditScore
    {
        public int? OfficeID { get; set; }

        [Key]
        [Column(Order = 0)]
        [StringLength(50)]
        public string CenterID { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(10)]
        public string ProductID { get; set; }

        [Key]
        [Column(Order = 2)]
        public byte LoanTerm { get; set; }

        [Key]
        [Column(Order = 3)]
        [StringLength(10)]
        public string MemberID { get; set; }

        [Key]
        [Column(Order = 4)]
        [StringLength(40)]
        public string OfficeName { get; set; }

        [Key]
        [Column(Order = 5)]
        [StringLength(40)]
        public string EmpName { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime? JoingDate { get; set; }

        [Key]
        [Column(Order = 6)]
        [StringLength(150)]
        public string MemberName { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime? DOB { get; set; }

        [StringLength(155)]
        public string RefereeName { get; set; }

        [StringLength(255)]
        public string MemberAddress { get; set; }

        [StringLength(35)]
        public string SSN { get; set; }

        [Key]
        [Column(Order = 7, TypeName = "numeric")]
        public decimal PrincipalLoan { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime? DisburseDate { get; set; }

        [Key]
        [Column(Order = 8, TypeName = "numeric")]
        public decimal LoanPaid { get; set; }

        [Key]
        [Column(Order = 9, TypeName = "numeric")]
        public decimal LoanBalance { get; set; }

        [Key]
        [Column(Order = 10, TypeName = "numeric")]
        public decimal InterestPaid { get; set; }

        [Key]
        [Column(Order = 11, TypeName = "numeric")]
        public decimal LoanInstallment { get; set; }

        [Key]
        [Column(Order = 12, TypeName = "numeric")]
        public decimal IntInstallment { get; set; }

        [Key]
        [Column(Order = 13)]
        [StringLength(155)]
        public string LoanItem { get; set; }

        [StringLength(100)]
        public string PurposeName { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime? LastInstallmentDate { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? LoanPaid_ThisMonth { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? IntPaid_ThisMonth { get; set; }

        [Key]
        [Column(Order = 14)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public short LoanDuration { get; set; }

        [Key]
        [Column(Order = 15)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public short WeekPassed { get; set; }

        [Key]
        [Column(Order = 16)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public short DropInstallment { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime? AccountCloseDate { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime? LastPaymentDate { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? LastLoanPaid { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? LastIntPaid { get; set; }

        [Column(TypeName = "date")]
        public DateTime? ReportDate { get; set; }

        [StringLength(35)]
        public string MemberFirstName { get; set; }

        [StringLength(35)]
        public string MemberMiddleName { get; set; }

        [StringLength(35)]
        public string MemberLastName { get; set; }

        [StringLength(75)]
        public string StreetAddress1 { get; set; }

        [StringLength(75)]
        public string StreetAddress2 { get; set; }

        [StringLength(35)]
        public string City { get; set; }

        [StringLength(35)]
        public string State { get; set; }

        [StringLength(5)]
        public string ZipCode { get; set; }

        [StringLength(15)]
        public string TypeofID { get; set; }

        [StringLength(25)]
        public string IDComments { get; set; }

        [StringLength(35)]
        public string Race { get; set; }

        [StringLength(35)]
        public string Ethnicity { get; set; }
    }
}
