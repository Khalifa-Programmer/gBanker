using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace gBanker.Web.RestApi.Models.RequestModels
{
    public class LoanProposalDisburseRequest
    {
        public int officeId { get; set; }
        public long memberId { get; set; }
        public short productId { get; set; }
        public string bankName { get; set; }
        public string chequeNo { get; set; }
        public string chequeIssueDate { get; set; }
        public string userId { get; set; }
    }
}