using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace gBanker.Web.RestApi.Models.ResponseModels
{
    public class MemberImgSyncResponse : ApiResponse
    {
        public string message { get; set; }
        public bool imgSync { get; set; }
        public bool sigImgSync { get; set; }
    }
}