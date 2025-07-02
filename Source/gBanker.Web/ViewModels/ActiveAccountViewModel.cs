using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace gBanker.Web.ViewModels
{
    public class ActiveAccountViewModel
    {
        
        public DateTime DateFrom { get; set; }
        public DateTime DateTo { get; set; }
        public string Type { get; set; }
        public int OfficeId { get; set; }

        public string LoanAccountNo { get; set; }
        public string MemberCode { get; set; }
        public string MemberName { get; set; }
        public string OfficeCode { get; set; }
        public string SamityCode { get; set; }
    }
}