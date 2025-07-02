using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace gBanker.Web.ViewModels
{
    public class SmsConfigViewModel:BaseModel
    {
        public int SmsID { get; set; }

        public int? OrgID { get; set; }

        [Display(Name = "Account SID")]
        public string AccSID { get; set; }

        [Display(Name = "Auth Token")]
        public string AuthToken { get; set; }

        [Display(Name = "Phone No")]
        public string PhoneNo { get; set; }
    }
}