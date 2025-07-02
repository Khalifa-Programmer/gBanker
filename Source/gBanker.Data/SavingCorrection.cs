using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gBanker.Data
{
    [Table("SavingCorrection")]
    public partial class SavingCorrection
    {
        [Key]
        [Column(Order = 0)]
        public long SavingCorrectionID { get; set; }

        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public long SavingSummaryID { get; set; }

        [Key]
        [Column(Order = 2)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int OfficeID { get; set; }

        [Key]
        [Column(Order = 3)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public long MemberID { get; set; }

        [Key]
        [Column(Order = 4)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public short ProductID { get; set; }

        [Key]
        [Column(Order = 5)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int CenterID { get; set; }

        [Key]
        [Column(Order = 6)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int NoOfAccount { get; set; }

        [Key]
        [Column(Order = 7, TypeName = "date")]
        public DateTime TransactionDate { get; set; }

        [Key]
        [Column(Order = 8, TypeName = "numeric")]
        public decimal Deposit { get; set; }

        [Key]
        [Column(Order = 9, TypeName = "numeric")]
        public decimal Withdrawal { get; set; }

        [Key]
        [Column(Order = 10, TypeName = "numeric")]
        public decimal Balance { get; set; }

        [Key]
        [Column(Order = 11, TypeName = "numeric")]
        public decimal Penalty { get; set; }

        [Key]
        [Column(Order = 12)]
        public byte TransType { get; set; }

        [Key]
        [Column(Order = 13, TypeName = "numeric")]
        public decimal MonthlyInterest { get; set; }

        [Key]
        [Column(Order = 14)]
        public bool PresenceInd { get; set; }

        [Key]
        [Column(Order = 15, TypeName = "numeric")]
        public decimal TransferDeposit { get; set; }

        [Key]
        [Column(Order = 16, TypeName = "numeric")]
        public decimal TransferWithdrawal { get; set; }

        [Key]
        [Column(Order = 17)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int EmployeeID { get; set; }

        [Key]
        [Column(Order = 18)]
        public byte MemberCategoryID { get; set; }

        [Key]
        [Column(Order = 19)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public long IsActive { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime? InActiveDate { get; set; }

        [Key]
        [Column(Order = 20)]
        [StringLength(35)]
        public string CreateUser { get; set; }

        [Key]
        [Column(Order = 21, TypeName = "smalldatetime")]
        public DateTime CreateDate { get; set; }
    }
}
