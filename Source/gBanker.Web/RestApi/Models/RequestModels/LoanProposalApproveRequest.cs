using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace gBanker.Web.RestApi.Models.RequestModels
{
    public class LoanProposalApproveRequest
    {
        public int officeId { get; set; }
        public long memberId { get; set; }
        public int productId { get; set; }
        public double approvedAmount { get; set; }
        public string userId { get; set; }
    }
}