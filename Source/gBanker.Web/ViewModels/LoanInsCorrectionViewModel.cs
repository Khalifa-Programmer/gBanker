using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace gBanker.Web.ViewModels
{
    public class LoanInsCorrectionViewModel
    {
        public short EmployeeID { get; set; }
        public string EmployeeCode { get; set; }
        public string EmpName { get; set; }
        public int OfficeID { get; set; }
        public string OfficeCode { get; set; }
        public string OfficeName { get; set; }
        public System.DateTime TrxDate { get; set; }
        public long LoanSummaryID { get; set; }
        public string CenterCode { get; set; }
        public string CenterName { get; set; }
        public long newMemberID { get; set; }
        public string NewMemberCode { get; set; }
        public short NewProductID { get; set; }
        public string NewProductCode { get; set; }
        public string NewProdName { get; set; }
        public decimal LoanPaid { get; set; }
        public decimal IntPaid { get; set; }
        public long OldLoanSummaryID { get; set; }
        public long oldMemID { get; set; }
        public string OldMemCode { get; set; }
        public short ProductID { get; set; }
        public string ProductCode { get; set; }
        public string ProductFullNameEng { get; set; }
        public decimal OldLoanPaid { get; set; }
        public decimal OldIntPaid { get; set; }
        public Nullable<System.DateTime> OldTrxDate { get; set; }
        public System.DateTime CorrectionDate { get; set; }
        public string CreateUser { get; set; }

        public string OldName { get; set; }

        public string NewMemName { get; set; }
    }
}