using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace gBanker.Web.ViewModels
{
    public class LTSPenaltySlotViewModel
    {
        public int LTSPenaltySlotID { get; set; }
        public string PaymentFrequency { get; set; }
        public decimal? Duration { get; set; }
        [Display(Name = "Term Deposit")]
        public decimal? TermDeposit { get; set; }
        public DateTime? CreateDate { get; set; }
        public decimal? Penalty { get; set; }



    }// End Class
}// END Namespace