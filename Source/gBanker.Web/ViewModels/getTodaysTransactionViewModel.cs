using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace gBanker.Web.ViewModels
{
    public class getTodaysTransactionViewModel
    {
        public int OfficeID { get; set; }
        public string OfficeCode { get; set; }
        public long MemberID { get; set; }
        public string MemberCode { get; set; }
        public string FirstName { get; set; }
        public short ProductID { get; set; }
        public string ProductCode { get; set; }
        [Display(Name = "Samity ID")]
        public int CenterID { get; set; }
        [Display(Name = "Samity Code")]
        public string CenterCode { get; set; }
        public int LoanTerm { get; set; }
        public System.DateTime InstallmentDate { get; set; }
        public decimal PrincipalLoan { get; set; }
        public decimal LoanDue { get; set; }
        public decimal LoanPaid { get; set; }
        public decimal IntCharge { get; set; }
        public decimal IntDue { get; set; }
        public decimal IntPaid { get; set; }
        public short InstallmentNo { get; set; }
        public string CreateUser { get; set; }
        public System.DateTime CreateDate { get; set; }
        public short TrxType { get; set; }        
    }
}