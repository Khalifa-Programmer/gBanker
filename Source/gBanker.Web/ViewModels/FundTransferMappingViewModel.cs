using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace gBanker.Web.ViewModels
{
    public class FundTransferMappingViewModel 
    {
        public Int64 FundTransferMappingId { get; set; }

        [Display(Name = "Employee Id")]
        [Required(ErrorMessage = "{0} is Required")]
        public int EmployeeId { get; set; }

        [Display(Name = "Office Id")]
        [Required(ErrorMessage = "{0} is Required")]
        public int OfficeId { get; set; }
        public string CreateUser { get; set; }
        public DateTime CreateDate { get; set; }




    }
    public class FundTransferMappedOfficeViewModel
    {
        public int EmployeeId { get; set; }

        [Display(Name = "Office Id")]
        [Required(ErrorMessage = "{0} is Required")]
        public int OfficeId { get; set; }
        public string OfficeCode { get; set; }
        public string OfficeName { get; set; }
        public string CreateUser { get; set; }
        public DateTime CreateDate { get; set; }




    }//// End Class
}// END Namespace