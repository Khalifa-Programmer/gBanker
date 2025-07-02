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
    public class SMSSendViewModel
    {
        public string ddlSENDType { get; set; }
        public string SendTo { get; set; }
        public string ddlSendMessageType { get; set; }
        public string ddlSENDSubCategories { get; set; }
        public string SENDMessageDetailsId { get; set; }
        public string txtSENDWebLink { get; set; }
        public string txtSENDDate { get; set; }
    }

    public class SMSSendBulkViewModel
    {       
        public int OfficeId { get; set; }
        public IEnumerable<SelectListItem> OfficeList { get; set; }
        public string SearchKey { get; set; }
        public string DateFromValue { get; set; }
        public string DateToValue { get; set; }
        public string[] OfficeIds { get; set; }
    }
}