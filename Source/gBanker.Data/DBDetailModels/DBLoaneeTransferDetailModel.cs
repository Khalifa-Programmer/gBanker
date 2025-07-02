using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gBanker.Data.DBDetailModels
{
    public class DBLoaneeTransferDetailModel
    {

            public long MemberID { get; set; }
            public string MemberCode { get; set; }
            public string MemberName { get; set; }
            public int OfficeID { get; set; }
            public string OfficeCode { get; set; }
            public string OfficeName { get; set; }
            public int CenterID { get; set; }
            public string CenterCode { get; set; }
            public string CenterName { get; set; }
            public short ProductID { get; set; }            
            public string NewMemberCode { get; set; }
            public long LoanSummaryID { get; set; }
            public int NewOfficeID { get; set; }
            public int NewCenterID { get; set; }

        
    }
}
