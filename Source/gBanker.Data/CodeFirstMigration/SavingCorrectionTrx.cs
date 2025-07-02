using gBankerCodeFirstMigration.Db;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace gBanker.Data.CodeFirstMigration
{
    [Table("SavingCorrectionTrx")]
    public partial class SavingCorrectionTrx
    {
        public long SavingCorrectionTrxID { get; set; }

        public long SavingSummaryID { get; set; }

        public int OfficeID { get; set; }

        public long MemberID { get; set; }

        [StringLength(10)]
        public string MemberCode { get; set; }

        [StringLength(110)]
        public string MemberName { get; set; }

        public short ProductID { get; set; }

        [StringLength(10)]
        public string ProductCode { get; set; }

        [StringLength(50)]
        public string ProductName { get; set; }

        public int CenterID { get; set; }

        public int NoOfAccount { get; set; }

        [Column(TypeName = "date")]
        public DateTime TransactionDate { get; set; }

        [Column(TypeName = "numeric")]
        public decimal DueSavingInstallment { get; set; }

        [Column(TypeName = "numeric")]
        public decimal SavingInstallment { get; set; }

        [Column(TypeName = "numeric")]
        public decimal Deposit { get; set; }

        [Column(TypeName = "numeric")]
        public decimal Withdrawal { get; set; }

        [Column(TypeName = "numeric")]
        public decimal Balance { get; set; }

        [Column(TypeName = "numeric")]
        public decimal Penalty { get; set; }

        public byte TransType { get; set; }

        [Column(TypeName = "numeric")]
        public decimal MonthlyInterest { get; set; }

        public bool PresenceInd { get; set; }

        [Column(TypeName = "numeric")]
        public decimal TransferDeposit { get; set; }

        [Column(TypeName = "numeric")]
        public decimal TransferWithdrawal { get; set; }

        public short? EmployeeID { get; set; }

        public byte? MemberCategoryID { get; set; }

        public long IsActive { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime? InActiveDate { get; set; }

        [Required]
        [StringLength(35)]
        public string CreateUser { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime CreateDate { get; set; }
        public int OrgID { get; set; }
        public virtual Organization Organization { get; set; }
    }
}
