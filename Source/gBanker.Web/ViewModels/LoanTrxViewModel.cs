using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace gBanker.Web.ViewModels
{
    public class LoanTrxViewModel//:BaseModel
    {
        public long LoanTrxID { get; set; }
        public DateTime TrxDate { get; set; }
        public long LoanSummaryID { get; set; }
        public int OfficeID { get; set; }

        public long MemberID { get; set; }
        public int ProductID { get; set; }
        public int CenterID { get; set; }
        public int MemberCategoryID { get; set; }

        public int LoanTerm { get; set; }
        public DateTime InstallmentDate { get; set; }
        public decimal PrincipalLoan { get; set; }
        public decimal LoanDue { get; set; }

        public decimal LoanPaid { get; set; }
        public decimal IntCharge { get; set; }
        public decimal IntDue { get; set; }
        public decimal IntPaid { get; set; }

        public decimal Advance { get; set; }
        public decimal DueRecovery { get; set; }
        public byte TrxType { get; set; }
        public short InstallmentNo { get; set; }

        public short EmployeeID { get; set; }
        public byte? InvestorID { get; set; }
        public int OrgID { get; set; }


        public bool? IsActive { get; set; }


        public DateTime? InActiveDate { get; set; }

        [Required]
        [StringLength(35)]
        public string CreateUser { get; set; }


        public DateTime CreateDate { get; set; }
        public int rowSl { get; set; }


    }// END Class
}// END NameSpace