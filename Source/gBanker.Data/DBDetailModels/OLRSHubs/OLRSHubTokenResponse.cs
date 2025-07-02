using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gBanker.Data.DBDetailModels.OLRSHubs
{
    public class OLRSHubTokenResponse
    {
        public string token { get; set; }
        public string expiration { get; set; }
        public string error { get; set; }
        public string error_description { get; set; }
        public bool isSuccess { get; set; }
        public string message { get; set; }
    }

    public class OLRSHubResponse
    {
        public bool isSuccess { get; set; } 
        public string message { get; set; }
    }

    public class OlrsSyncResponse
    {
        public bool IS_SYNCHED_TO_PKSF { get; set; }
        public string MESSAGE { get; set; }
    }

}
