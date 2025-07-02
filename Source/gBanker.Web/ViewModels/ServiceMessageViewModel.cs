using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace gBanker.Web.ViewModels
{
    public class ServiceMessageViewModel
    {
        public long? rowSl { get; set; }
        public int ServiceMessageID { get; set; }
        public string ServiceMessage { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public Boolean isActive { get; set; }
        public Boolean? isOnline { get; set; }
    }
}