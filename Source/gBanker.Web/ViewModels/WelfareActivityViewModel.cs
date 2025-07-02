using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace gBanker.Web.ViewModels
{
    public class WelfareActivityViewModel
    {
        public int WelfareActivityId { get; set; }
        public int? OfficeId { get; set; }
        public string DateTo { get; set; }
        public int? ActivityId { get; set; }
        public string ActivityName { get; set; }
        public string SourceofFund { get; set; }
        public decimal? SurplusMicrofinance { get; set; }
        public decimal? SurplusOtherActivities { get; set; }
        public decimal? SurplusOwnFund { get; set; }
        public decimal? Donation { get; set; }
        public decimal? OtherSource { get; set; }
        public int? AreaCovered { get; set; }
        public int? NumberOfBeneficiaries { get; set; }
        public int? DurationOfActivity { get; set; }
        public decimal? AcitivityExpenditure { get; set; }
        public decimal? Surplus { get; set; }
        public List<SelectListItem> ActivityList { get; set; }
    }
}