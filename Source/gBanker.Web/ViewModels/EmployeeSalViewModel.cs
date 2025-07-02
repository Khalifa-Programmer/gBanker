using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace gBanker.Web.ViewModels
{
    public class EmployeeSalViewModel
    {
        [Display(Name = "Employee Code")]
        public  string EmployeeCode { get; set; }
        public string EmployeeName { get; set; }
        public int EmployeeID { get; set; }
        public int OfficeID { get; set; }

        [Display(Name = "Basic ")]
        public decimal? Basic { get; set; }
        public decimal? HRent { get; set; }
        public decimal? MA { get; set; }
        public decimal? TA { get; set; }
        public decimal? PFOwn { get; set; }
        public decimal? PFOrg { get; set; }
        public decimal? FestBonus { get; set; }
        public decimal? SSF { get; set; }
        public decimal? special { get; set; }
        public decimal? distance { get; set; }
        public decimal? dearness { get; set; }
        public decimal? MobileBill { get; set; }

        public decimal? GratuityOrg { get; set; }

        public decimal? HealthFund { get; set; }
        public bool IsActive { get; set; }
        
        public DateTime? InActiveDate { get; set; }

        public string CreateUser { get; set; }

        public DateTime? CreateDate { get; set; }
        public decimal? Penalty { get; set; }
        public string SalaryDate { get; set; }

    }// End Class
}// End Namespace