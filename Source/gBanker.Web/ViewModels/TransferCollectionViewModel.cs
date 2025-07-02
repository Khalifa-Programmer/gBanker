using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace gBanker.Web.ViewModels
{
    public class TransferCollectionViewModel
    {
        public int OfficeId { get; set; }
        public string MemberCode { get; set; }
        public string MemberName { get; set; }
        public string OfficeCode { get; set; }
        public string SamityCode { get; set; }
        public string SamityName { get; set; }
        public string ProductCode { get; set; }
        public decimal LoanPaid { get; set; }
        public decimal IntPaid { get; set; }
        public decimal Deposit { get; set; }
        public decimal SavingsCS { get; set; }
        public decimal TotalLoan { get; set; }


        public int CenterID { get; set; }
        public string CenterCode { get; set; }
        public string CenterName { get; set; }
        public IEnumerable<SelectListItem> CenterList { get; set; }
        public string ReportType { get; set; }
        public string EntCenterCode { get; set; }
        public string EntEmployeeCode { get; set; }
        public int IsUploaded { get; set; }

        public int RowSl { get; set; }

        public DateTime? CollectionDate { get; set; }


        public string EmployeeName { get; set; }
        public string EmployeeCode { get; set; }
        public short EmployeeID { get; set; }
    }
}