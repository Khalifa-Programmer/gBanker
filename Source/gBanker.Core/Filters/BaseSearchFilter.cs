using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace gBanker.Core.Filters
{
    public class BaseSearchFilter
    {
        public BaseSearchFilter()
        {
            SearchTerm = string.Empty;
            PageNumber = 1;
            PageSize = 20;
        }

        [Display(Name = "Search Term")]
        public string SearchTerm { get; set; }
        public int PageSize { get; set; }
        public int PageNumber { get; set; }
        public int TotalCount { get; set; }
        public int? OfficeId { get; set; }
        public string OfficeIds { get; set; }
        public int? OfficeTypeId { get; set; }
        public int? OfficeLocationId { get; set; }
        public int? DepartmentId { get; set; }
        public int? PFTypeId { get; set; }
        public int? BranchId { get; set; }
        public int? Year { get; set; }
        public int? Month { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string StartDateInString { get; set; }
        public string EndDateInString { get; set; }

        [Display(Name = "Month/Year")]
        public string MNYR { get; set; }

        [Display(Name = "FY Month")]
        public string FY_Month { get; set; }


        public string OrgCode { get; set; }
        public string PoCode { get; set; }
        public string PostingFlag { get; set; }
        public int OrganizationId { get; set; }

        [Display(Name = "Ind Code")]
        public string IND_CODE { get; set; }

        [Display(Name = "Male/Female Flag")]
        public string M_F_flag { get; set; }
        
        [Display(Name = "Synced Status")]
        public string SYNCED_STATUS { get; set; }

        [Display(Name = "Acc Group")]       
        public string ACCGROUP { get; set; }

        [Display(Name = "Employment Type")]
        public string EmploymentType { get; set; }

        public bool IsCalculateTotal { get; set; }
        public IEnumerable<SelectListItem> OfficeList { get; set; }

        public DateTime? DateFrom { get; set; }
        public DateTime? DateTo { get; set; }
        public string ReportName { get; set; }
        public string ReportType { get; set; }
        public string AssociatedTable { get; set; }
    }
}
