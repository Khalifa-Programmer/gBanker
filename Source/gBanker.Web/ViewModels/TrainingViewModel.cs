using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
namespace gBanker.Web.ViewModels
{
    public class TrainingViewModel
    {
        public int TrainingID { get; set; }
        public int? OfficeID { get; set; }
        public string TrainingType { get; set; }
        public string TrainingDate { get; set; }
        public int? NoOfParticipants { get; set; }
        public string CourseName { get; set; }
        public decimal? CostGeneralFund { get; set; }
        public decimal? CostMicroFinance { get; set; }
        public decimal? CostDonation { get; set; }
        public string OtherCostSource1 { get; set; }
        public decimal? CostAmount1 { get; set; }
        public string OtherCostSource2 { get; set; }
        public decimal? CostAmount2 { get; set; }
        public string OtherCostSource3 { get; set; }
        public decimal? CostAmount3 { get; set; }
        public List<SelectListItem> TrainingTypeList { get; set; }        
    }
}