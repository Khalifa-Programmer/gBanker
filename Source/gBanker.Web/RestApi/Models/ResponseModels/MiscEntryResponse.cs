using gBanker.Web.RestApi.Models.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace gBanker.Web.RestApi.Models.ResponseModels
{
    public class MiscEntryResponse : ApiResponse
    {
        public MiscEntryResult mer { get; set; }
        public string message { get; set; }
    }
}