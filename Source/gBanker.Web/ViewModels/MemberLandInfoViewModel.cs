using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace gBanker.Web.ViewModels
{
    public class MemberLandInfoViewModel : BaseModel
    {
        public long MemberLandID { get; set; }

        public int OrgID { get; set; }

        public int OfficeID { get; set; }

        public int CenterID { get; set; }

        public long MemberID { get; set; }

        public bool? LandVita { get; set; }

        public bool? LandKrishie { get; set; }

        public bool? LandBagan { get; set; }

        public bool? LandPukur { get; set; }

        public bool? LandOnabade { get; set; }

        public bool? LandBondhokDeya { get; set; }

        public bool? LandBorgaDeya { get; set; }

        public bool? LandBondhokNeya { get; set; }

        public bool? LandBorgaNeya { get; set; }

        public bool? KhashLand { get; set; }
        
        public string BorgaCondition { get; set; }
       
    }
}