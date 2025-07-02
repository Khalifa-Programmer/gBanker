using gBanker.Web.RestApi.Models.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace gBanker.Web.RestApi.Models.ResponseModels
{
    public class IdentityCheckResponseModel
    {
        public string status { get; set; }
        public IdentityCheckResult member { get; set; }
    }
}