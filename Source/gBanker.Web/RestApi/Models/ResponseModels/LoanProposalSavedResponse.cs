using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace gBanker.Web.RestApi.Models.ResponseModels
{
    public class LoanProposalSavedResponse : ApiResponse
    {
        public long inserted { get; set; }
    }
}