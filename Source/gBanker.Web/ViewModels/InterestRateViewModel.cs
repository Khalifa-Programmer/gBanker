using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace gBanker.Web.ViewModels
{
    public class InterestRateViewModel : BaseModel
    {

        public int InterestRateID { get; set; }
        public decimal? Duration { get; set; }
        public decimal? InterestRates { get; set; }
        public DateTime? EffectDate { get; set; }
        public string EffectDateMsg { get; set; }

        public decimal? EffextYear { get; set; }

        public int? EStatus { get; set; }




    }// End Class
}// End Namespace