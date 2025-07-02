using gBanker.Web.RestApi.Models.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace gBanker.Web.RestApi.Models.ResponseModels
{
    public class LoanProposalListResponse : ApiResponse
    {
        public String message { get; set; }
        public List<LoanProposal> loanProposals { get; set; }
    }
}