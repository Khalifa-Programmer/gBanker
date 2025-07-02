using gBanker.Web.Filters;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace gBanker.Web.ViewModels
{
    public class getWriteOffListViewModel
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
         [Display(Name = "Write off year")]
        [Range(0, 100)]
        public int writeoffyear { get; set; }
        public Nullable<decimal> PrincipalLoan { get; set; }
        public Nullable<decimal> LoanPaid { get; set; }
        public Nullable<decimal> LoanBalance { get; set; }
        public Nullable<decimal> intPaid { get; set; }
        public Nullable<decimal> IntCharge { get; set; }
        public Nullable<decimal> intBal { get; set; }
        public Nullable<decimal> Balance { get; set; }
        public DateTime? DisburseDate { get; set; }
        public IEnumerable<SelectListItem> memberListItems { get; set; }
        public IEnumerable<SelectListItem> officeListItems { get; set; }
    }
}