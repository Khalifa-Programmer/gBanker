using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace gBanker.Web.RestApi.Models.RequestModels
{
    public class GetMemberListRequest
    {
        public int officeId { get; set; }
        public string createUser { get; set; }
        public int offset { get; set; }
        public int limit { get; set; }
        public bool withoutImg { get; set; }
    }
}