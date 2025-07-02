//using ExcelMigration.ViewModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace gBanker.Web.ViewModels
{
    public class AuditToolsViewModel
    {
        public int? Sl { get; set; }
        public string SummaryID { get; set; }
        public string AccountNo { get; set; }

        public string ReturnMessage { get; set; }
        public int Result { get; set; }
        public bool IsSuperAdmin { get; set; }
        [Display(Name = "Upload File")]
        public HttpPostedFileBase UploadFile { get; set; }

        public int OfficeId { get; set; }

        public int CenterID { get; set; }
        [Display(Name = "Samity Code")]
        public string CenterCode { get; set; }

        public DateTime TrxDate { get; set; }
        public long MemberID { get; set; }
        public string MemberCode { get; set; }
        public string MemberName { get; set; }

        //public List<ValidateResultModel> WrongStaffInfoList { get; set; }
        //public List<ValidateResultModel> WrongCenterInfoList { get; set; }
        //public List<ValidateResultModel> WrongCustomerInfoList { get; set; }
        public List<SelectListItem> BranchCodeList { get; set; }
        public List<SelectListItem> ReportSpNameList { get; set; }


        public List<dynamic> CheckStaffDataTable { get; set; }
        public List<dynamic> CheckStaffDataTable2 { get; set; }
        public List<dynamic> CheckStaffDataTable3 { get; set; }


        public IEnumerable<SelectListItem> cashListItems { get; set; }
        public IEnumerable<SelectListItem> productListItems { get; set; }

        public IEnumerable<SelectListItem> centerListItems { get; set; }
        public IEnumerable<SelectListItem> officeListItems { get; set; }
        public IEnumerable<SelectListItem> membercategoryListItems { get; set; }
        public IEnumerable<SelectListItem> purposeListItems { get; set; }
        public IEnumerable<SelectListItem> memberListItems { get; set; }
        public string memName { get; set; }


    }// End Class

    public class ValidateResultModelAuditTools
    {
        public string SheetName { get; set; }
        public int RowSl { get; set; }
        public string ColumnName { get; set; }
        public string Message { get; set; }

    }// ENDClass

    public class ValidateColumnAuditTools
    {
        public string Value { get; set; }
        public bool Result { get; set; }
    }// END CLASS

}// End Namespace