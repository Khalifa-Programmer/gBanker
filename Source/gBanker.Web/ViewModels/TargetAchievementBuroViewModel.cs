using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace gBanker.Web.ViewModels
{
    public class TargetAchievementBuroViewModel
    {
        public int TargetId { get; set; }
        public int? ParticularId { get; set; }
        public string ParticularName { get; set; }
        public decimal? Balance { get; set; }
        public decimal? TargetCurrentYear { get; set; }
        public decimal? Target { get; set; }
        public decimal? Achievement { get; set; }
        public string Date { get; set; }
        public string DateMSG { get; set; }
        public string EmployeeName { get; set; }
        public string EmployeeNameBn { get; set; }
        public int OfficeID { get; set; }
        public int ProductID { get; set; }
        public int EmployeeID { get; set; }
        public string ProductNameWithCode { get; set; }
        public IEnumerable<SelectListItem> ProductTypeList { get; set; }
        public IEnumerable<SelectListItem> ParticularList { get; set; }
        public IEnumerable<SelectListItem> EmployeeList { get; set; }

    }
}