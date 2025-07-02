using gBanker.Web.RestApi.Models.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace gBanker.Web.RestApi.Models.ResponseModels
{
    public class CsRefundGetResultResponse : ApiResponse
    {
        public CsRefundGetResult csRefundGetResult;
        public string message;
    }
}