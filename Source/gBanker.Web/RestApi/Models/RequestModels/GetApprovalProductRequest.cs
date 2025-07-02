using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace gBanker.Web.RestApi.Models.RequestModels
{
    public class GetApprovalProductRequest
    {
        public int orgId { get; set; }
        public long memberId { get; set; }
    }
}