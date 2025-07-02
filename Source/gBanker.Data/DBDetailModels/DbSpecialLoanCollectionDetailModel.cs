using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gBanker.Data.DBDetailModels
{
    public class DbSpecialLoanCollectionDetailModel
    {
        public long DailyLoanTrxID { get; set; }

        [Column(TypeName = "date")]
        public DateTime TrxDate { get; set; }

        public long LoanSummaryID { get; set; }

        public int OfficeID { get; set; }
        public string OfficeCode { get; set; }
        public string OfficeName { get; set; }
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

        [StringLength(1)]
        public string InterestCalculationMethod { get; set; }

        public int CenterID { get; set; }

        public string CenterCode { get; set; }
        public string CenterName { get; set; }
        public byte MemberCategoryID { get; set; }

        public int LoanTerm { get; set; }

        public short PurposeID { get; set; }

        [Column(TypeName = "date")]
        public DateTime InstallmentDate { get; set; }

        [Column(TypeName = "numeric")]
        public decimal PrincipalLoan { get; set; }

        [Column(TypeName = "numeric")]
        public decimal LoanRepaid { get; set; }

        [Column(TypeName = "numeric")]
        public decimal LoanDue { get; set; }

        [Column(TypeName = "numeric")]
        public decimal LoanPaid { get; set; }

        [Column(TypeName = "numeric")]
        public decimal CumIntCharge { get; set; }

        [Column(TypeName = "numeric")]
        public decimal IntCharge { get; set; }

        [Column(TypeName = "numeric")]
        public decimal IntDue { get; set; }

        [Column(TypeName = "numeric")]
        public decimal IntPaid { get; set; }

        [Column(TypeName = "numeric")]
        public decimal Advance { get; set; }

        [Column(TypeName = "numeric")]
        public decimal DueRecovery { get; set; }

        public byte TrxType { get; set; }

        public short InstallmentNo { get; set; }

        public short EmployeeID { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? TotalPaid { get; set; }
    }
}
