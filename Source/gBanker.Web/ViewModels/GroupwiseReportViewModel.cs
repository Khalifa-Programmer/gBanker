using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace gBanker.Web.ViewModels
{
    public class GroupwiseReportViewModel : BaseModel
    {
        public int OfficeID { get; set; }
        public int EmployeeID { get; set; }
        public int ProductID { get; set; }
        [Display(Name = "Samity ID")]
        public int CenterID { get; set; }
    }
}