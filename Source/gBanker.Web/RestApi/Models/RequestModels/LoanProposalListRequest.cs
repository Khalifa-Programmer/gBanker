using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace gBanker.Web.RestApi.Models.RequestModels
{
    public class LoanProposalListRequest
    {
        public int officeId { get; set; }
        public string userId { get; set; }
    }
}