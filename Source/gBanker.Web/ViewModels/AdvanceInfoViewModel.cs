using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace gBanker.Web.ViewModels
{
    public class AdvanceInfoViewModel
    {
        [Display(Name = "Employee Code")]
        public string EmployeeCode { get; set; }
        public string EmployeeName { get; set; }
        public int EmployeeID { get; set; }
        public DateTime? AdvanceDate { get; set; }

        public string AdvanceDateMSG { get; set; }

        [Display(Name = "AdvanceAmount ")]
        public decimal? AdvanceAmount { get; set; }
        public int AdvanceSectorId { get; set; }
        public string Remarks { get; set; }

        public decimal? preparedBy { get; set; }
        public decimal? checkedBy { get; set; }
        public decimal? approvedBy { get; set; }

        public int ShowInSalarySheet { get; set; }

        public string SectorName { get; set; }

        public DateTime CreateDate { get; set; }

        public string CreateUser { get; set; }


    } // End Class
}// END NameSpace