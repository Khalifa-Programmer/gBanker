using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace gBanker.Web.ViewModels
{
    public class WeeNoViewModel
    {
        public long WeekNoID { get; set; }
        public int OrgID { get; set; }
        public int WeekYear { get; set; }
        public int WeekNoSl { get; set; }
        public DateTime? StartDate { get; set; }
        public string SD { get; set; }
        public DateTime? EndDate { get; set; }
        public string ED { get; set; }
    }
}