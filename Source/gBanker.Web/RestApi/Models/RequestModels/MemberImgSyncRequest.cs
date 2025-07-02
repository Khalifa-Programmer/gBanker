using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace gBanker.Web.RestApi.Models.RequestModels
{
    public class MemberImgSyncRequest
    {
        public int memberId { get; set; }
        public string img { get; set; }
        public string sigImg { get; set; }

    }
}