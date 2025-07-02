using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace gBanker.Web.ViewModels
{
    public class InsuranceSlotViewModel: BaseModel
    {

        public int InsuranceSlotID { get; set; }
        [Display(Name = "Duration")]
        public int Duration { get; set; }
        [Display(Name = "Minimum Amount")]
        public decimal MinAmount { get; set; }
        [Display(Name = "Maximum Amount")]
        public decimal MaxAmount { get; set; }
        [Display(Name = "Insurancen Rate")]
        public decimal InsuranceRate { get; set; }
        public decimal AmountBy { get; set; }

        [Display(Name = "Insurance Date")]
        [DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime InsuarnceDate { get; set; }
        public bool? IsRunning { get; set; }
        public int ProductID { get; set; }
        public string PaymentFrequency { get; set; }
        //public string CreateUser { get; set; }
        //public string CreateDate { get; set; }


    }// END Class
}// End Namespace