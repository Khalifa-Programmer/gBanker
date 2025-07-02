namespace gBanker.Data.CodeFirstMigration.Db
{
    using gBankerCodeFirstMigration.Db;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Product")]
    public partial class Product
    {
        public Product()
        {
            DailyLoanTrxes = new HashSet<DailyLoanTrx>();
            LoanSummaries = new HashSet<LoanSummary>();
            LoanTrxes = new HashSet<LoanTrx>();
            SavingSummaries = new HashSet<SavingSummary>();
        }

        public short ProductID { get; set; }

        [Required]
        [StringLength(10)]
        public string ProductCode { get; set; }

        [Required]
        [StringLength(50)]
        public string ProductName { get; set; }

        [StringLength(100)]
        public string ProductFullNameEng { get; set; }

        [StringLength(10)]
        public string ProductShortNameBng { get; set; }

        [StringLength(100)]
        public string ProductFullNameBng { get; set; }

        public byte? ProductType { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? InterestRate { get; set; }

        public short? Duration { get; set; }

        [StringLength(10)]
        public string MainProductCode { get; set; }

        [Column(TypeName = "numeric")]
        
        public decimal? LoanInstallment { get; set; }

        [Column(TypeName = "numeric")]

        public decimal? InterestInstallment { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? SavingsInstallment { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? MinLimit { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? MaxLimit { get; set; }

        [StringLength(1)]
        public string InterestCalculationMethod { get; set; }

        [StringLength(1)]
        public string PaymentFrequency { get; set; }

        [StringLength(7)]
        public string InsuranceItemCode { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? InsuranceItemRate { get; set; }

        [Required]
        [StringLength(100)]
        public string MainItemName { get; set; }
        public string SubMainCategory { get; set; }
        public decimal? LateFeePercentage { get; set; }


        public int OrgID { get; set; }
        public int GracePeriod { get; set; }
        public bool? IsActive { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime? InActiveDate { get; set; }

        [Required]
        [StringLength(15)]
        public string CreateUser { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime CreateDate { get; set; }
        public byte DurationOverCollection { get; set; }
        public virtual Organization Organization { get; set; }

        public virtual ICollection<DailyLoanTrx> DailyLoanTrxes { get; set; }

        public virtual ICollection<LoanSummary> LoanSummaries { get; set; }

        public virtual ICollection<LoanTrx> LoanTrxes { get; set; }

        public virtual ICollection<SavingSummary> SavingSummaries { get; set; }
        public virtual ICollection<RepaymentSchedule> RepaymentSchedules { get; set; }
        public byte? NextDisbursementDuration { get; set; }
        public byte? YearlyInterestPayable { get; set; }


    }
}
