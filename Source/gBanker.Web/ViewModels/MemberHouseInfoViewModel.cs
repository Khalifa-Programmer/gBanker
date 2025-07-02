using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace gBanker.Web.ViewModels
{
    public class MemberHouseInfoViewModel : BaseModel
    {
        public long MemberHouseID { get; set; }

        public int OrgID { get; set; }

        public int OfficeID { get; set; }

        public int CenterID { get; set; }

        public long MemberID { get; set; }

        public bool? HWPathkhari { get; set; }

        public bool? HWBash { get; set; }

        public bool? HWSchan { get; set; }

        public bool? HWPata { get; set; }

        public bool? HWMati { get; set; }

        public bool? HWTin { get; set; }

        public bool? HWOthers { get; set; }

        public bool? HRTin { get; set; }

        public bool? HRSchan { get; set; }

        public bool? HRKhar { get; set; }

        public bool? HRTinSchan { get; set; }

        public bool? HROthers { get; set; }
        
    }
}