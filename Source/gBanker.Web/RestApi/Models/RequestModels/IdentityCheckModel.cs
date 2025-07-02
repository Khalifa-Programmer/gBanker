using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace gBanker.Web.RestApi.Models.RequestModels
{
    public class IdentityCheckModel
    {
        public string keyword { get; set; }
        public string identityType { get; set; }
    }
}