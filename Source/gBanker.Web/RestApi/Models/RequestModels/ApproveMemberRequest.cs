using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace gBanker.Web.RestApi.Models.RequestModels
{
    public class ApproveMemberRequest
    {
        public int officeId { get; set; }
        public int centerId { get; set; }
        public long memberId { get; set; }
        public int[] products { get; set; }
        public int noOfAccount { get; set; }
        public string transactionDate { get; set; }
        public int memberCategoryId { get; set; }
        public int orgId { get; set; }
        public string createUser { get; set; }
        public string createDate { get; set; }
    }
}