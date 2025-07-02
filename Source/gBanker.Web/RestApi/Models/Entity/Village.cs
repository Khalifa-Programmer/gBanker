using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace gBanker.Web.RestApi.Models.Entity
{
    public class Village
    {
        public string UpozillaCode { get; set; }
        public string UnionCode { get; set; }
        public string UnionName { get; set; }
        public string VillageCode { get; set; }
        public string VillageName { get; set; }
    }
}