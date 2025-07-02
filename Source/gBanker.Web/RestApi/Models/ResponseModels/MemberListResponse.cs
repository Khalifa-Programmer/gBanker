
using gBanker.Web.RestApi.Models.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace gBanker.Web.RestApi.Models.ResponseModels
{
    public class MemberListResponse : ApiResponse
    {
        public List<Member> members { get; set; }
        public int totalCount { get; set; }
    }
}