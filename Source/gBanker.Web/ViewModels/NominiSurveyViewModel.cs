using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace gBanker.Web.ViewModels
{
    public class NominiSurveyViewModel
    {
        public long SNO { get; set; }
        public long MemberId { get; set; }
        public long MemberNomineeId { get; set; }
        public string NomineeName { get; set; }
        public string NomineeFather { get; set; }
        public string NomineeMother { get; set; }
        public string NomineeHusbandWife { get; set; }
        public string NomineeMobileNo { get; set; }
        public string NomineeRelation { get; set; }
    }
}