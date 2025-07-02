using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace gBanker.Web.ViewModels
{
    public class SearchMemberViewModel
    {
        public int maxInstallmentNo { get; set; }
        public int InsNo { get; set; }
        public int SerialNo { get; set; }
        public string MemberCode { get; set; }
        public long Memberid { get; set; }
        public string InstallmentDateMsg { get; set; } 
        public string OpeningDateMsg { get; set; }
        public int ProductID { get; set; }
        public decimal SavingInstallment { get; set; }
        public decimal Balance { get; set; }

        public int OfficeId { get; set; }
        public int CenterID { get; set; }

        public long SavingSummaryID { get; set; }
        public int NoOfAccount { get; set; }
        public DateTime OpeningDate { get; set; }
        public string CenterCode { get; set; }
        public string MemberName { get; set; }

        public string OrganaizationName { get; set; }
        public string JoiningDatemsg { get; set; }

        public string DistrictName { get; set; }
        public string DivisionName { get; set; }

        
        public string RefereeName { get; set; }

    }//END Class
}// END Namespace