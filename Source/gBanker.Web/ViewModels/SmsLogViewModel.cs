using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace gBanker.Web.ViewModels
{
    public class SmsLogViewModel:BaseModel
    {
        public long SmsLogID { get; set; }

        public int OrgID { get; set; }

        public long MemberID { get; set; }
        public string MemberName { get; set; }
        
        public string SmsType { get; set; }
        
        public string SmsFrom { get; set; }
        
        public string SmsTo { get; set; }
        
        public string SmsBody { get; set; }

        public DateTime? DateSent { get; set; }
        public string SmsStatus { get; set; }
        
    }
}