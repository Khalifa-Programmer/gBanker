using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace gBanker.Web.RestApi.Models.Entity
{
    public class LoanProposalDelete
    {
        public int OfficeID { get; set; }
        public long MemberID { get; set; }
        public short ProductId { get; set; }
        public long LoanSummaryID { get; set; }
        public bool IsActive { get; set; }
    }
}