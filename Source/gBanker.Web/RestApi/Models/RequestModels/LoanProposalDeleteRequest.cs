using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace gBanker.Web.RestApi.Models.RequestModels
{
    public class LoanProposalDeleteRequest
    {
        public int officeId { get; set; }
        public long memberId { get; set; }
        public int productId { get; set; }
        public long loanSummaryId { get; set; }
        public string userId { get; set; }
    }
}