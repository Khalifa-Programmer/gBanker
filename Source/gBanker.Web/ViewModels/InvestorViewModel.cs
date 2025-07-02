using gBanker.Web.Filters;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace gBanker.Web.ViewModels
{
    public class InvestorViewModel : BaseModel
    {
        [GlobalizedDisplayName("InvestorID")]
        public byte InvestorID { get; set; }
        [GlobalizedDisplayName("InvestorCode")]
        [Required(ErrorMessage = "Investor Code is required")]
        [StringLength(5)]
        public string InvestorCode { get; set; }
        [Required(ErrorMessage = "Investor Name is required")]
        [StringLength(50)]
        [GlobalizedDisplayName("Investor")]
        public string InvestorName { get; set; }

        public bool IsActive { get; set; }
        public Nullable<System.DateTime> InActiveDate { get; set; }

    }
}