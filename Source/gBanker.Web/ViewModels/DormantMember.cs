using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace gBanker.Web.ViewModels
{
    public class DormantMemberViewModel
    {
        public string MemberCode { get; set; }
        public string MemberName { get; set; }
        public string FatherName { get; set; }
        public string SamityCode { get; set; }
        public string SamityName { get; set; }
        public decimal SavingsBalance { get; set; }
        public string LastTransactionDate { get; set; }
        public string DormantDate { get; set; }

    }// END Class
}// END Namespace