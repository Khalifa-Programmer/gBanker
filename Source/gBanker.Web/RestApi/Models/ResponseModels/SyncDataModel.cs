using gBanker.Web.RestApi.Models.Entity;
using System.Collections.Generic;
using System.Net;

namespace gBanker.Web.RestApi.Models.ResponseModels
{
    public class SyncDataModel
    {
        public HttpStatusCode status;
        public int apiVersion { get; set; }
        public List<MemberProductDetail> data { get; set; }
    }
}